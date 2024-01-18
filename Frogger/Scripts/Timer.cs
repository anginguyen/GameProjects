using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 30f;
    public float timeLeft;
    int currentLives, livesRemaining;
    public GameObject timeUpUI, frog, lives, score, music;
    public AudioClip timeUp, timeLow;
    static bool timeUpStatus, timeLowStatus;

    // Start is called before the first frame update
    void Start()
    {
        timerBar = gameObject.GetComponent<Image>();
        frog = GameObject.FindWithTag("Player");
        music = GameObject.Find("MusicManager");
        score = GameObject.Find("Hi-Score UI");
        timeLeft = maxTime;
        StartCoroutine("TimerBarFiller");
        timeUpUI.SetActive(false);
        timeUpStatus = false;
        timeLowStatus = false;
        currentLives = lives.GetComponent<Lives>().frogLives;
    }
    void Update()
    {
        // Player still has lives and time left  
        if (timeLeft > 0 && lives.GetComponent<Lives>().frogLives > 0 && music.GetComponent<BGM>().froggerDied == false && score.GetComponent<ScoreManager>().gameWin == false)
        {
            // Decrement time left and the timer bar
            timeLeft -= Time.deltaTime;
            TimerBarFiller();

            // Change the timer bar color to red when there is less than 5.5 seconds left
            if (timeLeft < 5.5)
            {
                timerBar.color = Color.red;

                // Plays a "Low Time" audio once
                if (!timeLowStatus) {
                    gameObject.GetComponent<AudioSource>().PlayOneShot(timeLow);
                    timeLowStatus = true;
                }
            }
        }

        // Player runs out of time
        else if (timeLeft < 0) {
            timeUpUI.SetActive(true);

            // Plays a "Times Up" audio once and kills player
            if (!timeUpStatus && lives.GetComponent<Lives>().frogLives != 0) {
                gameObject.GetComponent<AudioSource>().PlayOneShot(timeUp);
                frog.GetComponent<Frog>().RoadDeath();
                timeUpStatus = true;
                Invoke("ResetTime", 1.75f);
            }
        }
    }

    // Resets timer after player activates Home Frog
    public void ResetTime()
    {
        timeLeft = maxTime;
        timerBar.color = Color.green;
        timeUpUI.SetActive(false);
    }

    IEnumerator TimerBarFiller()
    {
        while (true)
        {
            timerBar.fillAmount = timeLeft/maxTime;
            yield return new WaitForSeconds(0.5f); // Decrements evert 0.5f seconds.
        }
    }
}