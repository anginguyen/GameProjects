using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public Animator fadeAnimator; 
    Animator anim; 

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ShowCredits());
    }

    IEnumerator ShowCredits() {
        yield return new WaitForSeconds(1f);

        anim.enabled = true;
        yield return new WaitForSeconds(20f);

        fadeAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
