using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsGirlTrigger : MonoBehaviour
{
    public GameObject girl;
    Animator anim; 
    AudioSource stinger; 
    bool triggered;

    void Start() {
        anim = girl.GetComponent<Animator>();
        stinger = GetComponent<AudioSource>();
        triggered = false;
    }

    void OnTriggerEnter(Collider col) {
        if (!triggered) {
            StartCoroutine(Trigger());
        }
    }

    IEnumerator Trigger() {
        triggered = true; 
        anim.enabled = true;
        stinger.Play();
        yield return new WaitForSeconds(1f); 
        Destroy(girl);
    }
}
