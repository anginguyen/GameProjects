using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    public GameManager gameManager;
    bool entered;

    void Start() {
        entered = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !entered) {
            entered = true;
            gameManager.LoadNextLevel();
        }
    }
}
