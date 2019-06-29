using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    AudioSource source;
    System.Random random = new System.Random();
    public AudioClip[] oofs;

    void Start() {
        source = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Play sound for large collisions or pickaxe or spikes
        if (collision.gameObject.CompareTag("Pick") ||
            collision.gameObject.CompareTag("Danger") ||
            collision.relativeVelocity.magnitude > 50) {
            source.clip = oofs[random.Next(0, oofs.Length)];
            source.Play();
        }
    }
}
