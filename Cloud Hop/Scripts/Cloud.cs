using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameObject player;
    bool stepped;

    public GameObject levelManager;
    LevelManager lmScript;

    SpriteRenderer sr; 
    public Sprite[] cloudVanish;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        lmScript = levelManager.GetComponent<LevelManager>();
        stepped = false;
    }

    void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            // Regular cloud disappears when stepped on, blue cloud disappears after the second step
            if ( (tag == "Cloud" || (tag == "Blue Cloud" && stepped)) && player.GetComponent<Bunny>().canMove ) {
                lmScript.IncrementCloudsHopped();
                StartCoroutine("VanishAnimation");
            }
            else if (!stepped) {
                stepped = true;
            }
        }
    }

    IEnumerator VanishAnimation() {
        for (int i=0; i < 5; ++i) {
            yield return new WaitForSeconds(0.05f);
            sr.sprite = cloudVanish[i]; 
        }
        Destroy(gameObject);
    }
}
