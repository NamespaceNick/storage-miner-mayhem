using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampSpeed : MonoBehaviour {

    public float dampFactor = 0.95f;

    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        rb.velocity *= dampFactor;
    }
}
