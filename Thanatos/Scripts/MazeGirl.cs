using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGirl : MonoBehaviour
{
    public Animator girlAnimator; 
    public PlayerCamera camera;
    public AudioSource[] sounds;

    bool triggered = false;

    void OnTriggerStay(Collider col) {
        if (!triggered && camera.IsLookingAt("Girl")) {
            triggered = true;
            StartCoroutine(AnimateGirl());
        }
    }

    IEnumerator AnimateGirl() {
        girlAnimator.gameObject.GetComponent<BoxCollider>().enabled = false;
        girlAnimator.enabled = true;
        foreach (AudioSource sound in sounds) sound.Play();
        StartCoroutine(FadeOutSounds());
        yield return new WaitForSeconds(1.85f); 

        girlAnimator.gameObject.transform.rotation = Quaternion.EulerAngles(0, -180, 0);
        yield return new WaitForSeconds(0.5f); 

        Destroy(girlAnimator.gameObject);
        Destroy(gameObject);
    }

    IEnumerator FadeOutSounds() {
        float timer = 0;
        while (timer < 2.35f) {
            timer += Time.deltaTime;
            float percent = timer / 2.35f;
            foreach (AudioSource sound in sounds) {
                sound.volume = Mathf.Lerp(0.5f, 0f, percent);
            }
            yield return null;
        }
        yield break; 
    }
}
