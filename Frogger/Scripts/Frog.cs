using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private Rigidbody2D frogRB;
    private Collider2D frogCollider;
    public Vector2 defaultPosition, currentPosition, furtherPosition;
    public LayerMask layerMask;

    public AudioClip bounce, squash, plunk, munch;
    private AudioSource frogAudio;

    public SpriteRenderer sr;
    public Sprite frogJump, frogIdle, frogDeathWater1, frogDeathWater2, frogDeathWater3, frogSkull, frogDeathRoad1, frogDeathRoad2, frogDeathRoad3;

    GameObject lifeTracker, scoreTracker, musicTracker, timeTracker;
    public bool canMove;
    bool homeFrogActivated;
    
    void Start()
    {
        frogRB = gameObject.GetComponent<Rigidbody2D>();
        frogCollider = GetComponent<Collider2D>();
        frogAudio = gameObject.GetComponent<AudioSource>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        lifeTracker = GameObject.Find("Lives");
        musicTracker = GameObject.Find("MusicManager");
        scoreTracker = GameObject.Find("Hi-Score UI");
        timeTracker = GameObject.Find("Timer Bar");
        defaultPosition = transform.position;
        currentPosition.y = transform.position.y;
        furtherPosition.y = defaultPosition.y - 1;
        canMove = true;
        homeFrogActivated = false;
    }

    void Update()
    {
        RaycastHit2D homeFrogHit = Physics2D.Raycast(transform.position, Vector2.up, 1f, LayerMask.GetMask("Home Frog"));
        RaycastHit2D platformHit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, LayerMask.GetMask("Platform"));

        // Player moves with moving platforms (logs/turtles)
        if (platformHit.collider != null && canMove) {
            transform.SetParent(platformHit.collider.transform);
        }
        else {
            transform.SetParent(null);
        }

        if (canMove == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                FrogMove(Vector2.left);
                frogRB.MoveRotation(90); // Set frog rotation 90 to the left. 
                Invoke("RevertSprite", 0.125f); // Waits slightly before reverting frog back to its idle sprite.
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                FrogMove(Vector2.right);
                frogRB.MoveRotation(-90); // Set frog rotation 90 to the right. 
                Invoke("RevertSprite", 0.125f); 
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                // Doesn't let the player collide with an activated home frog 
                if (homeFrogHit.collider != null && homeFrogHit.collider.GetComponent<SpriteRenderer>().enabled) {
                    return;
                }

                FrogMove(Vector2.up);
                frogRB.MoveRotation(0); // Set frog rotation to default.
                Invoke("RevertSprite", 0.125f); 

                // Ensures Frogger cannot score after moving to the same row twice.
                currentPosition.y = transform.position.y;
                if (currentPosition.y >= furtherPosition.y)
                {
                    scoreTracker.GetComponent<ScoreManager>().score += 10; 
                    furtherPosition.y = currentPosition.y + 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                FrogMove(Vector2.down);
                frogRB.MoveRotation(180); // Set frog rotation to upside down.
                Invoke("RevertSprite", 0.125f); 
            }
        }

        // Player dies if they move off-screen
        if (transform.position.x <= -8f || transform.position.x >= 8f) {
            Death();
            frogAudio.PlayOneShot(squash);
            Respawn();
        }

        if (lifeTracker.GetComponent<Lives>().frogLives == 0)
        {
            Invoke("GameOver", 1.75f);
        }
    }

    public void Respawn()
    {
        sr.enabled = false;
        sr.sprite = frogIdle;
        transform.SetParent(null);
        transform.position = defaultPosition;
        frogRB.MoveRotation(0);
        frogCollider.enabled = true;
        frogAudio.volume = 1.0f; // Restore frog audio volume to normal (death sound is way too loud).
        Invoke("SpriteReappear", 0.04f); 

        // Only resets the timer when player makes it to the home frog
        if (canMove) {
            timeTracker.GetComponent<Timer>().ResetTime(); 
        }

        canMove = true;
        homeFrogActivated = false;
    }

    public void OnTriggerEnter2D (Collider2D col)
    {
        RaycastHit2D platformHit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, LayerMask.GetMask("Platform"));

        if (col.tag == "Home Frog") {
            col.GetComponent<FrogHome>().ActivateHomeFrog();
            homeFrogActivated = true;
        }
        else if (col.tag == "Fly")
        {
            scoreTracker.GetComponent<ScoreManager>().score += 200;
            col.GetComponent<Renderer>().enabled = false;
            frogAudio.PlayOneShot(munch);
        }
        else if (col.tag == "Water" && platformHit.collider == null && !homeFrogActivated)
        {
            WaterDeath();
        } 
        else if (col.tag == "Car Left" || col.tag == "Car Right" || (col.tag == "Home" && !homeFrogActivated))
        {
            RoadDeath();
        }
    }

    void Death() {
        lifeTracker.GetComponent<Lives>().frogLives -= 1;
        canMove = false;
        frogCollider.enabled = false;
        frogAudio.volume = 0.4f;
        musicTracker.GetComponent<BGM>().froggerDied = true;
    }

    public void WaterDeath() {
        Death();
        frogAudio.PlayOneShot(plunk);
        StartCoroutine("DeathAnimationWater");
        Invoke("Respawn", 1.75f);
    }

    public void RoadDeath()
    {
        Death();
        frogAudio.PlayOneShot(squash);
        StartCoroutine("DeathAnimationRoad");
        Invoke("Respawn", 1.75f);
    }

    void FrogMove(Vector3 direction) {
        sr.sprite = frogJump; // Change current sprite to new sprite (in this case, frog idle --> frog jump).
        frogAudio.PlayOneShot(bounce);
        transform.position = transform.position + direction;
    }

    void RevertSprite()
    {
        sr.sprite = frogIdle;
    }

    void SpriteReappear()
    {
        sr.enabled = true;
    }

    void GameOver()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeathAnimationWater()
    {
        yield return new WaitForSeconds(0.25f);
        frogRB.MoveRotation(0);
        sr.sprite = frogDeathWater1;
        yield return new WaitForSeconds(0.25f);
        sr.sprite = frogDeathWater2;
        yield return new WaitForSeconds(0.25f);
        sr.sprite = frogDeathWater3;
        yield return new WaitForSeconds(0.25f);
        sr.sprite = frogSkull;
    }

    IEnumerator DeathAnimationRoad()
    {
        yield return new WaitForSeconds(0.25f);
        frogRB.MoveRotation(0);
        sr.sprite = frogDeathRoad1;
        yield return new WaitForSeconds(0.25f);
        sr.sprite = frogDeathRoad2;
        yield return new WaitForSeconds(0.25f);
        sr.sprite = frogDeathRoad3;
        yield return new WaitForSeconds(0.25f);
        sr.sprite = frogSkull;
    }
}
