using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Ballz = 3;

    public GameObject canvas;
    public PlayerBall playerBall;

    // Wave information
    public static int RoundNumber = 0;
    public Text RoundText;

    // Image information
    public Image BlockPrefab;
    public List<Image> CurrentImages; // DONT SET IT TO PRIVATE.... MAJOR PROBLEMS!!! PLEASE STAY OF GOT IT? NO....

    // Block place information
    public RectTransform[] PlacePlatforms;
    public bool[] Platforms;
    public GameObject gameOverScreen;

    void Start()
    {
        Instance = this;
        playerBall.aimLine.OnRoundOver += OnRoundOver;
        UpdateText();
    }

    private void OnRoundOver()
    {
        NewRound();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        NewRound();
    //    }
    //}

    public void RemoveBlockFromList(Image Name)
    {
        CurrentImages.Remove(Name);
    }

    void NewRound()
    {
        RoundNumber++;
        int AmountofBlocks = Random.Range(1, 7);
        for (int i = 0; i < AmountofBlocks; i++)
        {
            int RandomPosition = Random.Range(0, 7);

            do
            {
                RandomPosition = Random.Range(0, 7);
            } while (Platforms[RandomPosition] == true);

            Image TempImage = Instantiate(BlockPrefab, PlacePlatforms[RandomPosition].rect.position, BlockPrefab.rectTransform.rotation) as Image;
            TempImage.transform.SetParent(canvas.transform);
            TempImage.transform.position = PlacePlatforms[RandomPosition].position;
            TempImage.transform.localScale = new Vector3(1, 1, 1);
            Platforms[RandomPosition] = true;

            CurrentImages.Add(TempImage);
        }
        for (int i = 0; i < Platforms.Length; i++)
        {
            if (Platforms[i] == true)
            {
                Platforms[i] = false;
            }
        }
        foreach (Image image in CurrentImages)
        {
            image.transform.position += new Vector3(0, -55, 0);
            if (image.transform.localPosition.y < -560f)
            {
                playerBall.aimLine.isRoundOver = true;
                Time.timeScale = 0;
                gameOverScreen.transform.SetAsLastSibling();
                gameOverScreen.SetActive(true);
            }
        }
        UpdateText();
    }

    void UpdateText()
    {
        RoundText.text = RoundNumber.ToString();
    }

    public void Replay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");
    }
}
