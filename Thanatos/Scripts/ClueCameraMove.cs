using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueCameraMove : MonoBehaviour
{
    public GameManager gameManager; 

    public Vector3 moveTo;
    public Quaternion rotateTo;
    public float newFOV;

    Vector3 startPos, differencePos;
    Quaternion startRot;
    float startFOV, differenceFOV;

    Camera camera; 

    void Start() {
        startPos = transform.position;
        differencePos = moveTo - startPos;
        startRot = transform.rotation;
        camera = GetComponent<Camera>();
        startFOV = camera.fieldOfView;
        differenceFOV = newFOV - startFOV;
    }

    public void MoveCamera() {
        StartCoroutine(PanToTopDownView());
    }

    public IEnumerator PanToTopDownView() {
        yield return new WaitForSeconds(0.25f);

        float timer = 0;
        float percent;
        while (timer < 2f) {
            timer += Time.deltaTime;
            percent = timer / 2f;
            transform.position = startPos + differencePos*percent;
            transform.rotation = Quaternion.Lerp(startRot, rotateTo, percent);
            camera.fieldOfView = startFOV + differenceFOV*percent;
            yield return null;
        }

        gameManager.SwitchToView();
    }
}