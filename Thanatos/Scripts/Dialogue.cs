using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public DialogueManager dialogueManager; 
    
    [SerializeField] Animator animator;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text instructionText;

    Image dialogueBox; 

    bool isLoading; 
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isLoading = false;
        dialogueText.text = "";
        if (nameText) nameText.text = "";
    }

    void Update() {
        if (!isLoading && Input.GetKey(KeyCode.Space)) {
            string name = dialogueManager.GetCurrentName();
            if (name == "") {
                isLoading = true;
                dialogueManager.EndEvent.Invoke();
            }
            else TriggerDialogue(name);
        }
    }

    public void TriggerDialogue(string name) {
        if (nameText) nameText.text = name;
        dialogueText.text = "";
        StartCoroutine(ShowDialogue(dialogueManager.GetCurrentDialogue()));
    }

    IEnumerator ShowDialogue(string dialogue) {
        if (!isLoading) {
            if (animator) animator.SetBool("Dialogue", true);
            isLoading = true; 

            yield return new WaitForSeconds(0.25f);

            for (int i=0; i < dialogue.Length; ++i) {
                dialogueText.text += dialogue[i];
                yield return new WaitForSeconds(0.03f);
            }

            isLoading = false;
            if (instructionText) instructionText.enabled = true; 
            if (animator) animator.SetBool("Dialogue", false);
            dialogueManager.NextDialogue();
        }
    }

    public IEnumerator DisplayDialogueBox(float time) {
        yield return new WaitForSeconds(time);

        if (nameText) nameText.enabled = true;
        dialogueText.enabled = true;

        yield return new WaitForSeconds(0.5f);

        string name = dialogueManager.GetCurrentName();
        TriggerDialogue(name);
    }
}
