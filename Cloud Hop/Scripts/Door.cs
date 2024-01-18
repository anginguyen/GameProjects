using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject player;

    SpriteRenderer sr; 
    public Sprite[] doorOpen;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            StartCoroutine("OpenAnimation");
        }
    }

    IEnumerator OpenAnimation() {
        for (int i=0; i < 3; ++i) {
            yield return new WaitForSeconds(0.05f);
            sr.sprite = doorOpen[i]; 
        }
    }
}