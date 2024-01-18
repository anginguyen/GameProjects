using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangman : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject key; 
    public Animator animator;

    [SerializeField] string message = "invertedflame";
    string currentMessage = "";
    char[] messageCache; 

    bool solved = false;
    bool isCorrect = false;

    void Start() {
        messageCache = new char[message.Length];

        // Already filled-in letters
        messageCache[0] = 'i';
        messageCache[3] = 'e';
        messageCache[7] = 'd';
        messageCache[9] = 'l';
        messageCache[10] = 'a';
    }

    void Update() {
        if (!solved && currentMessage == message) {
            solved = true;
            StartCoroutine(HangmanSolved());
        }
    }

    public void UpdateInput(int position, char letter) {
        messageCache[position] = letter;
        currentMessage = new string(messageCache);
    }

    IEnumerator HangmanSolved() {
        gameManager.SwitchToTransition();
        yield return new WaitForSeconds(1f); 

        animator.enabled = true; 

        yield return new WaitForSeconds(2.5f); 
        key.GetComponent<PickUp>().OnMouseDown();
        gameManager.SwitchToView();
    }
}
