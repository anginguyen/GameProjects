using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogHome : MonoBehaviour
{
    public GameObject frog, scoreKeeper;
    public GameObject timerBar;
    private Renderer frogHomeRend;
    int homePoints, bonusPoints;

    void Start() {
        frogHomeRend = GetComponent<Renderer>();
        frogHomeRend.enabled = false;
        frog = GameObject.FindWithTag("Player");
        timerBar = GameObject.FindWithTag("Timer");
        scoreKeeper = GameObject.Find("Hi-Score UI");
        homePoints = 50;
        
    }

    void Update()
    {
        bonusPoints = (int)timerBar.GetComponent<Timer>().timeLeft * 20;
    }

    public void ActivateHomeFrog() {
        scoreKeeper.GetComponent<ScoreManager>().homesFilled += 1;
        frogHomeRend.enabled = true;
        frog.GetComponent<Frog>().Respawn();
        gameObject.GetComponent<AudioSource>().Play();
        scoreKeeper.GetComponent<ScoreManager>().score += (homePoints + bonusPoints);
        frog.GetComponent<Frog>().furtherPosition.y = frog.GetComponent<Frog>().defaultPosition.y - 1;
        timerBar.GetComponent<Timer>().ResetTime();
    }
}
