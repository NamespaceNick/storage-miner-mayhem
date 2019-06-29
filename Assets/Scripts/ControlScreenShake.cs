using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScreenShake : MonoBehaviour {

    public float traumaMultiplier = 0.005f;
    public List<ShakeScreen> screens;

    // apply screen shake
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.relativeVelocity.magnitude > 35) {
            foreach(ShakeScreen screen in screens) {
                screen.ApplyTrauma(traumaMultiplier * collision.relativeVelocity.magnitude);
            }
        }
    }
}
