using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMN :Singleton <ScoreMN>
{
    [SerializeField] Text txtMyScore;
    [SerializeField] Text txtHighScore;

    private int highScore;
    private int myScore;
    private void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        myScore = 0;
        txtHighScore.text = highScore.ToString();
        txtMyScore.text = myScore.ToString () ;
    }

    public void SetMyScore(int _score)
    {
        myScore += _score;
        txtMyScore.text = myScore.ToString();
    }
    
    public void SetHighScore ()
    {
        if (myScore  > highScore)
        {
            highScore = myScore;
            PlayerPrefs.SetInt("highScore", highScore);
        }
    }

}
