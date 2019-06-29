using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles aiming and throwing the pick
public class MiningPick : MonoBehaviour {
    public GameObject thrownPickPrefab;
    public GameObject player;
    public float throwSpeed;

    GameObject thrownPick1;
    GameObject thrownPick2;
    Vector2 direction1;
    Vector2 direction2;
    
    Rigidbody2D playerRb;

    // Use this for initialization
    void Start () {
        playerRb = player.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // update the aim vector for the pick per player
    public void AimPick(int pickNum, Vector2 aimDirection) {
        if (pickNum == 1) direction1 = aimDirection;
        else direction2 = aimDirection;
    }

    
    // TODO: latch onto the grappling hook's indicator
    public void ThrowPick(int pickNum) {
        GameObject pick = GameObject.Instantiate(thrownPickPrefab);
        Vector2 direction;
        if (!thrownPick1.activeSelf && pickNum == 1) {
            thrownPick1 = pick;
            direction = direction1;
        }
        else if (!thrownPick2.activeSelf && pickNum == 2) {
            thrownPick2 = pick;
            direction = direction2;
        } else {
            return;
        }

        if (direction != Vector2.zero) {
            Rigidbody2D pickRb = pick.GetComponent<Rigidbody2D>();
            pick.SetActive(true);
            // the pick destroys when it hits a player or the level geometry
            pick.transform.position = player.transform.position;
            pickRb.velocity = playerRb.velocity + throwSpeed * direction;
            if (pickRb.velocity.magnitude < throwSpeed) {
                pickRb.velocity = pickRb.velocity.normalized * throwSpeed;
            }
        }
    }
}
