using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Block : MonoBehaviour
{
    public Text Number;
    public int NumberAmount;

    void Start()
    {
        float RandomNumber = Random.Range(0f, 1f);
        NumberAmount = RandomNumber > 0.1f ? GameManager.RoundNumber : GameManager.RoundNumber * 2;
        Number.text = NumberAmount.ToString();
    }

    void DecreaseAmount()
    {
        if (NumberAmount == 1)
        {
            GameManager.Instance.RemoveBlockFromList(this.gameObject.GetComponent<Image>());
            Destroy(this.gameObject);
        }
        else {
            NumberAmount--;
            Number.text = NumberAmount.ToString();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Ball" || other.transform.tag == "GhostBall")
        {
            DecreaseAmount();
        }
    }
}
