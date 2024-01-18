using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayGirl : MonoBehaviour
{
    public GameObject girl;
    public AudioManager audioManager;

    public AudioClip stinger;
    public AudioClip cryingSound;
    public bool flashes; 

    bool triggered;
    
    void Start()
    {
        triggered = false;
    }

    void OnTriggerEnter(){
        if (!triggered) {
            triggered = true; 
            if (flashes) StartCoroutine(FlashGirl());
            else StartCoroutine(ShowGirl());
            audioManager.PlayStinger(stinger);
        }
    }

    IEnumerator FlashGirl() {
        for (int i=0; i < 5; ++i) {
            girl.SetActive(true);
            yield return new WaitForSeconds(0.12f); 
            girl.SetActive(false);
            yield return new WaitForSeconds(0.12f); 
        }
        Destroy(gameObject);
    }

    IEnumerator ShowGirl() {
        AudioSource.PlayClipAtPoint(cryingSound, transform.position);
        girl.SetActive(true);
        yield return new WaitForSeconds(2.5f); 
        girl.SetActive(false);
        Destroy(gameObject);
    }
}
