using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{
        set; get;
    }
    public float CoinScoreAmout = 5;
    private bool isGameStarted = false;
    public bool isDead { set; get; }
    private PlayerController controller;

    //UI and the UI fields
    public Animator gameCanvas;
    public TextMeshProUGUI scoreText, coinText, modifierText, highScoreText;

    private float score, coinScore, modifierScore;
    private int lastScore;
    


    //Death Menu
    public Animator deathMenuAnim;
    public TextMeshProUGUI deadScoreText, deadCoinText;


    //cameras
    public Camera introCam;
    public Camera mainCam;

    void Awake(){

        Instance =this;
        modifierScore = 1;
        modifierText.text = "x" + modifierScore.ToString("0.0");
        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        introCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);

        highScoreText.text = PlayerPrefs.GetInt("Hiscore").ToString();
    }

    void Update()
    {
           if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
            highScoreText.gameObject.SetActive(false);
           // FindObjectOfType<BuildingSpawner>().IsScrolling = true;
            mainCam.gameObject.SetActive(true);
            introCam.gameObject.SetActive(false);
            mainCam.GetComponent<CameraMotor>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
        }

        if (isGameStarted && !isDead)
        {
            //Bump Score Up
         
            score += (Time.deltaTime * modifierScore);
            if(lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
         
        }
    }

    public void GetCoin()
    {
        coinScore ++;
        coinText.text = coinScore.ToString();
        score += CoinScoreAmout;
        scoreText.text = score.ToString("0");
    }
  

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnDeath()
    {
        isDead = true;
        //FindObjectOfType<BuildingSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

        //check if highscore
        if(score > PlayerPrefs.GetInt("Hiscore"))
        {
            float s = score;
            if (s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("Hiscore", (int)s);
        }
    }
}
