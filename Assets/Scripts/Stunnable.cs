using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunnable : MonoBehaviour {

    public float stunTime;
    public GameObject stunPrefab;

    GrapplingHook gHook;
    bool stunned;
    // Use this for initialization
    void Start () {
        gHook = GetComponent<GrapplingHook>();
    }

    public bool IsStunned() {
        return stunned;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Pick") || collision.gameObject.CompareTag("Hook")) {
            StartCoroutine(StunRoutine());
        }
    }

    IEnumerator StunRoutine() {
        stunned = true;
        gHook.RetractHook(1);
        gHook.RetractHook(2);
        GameObject stunInstance = GameObject.Instantiate(stunPrefab);
        stunInstance.GetComponent<FollowWithOffset>().target = this.gameObject;
        yield return new WaitForSecondsRealtime(stunTime);
        Destroy(stunInstance);
        stunned = false;
    }

}
