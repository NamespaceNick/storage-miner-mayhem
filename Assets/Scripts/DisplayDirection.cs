using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class DisplayDirection : MonoBehaviour {
    public float offsetDistance;
    public GameObject leftIndicatorPrefab, rightIndicatorPrefab;

    GameObject leftIndicator, rightIndicator;
    Vector3 leftDirection, rightDirection;
    GameObject player;
    // TODO: This will need to be changed once we integrate dual controllers
    InputDevice device;


    void Start()
    {
        leftIndicator = Instantiate(leftIndicatorPrefab);
        rightIndicator = Instantiate(rightIndicatorPrefab);
        player = GameObject.Find("Player");
    }

    void Update()
    {
        device = InputManager.ActiveDevice;
        leftIndicator.GetComponent<Renderer> ().enabled = (device.LeftStick.Vector != Vector2.zero);
        rightIndicator.GetComponent<Renderer> ().enabled = (device.RightStick.Vector != Vector2.zero);
        leftDirection = device.LeftStick.Vector.normalized;
        rightDirection = device.RightStick.Vector.normalized;

        leftIndicator.transform.position = player.transform.position + (leftDirection * offsetDistance);
        leftIndicator.transform.up = (player.transform.position - leftIndicator.transform.position).normalized;

        rightIndicator.transform.position = player.transform.position + (rightDirection * offsetDistance);
        rightIndicator.transform.up = (player.transform.position - rightIndicator.transform.position).normalized;
    }
}
