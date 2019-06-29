using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour {
    public GameObject corpse;

    GameObject ghost;
    [SerializeField]
    GameObject checkpoint;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    GrapplingHook gHook;
    Collider2D collider;
    RigidbodyConstraints2D constraints;
    RigidbodyConstraints2D frozen = RigidbodyConstraints2D.FreezePositionX |
        RigidbodyConstraints2D.FreezePositionY |
        RigidbodyConstraints2D.FreezeRotation;

    Color opaque = Color.white;
    Color faded = new Color(1, 1, 1, 0.5f);
    bool respawning = false;
    Vector2 floatDirection;
    string deathTag;
    int team;
    Avatar avatar;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        gHook = GetComponent<GrapplingHook>();
        collider = GetComponent<Collider2D>();
        constraints = rb.constraints;
        sprite = transform.Find("Visual").GetComponent<SpriteRenderer>();
        ghost = transform.Find("Ghost").gameObject;
        ghost.SetActive(false);
        avatar = GetComponent<Avatar>();
        if (avatar) {
            team = avatar.team;
            deathTag = (team == 0) ? "Team1Deaths" : "Team2Deaths";
            Debug.Log("Registered death tag");
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Danger") {
            rb.constraints = frozen;
            floatDirection = collision.GetContact(0).normal;
            LogDeath();
            DoRespawn();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch (other.gameObject.tag) {
            case "Checkpoint":
                if (other.gameObject.GetComponent<LightLantern>().team == team)
                {
                    checkpoint = other.gameObject;
                }
                break;
            default:
                break;
        }
    }

    void LogDeath() {
        Debug.Log(deathTag);
        if (deathTag != null)
            Settings.Increment(deathTag);
    }

    void DoRespawn() {
        // Only allow this to be done once if touching multiple bad tiles
        if (respawning) { return; }
        respawning = true;
        if (gHook != null) {
            gHook.RetractHook(1);
            gHook.RetractHook(2);
            gHook.enabled = false;
        }

        StartCoroutine(DoTheGhostThing());
    }


    void EnableGhostMode() {
        GameObject newCorpse = Instantiate(corpse, transform.position, transform.rotation);
        // Remove the blue helment from the yellow player's corpses
        if (sprite.sprite.name == "game-objects_0") {
            Destroy(newCorpse.transform.Find("Helmet").gameObject);
        }
        ghost.SetActive(true);
        collider.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
    }

    void DisableGhostMode() {
        ghost.SetActive(false);
        collider.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = constraints;
    }

    IEnumerator DoTheGhostThing() {
        EnableGhostMode();

        // Fade in Ghost
        SpriteRenderer sprite = ghost.GetComponent<SpriteRenderer>();
        sprite.color = Color.white;

        StartCoroutine(HoverUpAndDown());

        float start = Time.time;

        // Rise up and start floating
        Vector2 dest = (floatDirection * 5) + (Vector2)transform.position;
        for (int i = 0; i < 25; i++) {
            transform.position = Vector2.Lerp(transform.position, dest, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 0.1f);
            yield return new WaitForSeconds(0.001f);
        }

        if (checkpoint != null) {
            Debug.Log("Headed to checkpoint");
            Vector3 goal = checkpoint.transform.position + new Vector3(-1, 0, 0);
            float dist = (goal - transform.position).magnitude / 50f;
            while ((transform.position - goal).magnitude > 1f) {
                transform.position = transform.position + dist * (goal - transform.position).normalized;
                yield return new WaitForSeconds(0.001f);
            }
        } else {
            yield return new WaitForSeconds(3f);
        }

        float diff = Time.time - start;

        Debug.Log("Respawn time: " + diff.ToString());

        DisableGhostMode();
        respawning = false;
        gHook.enabled = true;
    }

    IEnumerator HoverUpAndDown() {
        while (respawning) {
            Vector3 pos = transform.position;
            pos.y += (0.05f * Mathf.Sin(3 * Time.realtimeSinceStartup));
            transform.position = pos;
            yield return null;
        }
    }

    public bool IsRespawning() {
        return respawning;
    }
}
