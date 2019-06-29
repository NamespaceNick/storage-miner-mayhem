using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Vector3 offset = new Vector3(0, 0, -10f);
    public float minCameraSize = 5f;
    public float playerDistanceOffset = 5f;
    public float playerDistanceFactor = 0.3f;
    public GameObject cameraPrefab;

    GameObject player1, player2;
    Camera cam1;
    Camera cam2;
    bool split = false;

    Rect full = new Rect(0, 0, 1f, 1f);
    Rect lefthalf = new Rect(0, 0, 0.50f, 1f);
    Rect righthalf = new Rect(0.50f, 0, 0.50f, 1f);

    void Start() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        player1 = players[0];
        player2 = players[1];
        cam1 = GetComponent<Camera>();

        cam2 = Instantiate(cameraPrefab, transform.position, Quaternion.identity).GetComponent<Camera>();
        cam2.rect = righthalf;
        cam2.enabled = false;
    }

    void Update() {
        // If the players are far apart, split the screen in half and follow both.
        // MILESTONE 2: for now, always split the screen because our camera transition is too jarring
        if (PlayerDistance() >= 0f) {
            if (!split) { Multiplex(); }

            // Put the players on the left or right of the screen based on who is further ahead.
            if (player1.transform.position.x > player2.transform.position.x) {
                transform.position = player2.transform.position + offset;
                cam2.gameObject.transform.position = player1.transform.position + offset;
            } else {
                transform.position = player1.transform.position + offset;
                cam2.gameObject.transform.position = player2.transform.position + offset;
            }
        } else {
            if (split) { Singularize(); }

            // set the center of the camera to the middle of the two players + some z-axis offset
            transform.position = (player1.transform.position + player2.transform.position) / 2 + offset;

            // set the camera's zoom to the max of:
            // 1. a minimum camera zoom
            // 2. a dynamic zoom based on the distance between the two players
            cam1.orthographicSize = Mathf.Max(minCameraSize,
                                              playerDistanceFactor * (player1.transform.position - player2.transform.position).magnitude
                                              + playerDistanceOffset);
        }
    }

    void Multiplex() {
        split = true;
        cam1.rect = lefthalf;
        cam1.orthographicSize = minCameraSize;

        cam2.enabled = true;
        cam2.orthographicSize = minCameraSize;
    }

    void Singularize() {
        split = false;
        cam1.rect = full;
        cam2.enabled = false;
    }

    float PlayerDistance() {
        return Vector3.Distance(player1.transform.position,
                                player2.transform.position);
    }
}
