using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTransition : MonoBehaviour
{
    public GameManager gameManager;
    public AudioManager audioManager;
    public PlayerMovement player; 

    public GameObject clueCamera;
    public Door door;
    public GameObject stairsGirlTrigger;

    public MeshRenderer topClue;
    public MeshRenderer bottomClue;

    public AudioSource doorSound;
    public AudioSource runningSound;

    bool hasAnimated = false;

    void Update()
    {
        if (!hasAnimated && clueCamera.activeSelf) {
            hasAnimated = true; 
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition() {
        yield return new WaitForSeconds(0.1f);
        gameManager.SwitchToTransition();

        StartCoroutine(audioManager.FadeOutBackgroundMusic());
        yield return new WaitForSeconds(4f);

        doorSound.Play();
        yield return new WaitForSeconds(1.15f);
        runningSound.Play();
        yield return new WaitForSeconds(4f);

        topClue.enabled = false;
        bottomClue.enabled = true;
        yield return new WaitForSeconds(0.4f);

        door.UnlockRegular();
        door.Open();
        door.SwitchToInactive();

        stairsGirlTrigger.SetActive(true);
        player.walkSoundEnabled = true; 
        gameManager.SwitchToView();
        yield break;
    }
}
