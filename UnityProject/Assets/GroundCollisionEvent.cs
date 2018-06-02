using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroundCollisionEvent : MonoBehaviour
{
    public AimLine aimLine;
    public float MoveToPlayerBallSpeed = 10;

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.tag == "Ball")
        {
            if (AimLine.IsFirstTimePushed)
            {
                collision2D.collider.attachedRigidbody.velocity = Vector2.zero;
                aimLine.startPos = collision2D.collider.transform.position;
                GameManager.Instance.playerBall.aimLine.aimLineRect.localScale = new Vector3(0.1F, 0.1F, 0.1F);
            }
        }
        else if(collision2D.collider.tag == "GhostBall")
        {
            collision2D.collider.attachedRigidbody.velocity = Vector2.zero;

            StartCoroutine(MoveToPlayerBall(collision2D.collider));
        }
    }

    IEnumerator MoveToPlayerBall(Collider2D collider)
    {
        while (true)
        {
            if (Mathf.Approximately(collider.transform.position.magnitude, aimLine.playerBall.transform.position.magnitude))
            {
                Destroy(collider.gameObject);
                aimLine.ghostBalls.RemoveAt(aimLine.ghostBalls.Count - 1);

                if (aimLine.ghostBalls.Count == 0)
                    aimLine.IsRoundOver = true;

                yield break;
            }

            if (aimLine.playerBallGrounded && aimLine.othersGrounded)
            {
                collider.transform.position = Vector2.MoveTowards(
                    collider.transform.position,
                    aimLine.playerBall.transform.position, Time.deltaTime * MoveToPlayerBallSpeed);
            }

            yield return Time.deltaTime;
        }
    }
}
