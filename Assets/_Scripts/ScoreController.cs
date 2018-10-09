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
    }

    public void EnableScore()
    {
        stopScore = false;
    }
}
