using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score, highScore, homesFilled;
    public TMPro.TMP_Text scoreDisplay, highScoreDisplay;
    public bool gameWin;
    public GameObject gameWinUI, lifeTracker, scoreTracker, timeTracker, musicTracker, frog;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreDisplay.text = score.ToString("00000");
        highScoreDisplay.text = highScore.ToString("00000");
        homesFilled = 0;
        gameWin = false;
        lifeTracker = GameObject.Find("Lives");
        musicTracker = GameObject.Find("MusicManager");
        scoreTracker = GameObject.Find("Hi-Score UI");
        timeTracker = GameObject.Find("Timer Bar");
        frog = GameObject.Find("Frogger");
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = score.ToString("00000");
        highScoreDisplay.text = highScore.ToString("00000");

        if (score > highScore)
        {
            highScore = score; 
        }

        if (homesFilled == 5)
        {
            musicTracker.GetComponent<AudioSource>().Stop();
            frog.GetComponent<Frog>().canMove = false;
            score += 1000;
            homesFilled = 0;
            gameWin = true;
            gameWinUI.SetActive(true);
            gameObject.GetComponent<AudioSource>().Play();
            Invoke("GameWin", 9f);
        }
    }
    void GameWin()
    {
    SceneManager.LoadScene("Game Win");
    } 
}
