using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tosser : MonoBehaviour {
    public int angularVelocity;
    public Vector2 initVel;
    Vector3 origPos;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        origPos = transform.position;
        rb.velocity = initVel;
        rb.angularVelocity = angularVelocity;
    }

    void OnTriggerEnter2D(Collider2D other) {
        transform.position = origPos;
        rb.velocity = initVel;
        rb.angularVelocity = angularVelocity;
    }
}
