using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    int randomNumber;
    GameObject score;

    void Start()
    {
        randomNumber = Random.Range(1, 6);
        score = GameObject.Find("Hi-Score UI");
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (randomNumber == 1)
        {
            Invoke("Spawn", 1.5f);
            transform.position = new Vector2(-6f, 4.09f);
            randomNumber = 0;
            Invoke("Shuffle", 3.5f);
        }

        if (randomNumber == 2)
        {
            Invoke("Spawn", 1.5f);
            transform.position = new Vector2(-3.02f, 4.09f);
            randomNumber = 0;
            Invoke("Shuffle", 3.5f);
        }

        if (randomNumber == 3)
        {
            Invoke("Spawn", 1.5f);
            transform.position = new Vector2(0f, 4.09f);
            randomNumber = 0;
            Invoke("Shuffle", 3.5f);
        }

        if (randomNumber == 4)
        {
            Invoke("Spawn", 1.5f);
            transform.position = new Vector2(3.02f, 4.09f);
            randomNumber = 0;
            Invoke("Shuffle", 3.5f);
        }

        if (randomNumber == 5)
        {
            Invoke("Spawn", 1.5f);
            transform.position = new Vector2(6f, 4.09f);
            randomNumber = 0;
            Invoke("Shuffle", 3.5f);
        }
    }

    void Spawn()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }
    void Shuffle()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        Invoke("RandomNumberWait", 2f);
    }
    void RandomNumberWait()
    {
        randomNumber = Random.Range(1, 6);
    }
}
