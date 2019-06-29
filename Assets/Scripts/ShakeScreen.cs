using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreen : MonoBehaviour {

    // how much screen shake and how fast
    public float speedMultiplier = 16f;
    public float translationMultiplier = 2f;
    public float rotationMultiplier = 4f;

    // variable that decreases over time between 0 and 1
    float trauma = 0f;
    // controls random function
    float xSeed, ySeed, rSeed;

    // initialize random seeds
    void Start() {
        xSeed = Random.value;
        ySeed = xSeed + 1;
        rSeed = ySeed + 1;
    }

    void Update() {
        // set translation and rotation proportional to trauma squared using Perlin Noise
        transform.localPosition = translationMultiplier * trauma * trauma * new Vector3(2*Mathf.PerlinNoise(xSeed, speedMultiplier * Time.time) - 1,
                                                                                        2*Mathf.PerlinNoise(ySeed, speedMultiplier * Time.time) - 1,
                                                                                        0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, rotationMultiplier * trauma * trauma * (2 * Mathf.PerlinNoise(rSeed, speedMultiplier * Time.time) - 1));
        // shrink trauma
        trauma -= Time.deltaTime;
        if(trauma < 0) {
            trauma = 0;
        }
    }

    // add trauma
	public void ApplyTrauma(float newTrauma) {
        trauma += newTrauma;
        if(trauma > 1) {
            trauma = 1;
        };
	}
}
