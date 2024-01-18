using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Camera clueCamera;

    public Vector3 boundsMin;
    public Vector3 boundsMax; 

    public bool onBlackboard;
    public bool isDragging = false;

    AudioSource dragSound;

    void Start() {
        dragSound = GetComponent<AudioSource>();
    }

    void Update() {
        if (isDragging) {
            if (!onBlackboard) {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 0.36f; 
                mousePos = clueCamera.ScreenToWorldPoint(mousePos);
                SnapToMouse(mousePos);
            }
            else {
                Ray ray = clueCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit; 

                if (Physics.Raycast(ray, out hit, 1f, LayerMask.GetMask("Draggable"))) {
                    SnapToMouse(hit.point);
                }
                else {
                    SnapToMouse(ray.GetPoint(0.5645f));
                }
            }
        }
    }

    void OnMouseDown() {
        Ray ray = clueCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f, LayerMask.GetMask("Draggable"))) {
            if (dragSound) dragSound.Play();
            
            Vector3 newPos = hit.point;
            if (onBlackboard) newPos.x = transform.position.x;
            else newPos.y = transform.position.y;
            transform.position = newPos;
            isDragging = true;
        }
    }

    void OnMouseUp() {
        if (isDragging) {
            isDragging = false;
            if (dragSound) dragSound.Play();
        }
    }

    void SnapToMouse(Vector3 hit) {
        Vector3 newPos = hit;

        // Hangman pieces on blackboard
        if (onBlackboard) {
            newPos.x = transform.position.x;
            newPos.y = Mathf.Clamp(newPos.y, boundsMin.y, boundsMax.y);
            newPos.z = Mathf.Clamp(newPos.z, boundsMin.z, boundsMax.z);
        }
        // Paper puzzle on desk
        else {
            newPos.x = Mathf.Clamp(newPos.x, boundsMin.x, boundsMax.x);
            newPos.y = transform.position.y;
            newPos.z = Mathf.Clamp(newPos.z, boundsMin.z, boundsMax.z);
        }

        transform.position = newPos;
    }

    public void SetPosition(Vector3 position) {
        transform.localPosition = position;
    }
}
