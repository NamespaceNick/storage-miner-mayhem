using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyTarget : MonoBehaviour
{
    bool beenHit = false;
    Text text;

    void Start() {
        text = GetComponentInChildren<Text>();
        text.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (beenHit) return;
            beenHit = true;
            text.enabled = true;
            GameController.numReady++;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (beenHit) return;
            beenHit = true;
            GameController.numReady++;
        }
    }
}
