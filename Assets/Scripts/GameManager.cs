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
    private PlayerController controller;

    //UI and the UI fields
    public TextMeshProUGUI scoreText, coinText, modifierText;

    private float score, coinScore, modifierScore;
    private int lastScore;

    void Awake(){

        Instance =this;
        modifierScore = 1;
        modifierText.text = "x" + modifierScore.ToString("0.0");
        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
           if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
        }

        if (isGameStarted)
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
}
