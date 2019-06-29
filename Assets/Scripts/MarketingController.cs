using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketingController : MonoBehaviour {

    bool movingCamera = false;

    // Use this for initialization
    void Start () {
        StartCoroutine(DoMarketing());
    }

    IEnumerator DoMarketing() {
        // Prepare Dangler
        GrapplingHook ghook = GameObject.Find("Dangle").GetComponent<GrapplingHook>();
        ghook.DisableVisuals();
        ghook.AimHook(1, Vector2.up);
        ghook.ThrowHook(1);
        ActivatePlayerGroup(GameObject.Find("StunPlayers"));

        ActivatePlayerGroup(GameObject.Find("StartingPlayers"));
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(AdvanceCamera());

        ActivatePlayerGroup(GameObject.Find("GoldPlayers"));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(AdvanceCamera());


        ActivatePlayerGroup(GameObject.Find("SpikePlayers"));
        yield return new WaitForSeconds(3.5f);


        ActivatePlayerGroup(GameObject.Find("ClimbingPlayer"));
        Climber climber = GameObject.Find("ClimbingPlayer/Player2D").GetComponent<Climber>();
        climber.StartClimb();
        yield return StartCoroutine(AdvanceCamera());
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(AdvanceCamera());


        yield return new WaitForSeconds(0.5f);
        ghook = GameObject.Find("StunPlayers/Player2D").GetComponent<GrapplingHook>();
        ghook.DisableVisuals();
        ghook.AimHook(1, new Vector2(1f, 0.25f));
        ghook.ThrowHook(1);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(AdvanceCamera());
    }

    IEnumerator AdvanceCamera() {
        Vector3 dest = Camera.main.transform.position + new Vector3(24, 0, 0);

        while ((Camera.main.transform.position - dest).magnitude > 0.1f) {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, dest, 0.1f);
            yield return null;
        }

        Camera.main.transform.position = dest;
    }

    void ActivatePlayerGroup(GameObject group) {
        foreach (Rigidbody2D rb in group.GetComponentsInChildren<Rigidbody2D>()) {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
