using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;

    void Update() {
        if (IsOffScreen()) {
            Destroy(gameObject);
        }
        else {
            if (tag == "Car Left" || tag == "Turtle") {
                transform.Translate(Vector2.left * movementSpeed * Time.deltaTime); 
            }
            else if (tag == "Car Right" || tag == "Log") {
                transform.Translate(Vector2.right * movementSpeed * Time.deltaTime); 
            }
        }
    }

    bool IsOffScreen() {
        if ( ( (tag == "Car Left" || tag == "Turtle") && transform.position.x <= -10) || ( (tag == "Car Right" || tag == "Log") && transform.position.x >= 12)) {
            return true;
        }
        return false;
    }
}
