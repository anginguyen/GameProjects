using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public AudioClip openSound;
    Animator anim;
    bool isOpen;

    void Start() {
        anim = GetComponent<Animator>();
        isOpen = false;
    }

    void OnMouseDown() {
        if (isOpen) {
            anim.SetBool("Open", false);
            isOpen = false;
        }
        else {
            anim.SetBool("Open", true);
            isOpen = true;
        }
        AudioSource.PlayClipAtPoint(openSound, transform.position, 0.15f);
    }
}
