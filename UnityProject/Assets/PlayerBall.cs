using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AimLine))]
public class PlayerBall : MonoBehaviour
{
    private Rigidbody2D Rb2D;
    float sx;
    float sy;
    public static Vector2 position;
    public AimLine aimLine;

    void Start()
    {
        //aimLine = GetComponent<AimLine>();
        //Rb2D = GetComponent<Rigidbody2D>();
        //sx = Random.Range(0, 2) == 0 ? -1 : 1;
        //sy = Random.Range(0, 2) == 0 ? -1 : 1;

        //Rb2D.velocity = new Vector3(400 * sx, 400 * sy, 0);
    }


    void Update()
    {
        position = transform.position;
    }
}
