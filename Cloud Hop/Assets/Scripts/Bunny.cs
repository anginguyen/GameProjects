using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bunny : MonoBehaviour
{
    public LayerMask layerMask;

    public int lives = 3;
    public bool canMove;

    public GameObject lockGO;
    bool hasKey;

    public AudioSource deathAudio;
    public AudioSource winAudio;

    public GameObject levelManager;
    LevelManager lmScript;

    Animator bunnyAnimator;
    
    void Start()
    {
        hasKey = false;
        canMove = true;
        deathAudio = GetComponent<AudioSource>();
        lmScript = levelManager.GetComponent<LevelManager>();
        bunnyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove) {
            // If player is not on a cloud, they die
            RaycastHit2D cloudHit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, LayerMask.GetMask("Cloud"));
            if (cloudHit.collider == null) {
                Death();
            }

            // If player is in front of a lock and they already picked up the key
            RaycastHit2D lockHit = Physics2D.Raycast(transform.position, Vector2.left, 0.75f, LayerMask.GetMask("Lock"));
            if (lockHit.collider != null && hasKey && lockGO.GetComponent<SpriteRenderer>().enabled) {
                lockGO.GetComponent<AudioSource>().Play();
                lockGO.GetComponent<SpriteRenderer>().enabled = false;
            }

            // Player movement left, right, up, down
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                Hop(new Vector3(-0.75f, 0, 0));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                Hop(new Vector3(0.75f, 0, 0));
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                Hop(new Vector3(0, 0.75f, 0));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                Hop(new Vector3(0, -0.75f, 0));
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other) {
        // Once player gets to the door, load next level
        if (other.tag == "Door") {
            lmScript.levelComplete = true;
            winAudio.Play();
        }

        // Play key pick-up sound and remove key when player picks it up
        if (other.tag == "Key") {
            other.gameObject.GetComponent<AudioSource>().Play();
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            hasKey = true;
        }
    }

    void Hop(Vector3 direction) {
        // Prevents players from moving into walls
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, direction, 0.75f, LayerMask.GetMask("Wall"));
        if (wallHit.collider != null) {
            return;
        }

        // Prevents player from passing through lock if they didn't pick up the key first
        RaycastHit2D lockHit = Physics2D.Raycast(transform.position, direction, 0.75f, LayerMask.GetMask("Lock"));
        if (lockHit.collider != null && lockGO.GetComponent<SpriteRenderer>().enabled) {
            return;
        }

        // Prevents player from completing level if not all clouds have been stepped on 
        RaycastHit2D doorHit = Physics2D.Raycast(transform.position, direction, 0.75f, LayerMask.GetMask("Door"));
        if (doorHit.collider != null && lmScript.cloudsHopped != lmScript.totalClouds-1) {
            return;
        }

        // Moves player
        transform.position += direction;
    }

    // Triggers death animation, plays death audio, and stops player movement
    void Death() {
        bunnyAnimator.SetTrigger("Death");
        deathAudio.Play();
        canMove = false;
        lmScript.playerDead = true;
        Invoke("BunnyDisappear", 1.5f);
    }

    // Destroys bunny GO
    void BunnyDisappear() {
        Destroy(gameObject);
    }
}
