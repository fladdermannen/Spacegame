using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public GameManager gameManager;

    private int score;
    private bool stopScore = true;

    private void Start()
    {
        score = 0;
    }

    private void Update()
    {
        if (!stopScore)
        {
            gameObject.GetComponent<Text>().text = "Score: " + score;
            score++;
        }

    }


    public int GetScore()
    {
        return score;
    }

    public void AddPoints(int p)
    {
        score += p;
    }

    public void StopScore()
    {
        stopScore = true;
        SubmitNewHighscore(score);
    }

    public void EnableScore()
    {
        stopScore = false;
    }

    private void SaveHighscores()
    {
        PlayerPrefs.SetInt("highscore1", Highscores.highscore1);
        PlayerPrefs.SetInt("highscore2", Highscores.highscore2);
        PlayerPrefs.SetInt("highscore3", Highscores.highscore3);
    }

    public void SubmitNewHighscore(int newScore)
    {
        if (Highscores.highscore1 == 0)
        {
            Highscores.highscore1 = newScore;
            SaveHighscores();
        }
        else if (newScore > Highscores.highscore1)
        {
            Highscores.highscore3 = Highscores.highscore2;
            Highscores.highscore2 = Highscores.highscore1;
            Highscores.highscore1 = newScore;
            SaveHighscores();
        }
        else if (newScore > Highscores.highscore2 && newScore < Highscores.highscore1)
        {
            Highscores.highscore3 = Highscores.highscore2;
            Highscores.highscore2 = newScore;
            SaveHighscores();
        }
        else if (newScore > Highscores.highscore3 && newScore < Highscores.highscore2)
        {
            Highscores.highscore3 = newScore;
            SaveHighscores();
        }
    }
}
