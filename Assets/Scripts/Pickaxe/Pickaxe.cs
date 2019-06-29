using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles aiming and throwing the pick
public class Pickaxe : MonoBehaviour {
    public GameObject thrownPickPrefab;
    public GameObject player;
    public float throwSpeed;
    public float pickOffset;
    public float pushBackFactor;
    public float throwCooldown;

    GameObject thrownPick1;
    GameObject thrownPick2;
    Vector2 direction1;
    Vector2 direction2;

    float pick1Timer;
    float pick2Timer;

    Rigidbody2D playerRb;

    // Use this for initialization
    void Start () {
        playerRb = player.GetComponent<Rigidbody2D>();
        pick1Timer = 0f;
        pick2Timer = 0f;
    }

    private void Update() {
        if (pick1Timer > 0f) pick1Timer -= Time.deltaTime;
        if (pick2Timer > 0f) pick2Timer -= Time.deltaTime;
    }

    // update the aim vector for the pick per player
    public void AimPick(int pickNum, Vector2 aimDirection) {
        if (pickNum == 1) direction1 = aimDirection;
        else direction2 = aimDirection;
    }

    public void ThrowPick(int pickNum) {
        GameObject pick = GameObject.Instantiate(thrownPickPrefab);
        Vector2 direction;
        // make sure that the pick doesnt exist and the cooldown timer is also at 0
        if (thrownPick1 == null && pickNum == 1 && pick1Timer <= 0f) {
            thrownPick1 = pick;
            direction = direction1;
            pick1Timer = throwCooldown;
        }
        else if (thrownPick2 == null && pickNum == 2 && pick2Timer <= 0f) {
            thrownPick2 = pick;
            direction = direction2;
            pick2Timer = throwCooldown;
        } else {
            // don't allow more than one pick per player
            Destroy(pick);
            return;
        }

        if (direction != Vector2.zero) {
            Rigidbody2D pickRb = pick.GetComponent<Rigidbody2D>();
            pick.SetActive(true);
            // the pick destroys when it hits a player or the level geometry
            Vector3 spawnPos = new Vector3(direction.normalized.x, direction.normalized.y);
            pick.transform.position = player.transform.position + spawnPos * pickOffset;
            pickRb.velocity = playerRb.velocity + throwSpeed * direction;
            if (pickRb.velocity.magnitude < throwSpeed) {
                pickRb.velocity = pickRb.velocity.normalized * throwSpeed;
            }
            // add a bit of pushback to the player
            playerRb.AddForce(direction * -pushBackFactor, ForceMode2D.Impulse);
        } else {
            Destroy(pick);
            return;
        }
    }
}
