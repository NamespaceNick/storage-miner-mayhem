using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitDynamite : MonoBehaviour {

    public float flashTime = 0.1f;
    public float fuseTime = 1f;
    public GameObject explosionPrefab;
    public Sprite defaultSprite, flashSprite;

    bool started = false;
    SpriteRenderer sr;
    AudioSource source;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(!started && (collider.gameObject.CompareTag("Hook") || collider.gameObject.CompareTag("Pick"))) {
            started = true;
            StartCoroutine(Flash());
            StartCoroutine(Fuse());
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(!started && (collision.gameObject.CompareTag("Hook") || collision.gameObject.CompareTag("Pick"))) {
            started = true;
            StartCoroutine(Flash());
            StartCoroutine(Fuse());
        }
    }

    IEnumerator Flash() {
        while(true) {
            yield return new WaitForSeconds(flashTime);
            sr.sprite = defaultSprite;
            yield return new WaitForSeconds(flashTime);
            sr.sprite = flashSprite;
        }
    }

    IEnumerator Fuse() {
        source.Play();
        yield return new WaitForSeconds(fuseTime);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
