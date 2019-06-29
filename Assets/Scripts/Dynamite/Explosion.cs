using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float explosionRadius = 5f;
    public float explosionForce = 20f;
    public float timeToLive = 10f;

	void Start() {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();

        // find all colliders in range
        foreach(Collider2D col in cols) {
            // add rigidbody to list
            if(col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody)) {
                rigidbodies.Add(col.attachedRigidbody);
            }

            // destroy destructable colliders
            if(col.CompareTag("Breakable")) {
                Destroy(col.gameObject);
            }
        }

        // add explosive force to every rigidbody in range
        foreach(Rigidbody2D rb in rigidbodies) {
            rb.AddForce(explosionForce * (rb.transform.position-transform.position).normalized, ForceMode2D.Impulse);
        }

        StartCoroutine(DestroySelf());
	}

    // destroy explosion after timeToLive
    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
