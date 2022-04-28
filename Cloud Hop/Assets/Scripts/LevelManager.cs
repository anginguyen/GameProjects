using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerPrefab;

    Bunny playerScript;
    Bunny playerPrefabScript;

    public GameObject[] playerLivesUI;

    public int cloudsHopped;
    public int totalClouds;

    public bool playerDead;
    public bool levelComplete;
   
    public TMPro.TMP_Text cloudsHoppedDisplay;
    public GameObject gameOverScreen;
    public GameObject levelUpScreen;

    void Start()
    {
        playerScript = player.GetComponent<Bunny>();
        playerPrefabScript = playerPrefab.GetComponent<Bunny>();
        cloudsHopped = 0;
        playerDead = false;
        levelComplete = false;
        cloudsHoppedDisplay.text = "0/" + totalClouds.ToString();

        // Show the number of lives player has left
        for (int i=0; i < playerPrefabScript.lives; ++i) {
            playerLivesUI[i].SetActive(true);
        }
    }

    void Update()
    {
        // Restarts the level when player dies
        if (playerDead) {
            playerDead = false;
            playerPrefabScript.lives--;

            // If player loses all lives, show Game Over screen and reset lives
            if (playerPrefabScript.lives <= 0) {
                playerPrefabScript.lives = 3;
                gameOverScreen.SetActive(true);
                playerLivesUI[0].SetActive(false);
                Invoke("LoadCredits", 3f);
            }
            else {
                Invoke("ReloadScene", 2f);
            }
        }

        // Advances player to next level when they complete the current one
        if (levelComplete) {
            levelComplete  = false;
            playerPrefabScript.lives = 3;
            levelUpScreen.SetActive(true);

            // Loads credits if this is the last level 
            if (SceneManager.GetActiveScene().name == "Level3") {
                Invoke("LoadCredits", 3f);
            }
            else {
                Invoke("LoadNextScene", 1.20f);
            }
        }
    }

    // Increment number of clouds hopped and update the Clouds Hopped text
    public void IncrementCloudsHopped() {
        cloudsHopped++;
        cloudsHoppedDisplay.text = cloudsHopped.ToString() + "/" + totalClouds.ToString();
    }

    void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void LoadCredits() {
        SceneManager.LoadScene("Credits");
    }
}
