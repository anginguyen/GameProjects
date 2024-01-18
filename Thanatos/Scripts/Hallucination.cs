using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hallucination : MonoBehaviour
{
    public GameManager gameManager;
    public Sprite[] hallucinationSprites;

    Image image; 
    Animator anim;
    AudioSource stinger;

    float timer; 
    int spriteIndex;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        anim = GetComponent<Animator>();
        stinger = GetComponent<AudioSource>();
        timer = Random.Range(180f, 360f); 
        spriteIndex = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.currState == GameManager.State.Move) {
            timer -= Time.deltaTime;

            if (timer <= 0f) {
                anim.SetTrigger("Hallucination");
                stinger.Play();

                timer = Random.Range(180f, 360f); 
                spriteIndex++;
                if (spriteIndex == hallucinationSprites.Length) spriteIndex = 0; 
                image.sprite = hallucinationSprites[spriteIndex];
            }
        }
    }
}
