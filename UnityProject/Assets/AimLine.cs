using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBall
{
    public GhostBall(GameObject go, bool isGrounded)
    {
        gameObject = go;
        grounded = isGrounded;
        rigid = go.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (!gameObject)
        {
            grounded = true;
            return;
        }

        grounded = rigid.velocity == Vector2.zero;
    }

    public GameObject gameObject;
    public Rigidbody2D rigid;
    public bool grounded;
}

public class AimLine : MonoBehaviour
{
    public float rotationSpeed = 10;
    RectTransform rect;
    public RectTransform aimLineRect;
    public RectTransform aimTriangleRect;
    public float clampMag = 10;
    const float kMaxScaleMagnitude = 3.98F;
    const float kMaxDot = -0.9621855F;
    public Vector3 minRotatedPos = new Vector3(196.9F, 57.1f, 0.0F);
    public Vector3 maxRotatedPos = new Vector3(199.3f, 56.3F, 0.0F);
    Rigidbody2D ballRigid;
    public float ballSpeed = 500.0F;
    public static bool IsFirstTimePushed;
    public float maxScale;
    public List<GhostBall> ghostBalls = new List<GhostBall>();
    public Canvas canvas;
    public Collider2D playerBall;
    public Vector2 startPos;
    public float ghostBallSpawnTime = 5;
    public float ghostBallSpawnDelay = 5;

    public bool playerBallGrounded;
    public bool othersGrounded;
    public bool isRoundOver;
    public event System.Action OnRoundOver;

    bool buttonUp;

    public bool IsRoundOver
    {
        get
        {
            return isRoundOver;
        }

        set
        {
            if (OnRoundOver != null && value)
                OnRoundOver.Invoke();

            isRoundOver = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        rect = GetComponent<RectTransform>();
        ballRigid = GetComponent<Rigidbody2D>();
        aimLineRect.gameObject.SetActive(false);
        aimTriangleRect.gameObject.SetActive(false);

        IsRoundOver = true;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(ghostBalls.Count);

        foreach (var ball in ghostBalls)
            ball.Update();

        playerBallGrounded = ballRigid.velocity == Vector2.zero;
        othersGrounded = true;
        foreach (var ball in ghostBalls)
        {
            if (!ball.grounded)
            {
                othersGrounded = false;
                break;
            }
        }

        if (ghostBalls.Count == GameManager.Instance.Ballz)
            CancelInvoke("SpawnGhostBall");

        Debug.Log(ballRigid.velocity);
        if (ballRigid.velocity == Vector2.zero)
        {
            Debug.Log(IsRoundOver);
            if(IsRoundOver)
            {
                if (Input.GetMouseButton(0))
                {
                    aimLineRect.gameObject.SetActive(true);
                    aimTriangleRect.gameObject.SetActive(true);
                    var v = -Input.GetAxis("Mouse Y");
                    var h = -Input.GetAxis("Mouse X");
                    rect.RotateAround(rect.position, new Vector3(0, 0, 1), h * rotationSpeed * Time.deltaTime);
                    aimLineRect.localScale += new Vector3(v, v, v);
                    var clampScaleX = Mathf.Clamp(aimLineRect.localScale.x, 0, maxScale);
                    var clampScaleY = Mathf.Clamp(aimLineRect.localScale.y, 0, maxScale);
                    var clampScaleZ = Mathf.Clamp(aimLineRect.localScale.z, 0, maxScale);
                    aimLineRect.localScale = new Vector3(clampScaleX, clampScaleY, clampScaleZ);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    IsRoundOver = false;

                    if (ghostBalls.Count < GameManager.Instance.Ballz)
                        InvokeRepeating("SpawnGhostBall", ghostBallSpawnTime, ghostBallSpawnDelay);

                    IsFirstTimePushed = true;
                    aimLineRect.gameObject.SetActive(false);
                    aimTriangleRect.gameObject.SetActive(false);
                    ballRigid.velocity += (Vector2)aimLineRect.up * 15000 * Time.fixedDeltaTime * ballSpeed;
                }
            }
        }
    }


    void SpawnGhostBall()
    {
        var clone = Instantiate(GameManager.Instance.playerBall.gameObject, startPos, playerBall.transform.rotation, canvas.transform);
        Physics2D.IgnoreCollision(clone.GetComponent<Collider2D>(), playerBall, true);

        foreach (var ball in ghostBalls)
        {
            Physics2D.IgnoreCollision(clone.GetComponent<Collider2D>(), ball.gameObject.GetComponent<Collider2D>(), true);
        }

        clone.GetComponent<PlayerBall>().enabled = false;
        clone.GetComponent<AimLine>().enabled = false;
        clone.tag = "GhostBall";
        ghostBalls.Add(new GhostBall(clone, false));

        var rigid = clone.GetComponent<Rigidbody2D>();
        rigid.velocity += (Vector2)aimLineRect.up * ballSpeed * 15000 * Time.fixedDeltaTime;
    }

    void Draw()
    {
    }
}
