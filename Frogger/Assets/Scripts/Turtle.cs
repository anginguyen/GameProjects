using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite turtleSwim1, turtleSwim2, turtleSwim3, turtleDive1, turtleDive2;
    [Range(1, 2)] //Enables a nifty slider in the editor
    public int turtleType = 1; // If turtleType = 1, only swimming. If = 2, only diving.

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
       
        if(turtleType == 1)
        {
            StartCoroutine("TurtleAnimationSwimming");
        }

        else if(turtleType == 2)
        {
            StartCoroutine("TurtleAnimationDiving");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TurtleAnimationSwimming()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleSwim1;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleSwim2;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleSwim3;
        }
    }

    IEnumerator TurtleAnimationDiving()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleSwim1;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleSwim2;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleSwim3;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleDive1;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleDive2;
            yield return new WaitForSeconds(0.25f);
            gameObject.GetComponent<Renderer>().enabled = false;
            // Remove layermask for instance.
            yield return new WaitForSeconds(0.35f);
            gameObject.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleDive2;
            yield return new WaitForSeconds(0.25f);
            sr.sprite = turtleDive1;
        }
    }
}
