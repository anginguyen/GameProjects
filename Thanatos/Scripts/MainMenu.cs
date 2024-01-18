using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;

    public void StartGame() {
        StartCoroutine(Transition());
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator Transition() {
        animator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
