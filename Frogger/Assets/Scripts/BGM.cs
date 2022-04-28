using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public GameObject timer, frog, lives;
    public AudioClip gameOverOST;
    bool gameIsOver;
    public bool froggerDied;
    int startingLives; 

    // Start is called before the first frame update
    void Start()
    {
        startingLives = lives.GetComponent<Lives>().frogLives;
        gameObject.GetComponent<AudioSource>().Play();
        gameIsOver = false;
        froggerDied = false;
    }
    private void Update()
    {
        if (lives.GetComponent<Lives>().frogLives == 0 && !gameIsOver)
        {
            gameObject.GetComponent<AudioSource>().Stop();
            gameObject.GetComponent<AudioSource>().clip = gameOverOST;
            gameObject.GetComponent<AudioSource>().Play();
            gameIsOver = true;
        }

        if (lives.GetComponent<Lives>().frogLives < startingLives && froggerDied == true)
        {
            gameObject.GetComponent<AudioSource>().Stop();
            Invoke("ResumeMusic", 1f); // Resumes music after Frogger dies.
            frog.GetComponent<Frog>().furtherPosition.y = frog.GetComponent<Frog>().defaultPosition.y - 1; // Resets further position to default to re-enable scoring.
        }
    }
    void ResumeMusic()
    {
        gameObject.GetComponent<AudioSource>().Play();
        froggerDied = false;
    }
}
