using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameManager gameManager;

    public float moveSpeed;
    public Transform orientation;

    public PlayerCamera playerCamera;

    float horizontalInput;
    float verticalInput;

    public GameObject pauseMenuUI;

    public bool walkSoundEnabled = false; 

    Vector3 moveDirection;
    Rigidbody rb; 
    AudioSource walkSound;

    void Start()
    {
        transform.rotation = Quaternion.EulerAngles(0f, -135f, 0f);
        rb = GetComponent<Rigidbody>();
        walkSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameManager.currState == GameManager.State.Move) {
            MyInput();  
        }
        // Switch flashlight
        if (Input.GetKeyDown(KeyCode.F)) {
            gameManager.SwitchFlashlight();
            gameManager.DisableInstructionText();
        }
    }

    void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // Interact with objects
        if (Input.GetKeyDown(KeyCode.E)) {
            playerCamera.InteractWithObject();
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        if (walkSoundEnabled) {
            if (moveDirection == Vector3.zero) {
                walkSound.Pause();
            }
            else if (!walkSound.isPlaying) {
                walkSound.Play(); 
        }
        }
    }

    public void ResetInputs() {
        verticalInput = 0;
        horizontalInput = 0;
    }
}
