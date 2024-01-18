using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangmanPosition : MonoBehaviour
{
    [SerializeField] Hangman hangman;
    [SerializeField] int position;

    string letter = "";
    // Transform oldParent;

    void OnTriggerStay(Collider other) {
        if (other.tag == "Hangman Letter" && !other.gameObject.GetComponent<Draggable>().isDragging && letter == "") {
            letter = other.gameObject.name;
            hangman.UpdateInput(position, letter[0]);
            // SnapToPosition(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Hangman Letter" && other.gameObject.name == letter) {
            letter = "";
            hangman.UpdateInput(position, ' ');
            // other.transform.SetParent(oldParent);
        }
    }

    // void SnapToPosition(GameObject letter) {
    //     oldParent = letter.transform.parent;
    //     letter.transform.SetParent(this.gameObject.transform);
    //     letter.GetComponent<Draggable>().SetPosition(new Vector3(0, 1, 0));
    // }
}
