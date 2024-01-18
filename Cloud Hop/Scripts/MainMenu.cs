using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject bunnyPrefab;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        bunnyPrefab.GetComponent<Bunny>().lives = 3;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
