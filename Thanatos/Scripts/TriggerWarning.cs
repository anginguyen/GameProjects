using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerWarning : MonoBehaviour
{
    public Animator animator;
    public float timer;
    public AudioClip phoneRingingSound;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0){
            StartCoroutine(Transition());
            timer = 1000f;
        }
    }

    IEnumerator Transition(){
        animator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1.5f);
        AudioSource.PlayClipAtPoint(phoneRingingSound, transform.position);
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
