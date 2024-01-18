using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    public GameManager gameManager;

    public Transform player;
    public Transform orientation; 
    public float sensitivity;
    public Slider slider;

    public float startRotation;
    float xRotation;
    float yRotation;

    bool viewedBlackboard; 
    string tempText;
    bool tempInstructionTextOn;
    float timer; 

    public ClueDesk[] clueDesks;
    public PickUp[] pickUpObjects;
    public Blackboard[] blackboards;
    bool indicatorOn;

    void Start() { 
        startRotation = gameManager.GetStartRotation();
        viewedBlackboard = false;
        tempInstructionTextOn = false;
        indicatorOn = false;
        sensitivity = PlayerPrefs.GetFloat("currentSensitivity", 100);
    }


    void Update()
    {
        if (gameManager.currState == GameManager.State.Move) {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;
            PlayerPrefs.SetFloat("currentSensitivity", sensitivity);


            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation+startRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation+startRotation, 0);
            player.rotation = Quaternion.Euler(0, yRotation+startRotation, 0);
        
            DetectObject();
        }
        

        // Turn off temp instructions after 3 seconds 
        if (tempInstructionTextOn) {
            gameManager.EnableInstructionText(tempText);
            timer += Time.deltaTime;
            if (timer >= 3.0f) {
                tempInstructionTextOn = false;
                gameManager.DisableInstructionText();
            }
        }

    }

     public void AdjustSpeed(float newSpeed) {
        sensitivity = newSpeed * 20;
    }



    public void InteractWithObject() {
        RaycastHit hit; 

        // Door
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f, LayerMask.GetMask("Door"))) {
            hit.collider.gameObject.GetComponent<Door>().Open();
        }
        // Clue Desk
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f, LayerMask.GetMask("Clue Desk"))) {
            hit.collider.gameObject.GetComponent<ClueDesk>().ToggleClue();
        }
        // Blackboard
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3f, LayerMask.GetMask("Blackboard"))) {
            hit.collider.gameObject.GetComponent<Blackboard>().ToggleClue();

            // Turn on blackboard instructions 
            if (!viewedBlackboard) {
                tempText = "Click and hold down to drag letters";
                tempInstructionTextOn = true;
                timer = 0;
                viewedBlackboard = true;
            }
        }
        // Padlock
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f, LayerMask.GetMask("Padlock"))) {
            hit.collider.gameObject.GetComponent<HFPS.Systems.NumberPadlock>().TogglePadlock();
        }
        // Flashlight
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f, LayerMask.GetMask("Flashlight"))) {
            hit.collider.gameObject.GetComponent<PickUp>().OnMouseDown();

            // Turn on flashlight instructions 
            tempText = "Press [F] to use blacklight";
            tempInstructionTextOn = true;
            timer = 0;
        }
    }

    void DetectObject() {
        int level = gameManager.currLevel;
        RaycastHit hit; 

        // Door
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f, LayerMask.GetMask("Door"))) {
            if (level == 0) {
                Door.DoorType doorType = hit.collider.gameObject.GetComponent<Door>().type;
                if (doorType == Door.DoorType.Locked) {
                    gameManager.EnableInstructionText("Press [E] to unlock door");
                }
                else if (doorType != Door.DoorType.Inactive) {
                    gameManager.EnableInstructionText("Press [E] to open and close door");
                }
            }
        }
        // Clue Desk
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f, LayerMask.GetMask("Clue Desk"))) {
            hit.collider.gameObject.GetComponent<ClueDesk>().EnableIndicator();
            indicatorOn = true;
            if (level == 0) gameManager.EnableInstructionText("Press [E] to view");
        }
        // Blackboard
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3f, LayerMask.GetMask("Blackboard"))) {
            hit.collider.gameObject.GetComponent<Blackboard>().EnableIndicator();
            indicatorOn = true;
            if (!viewedBlackboard) gameManager.EnableInstructionText("Press [E] to view puzzle");
        }
        // Padlock
        else if (level == 0 && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f, LayerMask.GetMask("Padlock"))) { 
            gameManager.EnableInstructionText("Press [E] to unlock door");
        }
        // Flashlight
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.5f, LayerMask.GetMask("Flashlight"))) { 
            hit.collider.gameObject.GetComponent<PickUp>().EnableIndicator();
            indicatorOn = true;
            gameManager.EnableInstructionText("Press [E] to pick up blacklight");
        }
        else {
            gameManager.DisableInstructionText();

            // Disable all indicators
            if (indicatorOn) {
                foreach (ClueDesk desk in clueDesks) {
                    desk.DisableIndicator();
                }
                foreach (PickUp obj in pickUpObjects) {
                    obj.DisableIndicator();
                }
                foreach (Blackboard blackboard in blackboards) {
                    blackboard.DisableIndicator();
                }
                indicatorOn = false;
            }
        }
    }

    public bool IsLookingAt(string item) {
        RaycastHit hit; 
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 7f)) { 
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(item)) {
                return true; 
            }
            else {
                Vector3 position = transform.position; 
                if (item == "Jumpscare") {
                    if (position.x <= 9.6f) return false;   // Not in correct room 
                    position += new Vector3(4f, 0f, 0f);
                }
                else if (item == "Girl") position += new Vector3(1f, 0f, 0f);

                if (Physics.Raycast(position, transform.TransformDirection(Vector3.forward), out hit, 7f)) { 
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer(item)) {
                        return true; 
                    }
                }
            }
        }
        return false;
    }

    public void SetObjectGlow(bool blacklightOn) {
        foreach (ClueDesk desk in clueDesks) {
            if (!desk.clueCamera.activeSelf) {
                if (blacklightOn) desk.NoGlow();
                else desk.DisableIndicator();
            }
        }
    }
}
