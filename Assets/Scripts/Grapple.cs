using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour {

    public float throwSpeed = 50f;
    public float retractSpeed = 20f;
    public float maxDistance = 20f;
    public float minDistance = 5f;
    public GameObject player;
    public GameObject crumble;

    bool retracting = false;
    bool hooked = false;
    bool elastic = false;
    float ropeDistance = 0f;
    GameObject col;
    GameObject hookedObj;
    Rigidbody2D rb;
    Rigidbody2D playerRb;
    RigidbodyConstraints2D originalConstraints;

    LineRenderer line;
    AudioSource audioSource;

    void Awake() {
        col = transform.Find("WallCollider").gameObject;

        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        originalConstraints = rb.constraints;
    }

    void Update() {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, player.transform.position);

        // fixes being hooked to destroyed blocks
        if(hooked && (hookedObj == null || !hookedObj.activeSelf)) {
            Retract();
        }

        if(retracting) {
            // move to player if retracting
            // fixes retracting hooks responding to gravity
            // hook moves toward player at velocity affected by the speed of the player
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.velocity = (Vector3.Dot(player.GetComponent<Rigidbody2D>().velocity, dir) + retractSpeed) * dir;
            transform.up = -rb.velocity;

            if (Vector2.Distance(player.transform.position, transform.position) < 2 * rb.velocity.magnitude * Time.deltaTime) {
                retracting = false;
                gameObject.SetActive(false);
            }
        }

        // fixes hooks off screen
        if((transform.position - player.transform.position).magnitude > maxDistance && !hooked) {
            //Debug.Log("MAX RANGE");
            Retract();
        }
    }

    // mark grapple as hooked and freeze position
    void OnCollisionEnter2D(Collision2D collision) {
        if(!retracting) {
            // only happens when traveling
            if(!hooked && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Dynamite"))) {
                // stuns players and ignites dynamite
                Retract();
            }
            // make sure we're not hooking to a passthrough obstacle too close
            else {
                // particle effect
                CreateCrumble();
                audioSource.Play();

                // normal hookable terrain
                hooked = true;
                hookedObj = collision.gameObject;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                                 RigidbodyConstraints2D.FreezePositionY |
                                 originalConstraints;
                if(elastic) {
                    ropeDistance = 0;
                }
                else {
                    ropeDistance = (player.transform.position - transform.position).magnitude;
                }
            }
        }
    }

    void CreateCrumble() {
        GameObject crumbs = Instantiate(crumble, transform.position, Quaternion.identity);
        Destroy(crumbs, 1.0f);
    }

    // mark grapple as being thrown and set velocity
    // direction should be the direction the grapple should be thrown
    // isElastic determines grapple behavior once hooked
    public void Throw(Vector2 direction, bool isElastic) {
        Physics2D.IgnoreCollision(col.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, minDistance);
        foreach(Collider2D passCol in cols) {
            if(passCol.gameObject.CompareTag("Passthrough")) {
                Physics2D.IgnoreCollision(col.GetComponent<Collider2D>(), passCol, true);
            }
        }

        elastic = isElastic;
        transform.position = player.transform.position;
        rb.velocity = player.GetComponent<Rigidbody2D>().velocity + throwSpeed * direction;
        if(rb.velocity.magnitude < throwSpeed) {
            rb.velocity = rb.velocity.normalized * throwSpeed;
        }
        transform.up = rb.velocity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation |
                         originalConstraints;
        retracting = false;
        col.SetActive(true);
    }

    // mark grapple as retracting and unfreeze
    public void Retract() {
        if(!retracting) {
            rb.velocity = Vector3.zero;
            retracting = true;
            col.SetActive(false);
            hooked = false;
            rb.constraints = originalConstraints;
        }
    }

    // returns true if grapple is attached to something
    public bool Hooked() {
        return hooked;
    }

    // returns true if grapple is retracting
    public bool Retracting() {
        return retracting;
    }

    // return rope distance
    public float GetRopeDistance() {
        return ropeDistance;
    }

    // set rope distance (with internal constraints in place)
    public void SetRopeDistance(float newRopeDistance) {
        if(hooked) {
            ropeDistance = newRopeDistance;
            if(ropeDistance < 0)
            {
                ropeDistance = 0;
            }
            if(ropeDistance > maxDistance) {
                ropeDistance = maxDistance;
            }
        }
    }

    public void SetDirection(Vector2 dir) {
        if (hooked) { return; }
        transform.up = dir;
    }
}
