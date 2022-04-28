using UnityEngine;
using UnityEngine.SceneManagement;

public class Welcome : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            gameObject.GetComponent<AudioSource>().Play();
            Invoke("LoadInScene", 1.35f);
        }
    }

    void LoadInScene()
    {
        SceneManager.LoadScene("Level01");
    }
}
