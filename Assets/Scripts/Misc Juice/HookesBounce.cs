using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shrink or Grow using Hookes law for springiness
public class HookesBounce : MonoBehaviour {
    public bool small = true;
    public float k = 1f;
    public float dampening_factor = 0.85f;
    public float max_scale = 1.5f;
    public float min_scale = 0.75f;
    float vel = 0.0f;

    // Update is called once per frame
    void Update () {
        float desired_scale = max_scale;

        if (small)
            desired_scale = min_scale;

        float x = desired_scale - transform.localScale.x;

        float f;

        if (Time.timeScale == 0)
            f = -k * x * 0.01f;
        else
            f = -k * x * Time.deltaTime;

        vel += f;

        vel *= dampening_factor;

        transform.localScale += -Vector3.one * vel;
    }

    public void Strum() {
        transform.localScale = Vector3.one * max_scale;
    }
}
