using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour {
    ParticleSystem particles;
    AudioSource audio;
    public AudioClip coins;

    // Use this for initialization
    void Start () {
        particles = GetComponent<ParticleSystem>();
        particles.Stop();
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            particles.Play();
            audio.Play();
            Avatar avatar = collision.gameObject.GetComponent<Avatar>();
            if (avatar) {
                RaceController.PlayerHasWon(avatar.team);
            }
        }
    }
}
