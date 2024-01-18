using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public GameObject endingScreen;
    public Animator animator;
    public Dialogue dialogueBox;
    public UnityEvent EndEvent;
    
    AudioSource backgroundMusic;
    float volume;

    [SerializeField] string[] names;
    [SerializeField] string[] dialogue;

    [SerializeField] float dialogueTime;

    public bool isEnding;

    int currentDialogue;

    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        volume = backgroundMusic.volume;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentDialogue = 0;
        StartCoroutine(dialogueBox.DisplayDialogueBox(dialogueTime));
    }

    public string GetCurrentName() {
        if (currentDialogue < names.Length) return names[currentDialogue];
        return "";
    }

    public string GetCurrentDialogue() {
        if (currentDialogue < dialogue.Length) return dialogue[currentDialogue];
        return "";
    }

    public void NextDialogue() {
        currentDialogue++;
    }

    public void LoadNextScene() {
        if (!isEnding) StartCoroutine(TransitionToGame());
        else StartCoroutine(TransitionToEnd());
    }

    IEnumerator TransitionToGame() {
        animator.SetBool("FadeOut", true);

        // Fade out background music 
        float timer = 0;
        while (timer < 1f) {
            timer += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(volume, 0f, timer);
            yield return null;
        }

        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    IEnumerator TransitionToEnd() {
        yield return new WaitForSeconds(15.5f);
        
        // Fade out background music 
        float timer = 0;
        while (timer < 1f) {
            timer += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(volume, 0f, timer);
            yield return null;
        }

        endingScreen.SetActive(true); 
        yield return new WaitForSeconds(5f);

        animator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
