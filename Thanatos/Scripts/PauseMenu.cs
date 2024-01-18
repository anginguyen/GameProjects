using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject otherUI;
    public GameManager gameManager;

    public Slider slider;
    public float SensSlider;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                gameManager.LockPlayerCursor();
            }
            else
            {
                Pause();
                gameManager.UnlockPlayerCursor();
            }
        }

        SensSlider = slider.value;
        PlayerPrefs.SetFloat("sensitivity", SensSlider);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        otherUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        gameManager.LockPlayerCursor();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        otherUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game");
    }
}
