using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   

public class Lives : MonoBehaviour
{
    public int frogLives;
    public GameObject life1, life2, gameOverUI;
    // Start is called before the first frame update

    void Start()
    {
        frogLives = 3;
        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (frogLives == 2)
        {
            life2.SetActive(false);
        }
        if (frogLives == 1)
        {
            life1.SetActive(false);
        }
        if (frogLives == 0)
        {
            gameOverUI.SetActive(true);
            Invoke("GameOver", 6.25f);
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}
