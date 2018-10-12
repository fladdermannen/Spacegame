using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreLoader : MonoBehaviour {

    public GameObject label1, label2, label3;

    private void Start()
    {
        LoadHighscores();
    }

    public void LoadHighscores()
    {
        
        if (PlayerPrefs.HasKey("highscore1"))
        {
            Highscores.highscore1 = PlayerPrefs.GetInt("highscore1");
            label1.GetComponent<Text>().text = "" + Highscores.highscore1;
        }
        if (PlayerPrefs.HasKey("highscore2"))
        {
            Highscores.highscore2 = PlayerPrefs.GetInt("highscore2");
            label2.GetComponent<Text>().text = "" + Highscores.highscore2;
        }
        if (PlayerPrefs.HasKey("highscore3"))
        {
            Highscores.highscore3 = PlayerPrefs.GetInt("highscore3");
            label3.GetComponent<Text>().text = "" + Highscores.highscore3;
        }
    }


}
