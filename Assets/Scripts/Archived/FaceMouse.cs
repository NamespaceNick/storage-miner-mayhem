using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class FaceMouse : MonoBehaviour {

    public Vector3 offset = new Vector3(0f, 0f, 10f);

    Camera gameCamera;
    Vector3 direction;

    void Start() {
        gameCamera = Camera.main;
    }

    public Vector3 GetDirection() {
        Vector3 mousePosition = Input.mousePosition + offset;
        mousePosition = gameCamera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = transform.position.z;

        return mousePosition - transform.position;
    }
}
