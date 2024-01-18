using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType {
        Inactive,
        Sliding,
        Regular,
        Locked
    }

    public DoorType type; 

    public HFPS.Systems.NumberPadlock padlock;
    public HFPS.Systems.Keypad keypad;

    public Animator keyhole;
    public AudioClip keyInsertSound;
    public AudioClip keyTurnSound;
    public Door correspondingDoor;
    bool isUnlocked;

    public float openBy;
    public bool isOpen;

    public float duration = 0.5f;

    AudioSource doorSound;
    bool isOpening;

    public GameManager gameManager;

    void Start() {
        doorSound = GetComponent<AudioSource>();
        isOpening = false;
        isUnlocked = false;
    }

    public void Open() {
        if (type == DoorType.Regular) {
            if (!isOpening) {
                StartCoroutine(Rotate());
            }
        }
        else if (type == DoorType.Sliding) {
            if (!isOpening) {
                StartCoroutine(Slide());
            }
        }
        else if (type == DoorType.Locked) {
            if (padlock) {
                padlock.TogglePadlock();
            }
            else if (keypad) {
                keypad.ToggleKeypad();
            }
            else {
                gameManager.EnableInstructionText("You need a key to unlock this door.");
            }
        }
    }

    public void UnlockSliding() {
        type = DoorType.Sliding;
    }

    public void UnlockRegular() {
        type = DoorType.Regular;
    }

    public void SwitchToInactive() {
        type = DoorType.Inactive;
    }

    IEnumerator Slide() {
        if (keyhole && !isUnlocked) {
            gameManager.SwitchToTransition();
            keyhole.enabled = true; 
            yield return new WaitForSeconds(0.7f); 
            AudioSource.PlayClipAtPoint(keyInsertSound, keyhole.transform.position); 
            yield return new WaitForSeconds(0.6f); 
            AudioSource.PlayClipAtPoint(keyTurnSound, keyhole.transform.position, 0.5f); 
            yield return new WaitForSeconds(0.65f); 

            keyhole.enabled = false;
            correspondingDoor.type = DoorType.Sliding;
            gameManager.SwitchToMove();
            isUnlocked = true;
        }

        isOpening = true; 

        Vector3 startPos = transform.position;
        float diffPosX = openBy - startPos.x;
        doorSound.Play();

        float timer = 0, percent;
        while (timer <= duration) {
            timer += Time.deltaTime;
            percent = timer/duration;
            transform.position = startPos + new Vector3(openBy*percent, 0f, 0f);
            yield return null;
        }

        isOpen = !isOpen;
        openBy *= -1; 
        isOpening = false;
    }

    IEnumerator Rotate() {
        if (keyhole && !isUnlocked) {
            gameManager.SwitchToTransition();
            keyhole.enabled = true; 
            yield return new WaitForSeconds(0.7f); 
            AudioSource.PlayClipAtPoint(keyInsertSound, keyhole.transform.position); 
            yield return new WaitForSeconds(0.6f); 
            AudioSource.PlayClipAtPoint(keyTurnSound, keyhole.transform.position, 0.5f); 
            yield return new WaitForSeconds(0.65f); 

            keyhole.enabled = false;
            gameManager.SwitchToMove();
            isUnlocked = true;
        }

        isOpening = true;

        Quaternion startRot = transform.rotation;
        float startRotY = startRot.eulerAngles.y; 
        Quaternion rotateTo = isOpen ? Quaternion.Euler(0f, startRotY-openBy, 0f) : Quaternion.Euler(0f, openBy+startRotY, 0f);
        doorSound.Play();

        float timer = 0, percent;
        while (timer <= duration) {
            timer += Time.deltaTime;
            percent = timer/duration;
            transform.rotation = Quaternion.Lerp(startRot, rotateTo, percent);
            yield return null;
        }
        
        isOpen = !isOpen;
        isOpening = false;
    }
}
