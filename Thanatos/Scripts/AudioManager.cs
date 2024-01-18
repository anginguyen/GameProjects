using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] layers;
    public AudioSource audioSource;
    public AudioClip[] stingers;

    float timer;
    bool timerPaused = false;
    int index = 0;

    void Start() {
        timer = Random.Range(180f, 300f);
    }

    void Update() {
        if (!timerPaused) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                PlayStinger(stingers[index]);
                index++;
                if (index == stingers.Length) {
                    index = 0;
                }
            }
        }
        else if (!audioSource.isPlaying) {
            timerPaused = false; 
        }
    }

    public IEnumerator AddMusicLayer(int level) {
        float timer = 0;
        while (timer < 1f) {
            timer += Time.deltaTime;
            layers[level].volume = Mathf.Lerp(0f, 0.25f, timer);
            yield return null;
        }
        yield break; 
    }

    public IEnumerator FadeOutBackgroundMusic() {
        float timer = 0;
        while (timer < 3.5f) {
            timer += Time.deltaTime;
            float percent = timer / 3.5f;
            foreach (AudioSource layer in layers) {
                layer.volume = Mathf.Lerp(0.25f, 0f, percent);
            }
            yield return null;
        }
        yield break; 
    }

    public void PlayStinger(AudioClip stinger) {
        timerPaused = true;
        audioSource.PlayOneShot(stinger, 1f);
        timer = Random.Range(180f, 300f);
    }
}
