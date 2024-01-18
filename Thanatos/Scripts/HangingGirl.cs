using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingGirl : MonoBehaviour
{
    public GameObject girl;
    public Animator chair;
    public AudioClip chairSound;

    float timer; 
    bool isFlashing; 
    bool triggered;

    void Start() {
        timer = Random.Range(7f, 15f); 
        isFlashing = false;
        triggered = false;
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0f && !isFlashing) {
            StartCoroutine(FlashGirl());
        }
    }

    void OnTriggerEnter(){
        if (!triggered) {
            triggered = true; 
            chair.enabled = true;
            AudioSource.PlayClipAtPoint(chairSound, chair.gameObject.transform.position);
            StartCoroutine(FlashGirl());
        }
    }

    IEnumerator FlashGirl() {
        isFlashing = true;

        for (int i=0; i < 3; ++i) {
            girl.SetActive(true);
            yield return new WaitForSeconds(0.15f); 
            girl.SetActive(false);
            yield return new WaitForSeconds(0.15f); 
        }

        isFlashing = false; 
        timer = Random.Range(5f, 10f); 
    }
}
