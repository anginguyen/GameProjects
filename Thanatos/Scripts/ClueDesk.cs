using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClueDesk : MonoBehaviour
{
    public GameManager gameManager;
    public ClueIndicator[] clues;
    public GameObject clueCamera;

    public TMP_Text exitText; 

    AudioSource clickSound;

    public Material indicator;

    void Start() {
        clickSound = GetComponent<AudioSource>();
    }

    void Update() {
        if (gameManager.currState == GameManager.State.ViewObject && clueCamera.activeSelf) {
            if (Input.GetKeyDown(KeyCode.E)) {
                ToggleClue();
            }
        }
    }

    public void ToggleClue() {
        Invoke("SwitchStates", 0.001f);
        clueCamera.SetActive(!clueCamera.activeSelf);
        clickSound.Play();
    }

    void SwitchStates() {
        // Move to View
        if (gameManager.currState == GameManager.State.Move) {
            if (exitText) {
                exitText.enabled = true; 
                gameManager.DisableInstructionText();
            }
            NormalClue();
            gameManager.MoveToView();
        }
        // View to Move
        else {
            if (exitText) exitText.enabled = false; 
            if (gameManager.blacklightOn) NoGlow();
            gameManager.ViewToMove();
        }
    }

    // Turn on clue indicator when player is looking at desk 
    public void EnableIndicator() {
        foreach (ClueIndicator clue in clues) {
            clue.EnableIndicator(indicator);
        }
    }

    public void DisableIndicator() {
        foreach (ClueIndicator clue in clues) {
            if (!gameManager.blacklightOn) clue.DisableIndicator();
            else clue.NoGlow();
        }
    }

    public void NoGlow() {
        foreach (ClueIndicator clue in clues) {
            if (clueCamera.activeSelf) clue.DisableIndicator();
            else clue.NoGlow();
        }
    }

    public void NormalClue() {
        foreach (ClueIndicator clue in clues) {
            clue.NormalClue();
        }
    }
}
