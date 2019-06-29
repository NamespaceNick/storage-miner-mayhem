using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climber : MonoBehaviour {
    GrapplingHook ghook;

    bool zip1, zip2 = false;
    bool climbing = true;

    // Use this for initialization
    void Start () {
        ghook = GetComponent<GrapplingHook>();
        ghook.DisableVisuals();
    }

    public void StartClimb() {
        Debug.Log("Starting Climb");
        StartCoroutine(Climb());
    }

    IEnumerator Climb() {
        Vector2 upRight = new Vector2(1, 0.45f);
        Vector2 upLeft = new Vector2(-1, 0.45f);
        StartCoroutine(Zip());

        Debug.Log("Set Colors");
        ghook.SetPlayerColor(1, Color.green);
        ghook.SetPlayerColor(2, Color.yellow);

        while (climbing) {
            Debug.Log("Throw hook 1");
            ghook.AimHook(1, upRight);
            ghook.ThrowHook(1);
            zip1 = true;
            yield return new WaitForSeconds(0.5f);
            ghook.RetractHook(2);
            zip2 = false;

            yield return new WaitForSeconds(1f);

            ghook.AimHook(2, upLeft);
            ghook.ThrowHook(2);
            zip2 = true;
            yield return new WaitForSeconds(0.5f);
            ghook.RetractHook(1);
            zip1 = false;

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Zip() {
        while (true) {
            if (zip1) ghook.ActivateZip(1);
            if (zip2) ghook.ActivateZip(2);
            yield return null;
        }
    }
}
