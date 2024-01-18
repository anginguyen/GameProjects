using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] borders;
    public Collider[] wallColliders;

    public GameObject clueCamera;
    public Material indicator;

    AudioSource clickSound;

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
            SetWallColliders(false);
            SetBorders(false);
            gameManager.MoveToView();
        }
        // View to Move
        else {
            SetWallColliders(true);
            SetBorders(true);
            gameManager.ViewToMove();
        }
    }

    // Turn on border indicator when player is looking at blackboard 
    public void EnableIndicator() {
        foreach (GameObject border in borders) {
            border.GetComponent<ClueIndicator>().EnableIndicator(indicator);
        }
    }

    public void DisableIndicator() {
        foreach (GameObject border in borders) {
            border.GetComponent<ClueIndicator>().DisableIndicator();
        }
    }

    // Enable/disable indicator borders
    void SetBorders(bool isActive) {
        foreach (GameObject border in borders) {
            border.SetActive(isActive);
        }
    }

    // Enable/disable wall colliders
    void SetWallColliders(bool isActive) {
        foreach (Collider col in wallColliders) {
            col.enabled = isActive;
        }
    }
}
