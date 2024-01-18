using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameManager gameManager;
    public Door door; 
    public bool isSliding; 

    public AudioClip pickupSound;

    public Material indicator;
    Material originalMaterial;

    MeshRenderer mesh;

    void Start() {
        originalMaterial = GetComponent<Renderer>().material;
        mesh = GetComponent<MeshRenderer>();
    }

    public void OnMouseDown() {
        mesh.enabled = false;
        AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.35f);
        if (isSliding) door.UnlockSliding();
        else if (door) door.UnlockRegular();
        else if (gameManager) gameManager.hasBlacklight = true;
        Invoke("DeactivateObject", 1f);
    }

    void DeactivateObject() {
        gameObject.SetActive(false);
    }

    // Turn on indicator when player is looking at object 
    public void EnableIndicator() {
        GetComponent<Renderer>().material = indicator;
    }

    public void DisableIndicator() {
        GetComponent<Renderer>().material = originalMaterial;
    }
}
