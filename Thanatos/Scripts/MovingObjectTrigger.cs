using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectTrigger : MonoBehaviour
{
    public GameObject objects;
    public AudioClip[] sounds; 
    public bool hidden;
    bool triggered;

    void Start() {
        triggered = false;
    }

    void OnTriggerEnter(Collider col) {
        if (!triggered) {
            triggered = true;
            if (hidden) objects.SetActive(true);
            else objects.GetComponent<Animator>().enabled = true; 
            foreach (AudioClip sound in sounds) AudioSource.PlayClipAtPoint(sound, transform.position);
        }
    }
}
