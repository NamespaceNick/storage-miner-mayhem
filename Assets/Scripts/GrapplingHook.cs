using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GrapplingHook : MonoBehaviour {
    public GameObject grapplingHookPrefab, indicatorPrefab, reticlePrefab, ropePrefab;
    public bool allInstantiated = false;
    public float grappleForce = 50f;
    public float adjustRopeSpeed = 10f;
    public float zipRopeSpeed = 30f;
    public float indicatorLength = 2f;
    public float outlineOffset = 0.03f;
    public float lerpSpeed = 0.5f;
    public float missAlpha = 0.2f;

    GameObject grapplingHook1;
    GameObject grapplingHook2;
    Grapple grapple1;
    Grapple grapple2;
    public GameObject lIndicator;
    public GameObject rIndicator;
    public GameObject lReticle;
    public GameObject rReticle;
    GameObject rope1;
    GameObject rope2;
    PlayerController controller;
    Rigidbody2D rb;
    bool[] rappelling = new bool[] {false, false};
    bool[] zipActive = new bool[] {false, false};

    void Start() {
        allInstantiated = false;
        // instantiate hooks and player controller
        grapplingHook1 = Instantiate(grapplingHookPrefab);
        grapplingHook1.SetActive(false);
        grapple1 = grapplingHook1.GetComponent<Grapple>();
        grapple1.player = gameObject;

        grapplingHook2 = Instantiate(grapplingHookPrefab);
        grapplingHook2.SetActive(false);
        grapple2 = grapplingHook2.GetComponent<Grapple>();
        grapple2.player = gameObject;

        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        // instantiate hook indicators
        lIndicator = Instantiate(indicatorPrefab);
        rIndicator = Instantiate(indicatorPrefab);
        lIndicator.SetActive(false);
        rIndicator.SetActive(false);

        // instantiate aiming reticles
        lReticle = Instantiate(reticlePrefab);
        rReticle = Instantiate(reticlePrefab);
        lReticle.SetActive(false);
        rReticle.SetActive(false);

        // set hook outline
        Transform[] outline1 = grapplingHook1.transform.Find("Visual").gameObject.GetComponentsInChildren<Transform>();
        if(outline1.Length >= 5) {
            outline1[1].localPosition = outlineOffset * Vector3.up;
            outline1[2].localPosition = -outlineOffset * Vector3.up;
            outline1[3].localPosition = outlineOffset * Vector3.right;
            outline1[4].localPosition = -outlineOffset * Vector3.right;
        }
        Transform[] outline2 = grapplingHook2.transform.Find("Visual").gameObject.GetComponentsInChildren<Transform>();
        if(outline2.Length >= 5) {
            outline2[1].localPosition = outlineOffset * Vector3.up;
            outline2[2].localPosition = -outlineOffset * Vector3.up;
            outline2[3].localPosition = outlineOffset * Vector3.right;
            outline2[4].localPosition = -outlineOffset * Vector3.right;
        }

        // instantiate ropes
        // rope1 = Instantiate(ropePrefab);
        // ManageRope manager1 = rope1.GetComponent<ManageRope>();
        // manager1.player = gameObject;
        // manager1.hook = grapplingHook1;
        // rope2 = Instantiate(ropePrefab);
        // ManageRope manager2 = rope2.GetComponent<ManageRope>();
        // manager2.player = gameObject;
        // manager2.hook = grapplingHook2;
        allInstantiated = true;
    }

    void FixedUpdate() {
        // move player if grapples are hooked onto something
        // check player is far enough from the grapple to move based on rope distance
        if(grapplingHook1.activeSelf && grapple1.Hooked()) {
            Vector3 gr1dir = grapplingHook1.transform.position - transform.position;
            rb.AddForce(grappleForce * gr1dir.normalized * (Mathf.Max(gr1dir.magnitude - grapple1.GetRopeDistance(), 0)));

            // kill velocity in direction away from hook
            Vector2 normal = -gr1dir.normalized;
            if(gr1dir.magnitude > grapple1.GetRopeDistance()) {
                float dot = Vector3.Dot(rb.velocity, normal);
                if(dot > 0f) {
                    rb.velocity -= normal * dot;
                }
            }
        }
        if(grapplingHook2.activeSelf && grapple2.Hooked()) {
            Vector3 gr2dir = grapplingHook2.transform.position - transform.position;
            rb.AddForce(grappleForce * gr2dir.normalized * (Mathf.Max(gr2dir.magnitude - grapple2.GetRopeDistance(), 0)));

            // kill velocity in direction away from hook
            Vector2 normal = -gr2dir.normalized;
            if(gr2dir.magnitude > grapple2.GetRopeDistance()) {
                float dot = Vector3.Dot(rb.velocity, normal);
                if(dot > 0f) {
                    rb.velocity -= normal * dot;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        //Trigger(collider);
    }

    void OnTriggerStay2D(Collider2D collider) {
        //Trigger(collider);
    }

    // Trigger fires when the player collides with a hook,
    // to destroy the correct hook if it was retracting
    void Trigger(Collider2D collider) {
        if(collider.gameObject == grapplingHook1 && grapple1.Retracting()) {
            collider.gameObject.SetActive(false);
        }
        else if(collider.gameObject == grapplingHook2 && grapple2.Retracting()) {
            collider.gameObject.SetActive(false);
        }
    }

    public void SetPlayerColor(int hookNum, Color color) {
        GameObject hookVis, indicatorVis, reticleVis, hook;
        Debug.Log("SetPlayerColor(hookNum: " + hookNum + ", color: " + color);
        if(hookNum == 1) {
            if (grapplingHook1 == null) Debug.LogError("ghook1 is null");
            if (grapplingHook1.transform.Find("Visual").gameObject == null) Debug.LogError("ghook visual gameobject is null");
            hook = grapplingHook1;
            hookVis = grapplingHook1.transform.Find("Visual").gameObject;
            indicatorVis = lIndicator.transform.Find("Visual").gameObject;
            reticleVis = lReticle.transform.Find("Visual").gameObject;
            // rope = rope1;
        }
        else if(hookNum == 2) {
            if (grapplingHook2 == null) Debug.LogError("ghook2 is null");
            if (grapplingHook2.transform.Find("Visual").gameObject == null) Debug.LogError("ghook visual gameobject is null");
            hook = grapplingHook2;
            hookVis = grapplingHook2.transform.Find("Visual").gameObject;
            indicatorVis =  rIndicator.transform.Find("Visual").gameObject;
            reticleVis =  rReticle.transform.Find("Visual").gameObject;
            // rope = rope2;
        }
        else {
            return;
        }

        hook.GetComponent<LineRenderer>().material.color = color;

        foreach (Transform child in hookVis.transform) {
            child.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        foreach (Transform child in indicatorVis.transform) {
            child.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        foreach (Transform child in reticleVis.transform) {
            child.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        // rope.GetComponent<ManageRope>().SetPlayerColor(color);
    }

    // display indicators
    // left analog stick
    // aimDirection should be direction analog stick is pointing
    public void AimHook(int hookNum, Vector2 aimDirection) {
        GameObject hook, reticle, indicator;
        Grapple grapple;
        if(hookNum == 1) {
            hook = grapplingHook1;
            reticle = lReticle;
            indicator = lIndicator;
            grapple = grapple1;
        }
        else if(hookNum == 2) {
            hook = grapplingHook2;
            reticle = rReticle;
            indicator = rIndicator;
            grapple = grapple2;
        }
        else {
            return;
        }

        RaycastHit2D defHit;
        RaycastHit2D passHit;

        // if aiming
        if(aimDirection != Vector2.zero) {
            // direction hook will travel
            if (rb == null) Debug.Log("RB");
            if (grapple == null) Debug.Log("Grapple");
            Vector3 retDirection = grapple.throwSpeed * aimDirection.normalized + rb.velocity;
            SpriteRenderer indSr = indicator.GetComponentInChildren<SpriteRenderer>();
            // thank you answerhub
            // reticle against wall
            defHit = Physics2D.Raycast(transform.position, retDirection, grapple.minDistance, LayerMask.GetMask("Default"));
            passHit = Physics2D.Raycast(transform.position + retDirection.normalized * grapple.minDistance, retDirection, grapple.maxDistance-grapple.minDistance, LayerMask.GetMask("Default", "Passthrough Obstacle"));
            if(defHit.collider != null) {
                reticle.transform.position = Vector3.Lerp(reticle.transform.position, defHit.point, lerpSpeed);
                indSr.color = new Color(indSr.color.r, indSr.color.g, indSr.color.b, 1f);
            }
            else if(passHit.collider != null) {
                reticle.transform.position = Vector3.Lerp(reticle.transform.position, passHit.point, lerpSpeed);
                indSr.color = new Color(indSr.color.r, indSr.color.g, indSr.color.b, 1f);
            }
            // reticle in midair
            else {
                reticle.transform.position = Vector3.Lerp(reticle.transform.position, transform.position + retDirection.normalized * grapple.maxDistance, lerpSpeed);
                indSr.color = new Color(indSr.color.r, indSr.color.g, indSr.color.b, missAlpha);
            }
            reticle.SetActive(true);
        } else {
            reticle.transform.position = transform.position;
            reticle.SetActive(false);
        }

        // if aiming and not hooking
        if(aimDirection != Vector2.zero && !hook.activeSelf) {
            // indicator close to player
            indicator.transform.position = (Vector2)transform.position + aimDirection.normalized * indicatorLength;
            indicator.transform.up = aimDirection.normalized;
            indicator.SetActive(true);
        }
        else {
            indicator.SetActive(false);
        }
    }

    // throw hook, hookNum should be 1 or 2
    // A button down / X button down
    public void ThrowHook(int hookNum) {

        GameObject hook;
        Grapple grapple;
        Vector2 direction;
        bool shouldThrow = false;
        if(!grapplingHook1.activeSelf && hookNum == 1) {
            hook = grapplingHook1;
            grapple = grapple1;
            direction = lIndicator.transform.up;
            shouldThrow = lIndicator.activeSelf;
        }
        else if(!grapplingHook2.activeSelf && hookNum == 2) {
            hook = grapplingHook2;
            grapple = grapple2;
            direction = rIndicator.transform.up;
            shouldThrow = rIndicator.activeSelf;
        } else {
            return;
        }

        if (direction != Vector2.zero && shouldThrow) {
            hook.SetActive(true);

            // legacy parameter elastic set to false, creates rope instead of elastic rope
            grapple.Throw(direction, false);
        }
    }

    // retract hook, hookNum should be 1 or 2
    // A button release / X button release
    public void RetractHook(int hookNum) {
        Grapple grapple;
        if(grapplingHook1.activeSelf && hookNum == 1) {
            grapple = grapple1;
        }
        else if(grapplingHook2.activeSelf && hookNum == 2) {
            grapple = grapple2;
        }
        else {
            return;
        }

        grapple.Retract();
    }

    // called when the player turns a passive hook into an active hook
    void SetHookRopeDistance(int hookNum, int newRopeDistance) {
        Grapple grapple;
        if(grapplingHook1.activeSelf && hookNum == 1) {
            grapple = grapple1;
        }
        else if(grapplingHook2.activeSelf && hookNum == 2) {
            grapple = grapple2;
        }
        else {
            return;
        }

        grapple.SetRopeDistance(newRopeDistance);
    }

    // adjusts rope distance using a speed and current rope distance
    void ChangeHookRopeDistance(int hookNum, float speed) {
        Grapple grapple;
        if(grapplingHook1.activeSelf && hookNum == 1) {
            grapple = grapple1;
        }
        else if(grapplingHook2.activeSelf && hookNum == 2) {
            grapple = grapple2;
        }
        else {
            return;
        }

        grapple.SetRopeDistance(grapple.GetRopeDistance() + speed * Time.deltaTime);
    }

    // right analog stick
    public void Rappel(int hookNum, Vector2 aimDirection) {
        if(aimDirection != Vector2.zero) {
            rappelling[hookNum - 1] = true;
        }
        else {
            rappelling[hookNum - 1] = false;
        }

        ChangeHookRopeDistance(hookNum, -aimDirection.y * adjustRopeSpeed);
    }

    // zipline to hook, then retract hook. only works if hooked onto something
    IEnumerator Zip(int hookNum) {
        Grapple grapple;
        if(grapplingHook1.activeSelf && hookNum == 1) {
            grapple = grapple1;
        }
        else if(grapplingHook2.activeSelf && hookNum == 2) {
            grapple = grapple2;
        }
        else {
            yield break;
        }
        // dirty fix: hookNum is either 1 or 2, so we gotta subtract 1
        // to prevent from going out of bounds
        if(zipActive[hookNum - 1]) {
            yield break;
        }
        zipActive[hookNum - 1] = true;

        if(grapple.Hooked()) {
            while(grapple.Hooked() && grapple.GetRopeDistance() > 0) {
                ChangeHookRopeDistance(hookNum, -zipRopeSpeed);
                yield return null;
            }

            RetractHook(hookNum);
        }
        zipActive[hookNum - 1] = false;
    }

    // right trigger down / X button held
    public void ActivateZip(int hookNum) {
        //StartCoroutine(Zip(hookNum));

        Grapple grapple;
        if(grapplingHook1.activeSelf && hookNum == 1) {
            grapple = grapple1;
        }
        else if(grapplingHook2.activeSelf && hookNum == 2) {
            grapple = grapple2;
        }
        else {
            return;
        }

        if(!rappelling[hookNum - 1]) {
            ChangeHookRopeDistance(hookNum, -zipRopeSpeed);
        }
    }

    // returns reticles managed by player
    // called by ControlVoronoiCamera
    public GameObject GetReticle(int reticleNum) {
        if(reticleNum == 1) {
            return lReticle;
        }
        else if(reticleNum == 2) {
            return rReticle;
        }
        return null;
    }

    // returns grappling hooks by player
    // called by ControlVoronoiCamera
    public GameObject GetHook(int hookNum) {
        if(hookNum == 1) {
            return grapplingHook1;
        }
        else if(hookNum == 2) {
            return grapplingHook2;
        }
        return null;
    }

    public void DisableVisuals() {
        DeleteVisual(lIndicator);
        DeleteVisual(rIndicator);
        DeleteVisual(lReticle);
        DeleteVisual(rReticle);
    }

    void DeleteVisual(GameObject obj) {
        foreach (SpriteRenderer sp in obj.GetComponentsInChildren<SpriteRenderer>()) {
            sp.enabled = false;
        }
    }
}
