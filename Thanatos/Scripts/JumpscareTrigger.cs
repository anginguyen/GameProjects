using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public GameObject FlashImg;
    AudioSource stinger;
    bool jumpscare;

    void Awake(){
        stinger = GetComponent<AudioSource>();
        jumpscare = false;
    }

    void OnTriggerEnter(){
         if (!jumpscare) {
            stinger.Play();
            FlashImg.SetActive(true);
            jumpscare = true;
            StartCoroutine(EndJumpscare());
        }
    }

    IEnumerator EndJumpscare(){
        yield return new WaitForSeconds(0.2f);
        FlashImg.SetActive(false);
    }
}
