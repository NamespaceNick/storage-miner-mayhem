using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLantern : MonoBehaviour {

    public Sprite lit;
    public int team;
    SpriteRenderer sRenderer;

    // Use this for initialization
    void Start () {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") &&
            other.gameObject.GetComponent<Avatar>().team == team) {
            sRenderer.sprite = lit;
        }
    }
}
