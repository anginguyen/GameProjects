using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum State { 
        Move,
        ViewObject,
        Transition
    }

    public TelemetryManager.RecordMetrics telemetry;

    public AudioManager audioManager;

    public GameObject cameraHolder;
    public GameObject player;
    public GameObject camera;
    PlayerCamera cameraScript;

    public TMP_Text instructionText;
    public Animator fadeAnimator;
    
    Rigidbody playerRB; 
    PlayerMovement playerScript;

    [Header("Level Manager")]
    public State currState; 
    public int currLevel;

    [Header("Player Transform")]
    public Vector3[] startPositions;
    public float[] startRotations;

    [Header("Flashlight Manager")]
    public Light flashlight;
    public Light[] clueLights;
    public bool hasBlacklight;
    public bool blacklightOn;

    Color originalFlashlight;
    Color originalClueLight = new Color(1f, 1f, 1f); 
    Color blacklight = new Color(0.2039521f, 0.1420879f, 0.9716981f);

    [Header("Chalkboard Manager")]
    public GameObject[] chalkboardDrawings;
    public AudioSource jumpscareSound;
    bool jumpscareSoundPlayed;

    void Awake()
    {
        currLevel = 0;
        currState = State.Move;
        hasBlacklight = false;
        blacklightOn = false;
        jumpscareSoundPlayed = false;
        playerRB = player.GetComponent<Rigidbody>();
        playerScript = player.GetComponent<PlayerMovement>();
        cameraScript = camera.GetComponent<PlayerCamera>();
        originalFlashlight = flashlight.color;
        LockPlayerCursor();
        // telemetry.StartRecordingMetric(currLevel);
    }

    void Update() {
        if (currState == State.Move) {
            // Jumpscare on level 2
            if (!jumpscareSoundPlayed && blacklightOn) {
                if (cameraScript.IsLookingAt("Jumpscare")) {
                    jumpscareSound.Play();
                    jumpscareSoundPlayed = true;
                }
            }
        }
    }

    // Return start positions and rotations for current level
    public Vector3 GetStartPosition() {
        return startPositions[currLevel];
    }

    public float GetStartRotation() {
        return startRotations[currLevel];
    }

    // Load next level 
    public void LoadNextLevel() {
        // telemetry.StopRecordingMetric();
        StartCoroutine(Transition());
    }

    IEnumerator Transition() {
        // Reset player state
        currState = State.Transition;
        playerScript.ResetInputs();
        cameraScript.enabled = false;
        DisableInstructionText();

        // Fade out
        fadeAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);

        // Still in game
        if (currLevel < 2){
            // Transport player 
            currLevel++;
            player.transform.position = startPositions[currLevel];
            cameraHolder.transform.position = startPositions[currLevel];
            player.transform.rotation = Quaternion.EulerAngles(0f, startRotations[currLevel], 0f);
            camera.transform.rotation = Quaternion.EulerAngles(0f, startRotations[currLevel], 0f);
            cameraScript.startRotation = startRotations[currLevel];
            cameraScript.enabled = true;
            currState = State.Move;

            // Fade in
            yield return new WaitForSeconds(0.5f);
            currState = State.Transition;
            StartCoroutine(audioManager.AddMusicLayer(currLevel));
            fadeAnimator.SetBool("FadeOut", false);
            yield return new WaitForSeconds(1f);

            currState = State.Move;
            // telemetry.StartRecordingMetric(currLevel);
            yield break; 
        }
        // Go to ending scene
        else {
            // telemetry.EndGameMetrics();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            yield break;
        }
    }

    // Switch states
    public void SwitchToMove() {
        currState = State.Move;
    }

    public void SwitchToTransition() {
        currState = State.Transition;
        playerScript.ResetInputs();
    }

    public void SwitchToView() {
        currState = State.ViewObject;
    }

    // Lock/unlock player
    public void LockPlayerCursor() {
        playerRB.constraints = RigidbodyConstraints.FreezeRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockPlayerCursor() {
        playerRB.constraints = RigidbodyConstraints.FreezeAll;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Show instruction text
    public void EnableInstructionText(string text) {
        instructionText.text = text;
        instructionText.enabled = true;
    }

    public void DisableInstructionText() {
        instructionText.enabled = false;
    }

    // Toggle viewing clue objects
    public void MoveToView() {
        currState = State.ViewObject;
        UnlockPlayerCursor();
        playerScript.ResetInputs();
        flashlight.intensity = 0;
    }

    public void ViewToMove() {
        currState = State.Move;
        LockPlayerCursor();
        flashlight.intensity = 1;
    }

    // Switch between regular flashlight and blacklight
    public void SwitchFlashlight() {
        if (hasBlacklight) {
            blacklightOn = !blacklightOn; 
            foreach (Light cl in clueLights) {
                ToggleLight(cl, true);
            }
            ToggleLight(flashlight, false);
        }
    }

    void ToggleLight(Light light, bool isClueLight) {
        // Clue light
        if (isClueLight) {
            if (light.color == originalClueLight) light.color = blacklight;
            else light.color = originalClueLight;
        }
        // Player flashlight
        else {
            if (blacklightOn) {
                light.color = blacklight;
                light.range = 3;
                SetChalkboardDrawings(true);
                cameraScript.SetObjectGlow(true);
            }
            else {
                light.color = originalFlashlight;
                light.range = 8;
                SetChalkboardDrawings(false);
                cameraScript.SetObjectGlow(false);
            }
        }
    }

    // Show/hide chalkboard drawings
    void SetChalkboardDrawings(bool isActive)
    {
        foreach (var drawing in chalkboardDrawings)
        {
            drawing.SetActive(isActive);
        }
    }
}
