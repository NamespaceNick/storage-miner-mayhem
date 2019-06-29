using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// purely handles the thrown pick projectile
public class ThrownPick : MonoBehaviour {
    public GameObject crumble;

    Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rb.transform.up = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        CreateCrumble();
        Destroy(this.gameObject);
    }

    void CreateCrumble() {
        GameObject crumbs = Instantiate(crumble, transform.position, Quaternion.identity);
        Destroy(crumbs, 1.0f);
    }
}
