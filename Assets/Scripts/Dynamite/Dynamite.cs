using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour {

    // controls how hard to throw dynamite
    public float throwChargeRate = 1f;
    public float maxThrow = 20f;
    // initial rotation of dynamite
    public float initialAngularVelocity = 5f;
    // max sticks of dynamite player can carry
    public int maxDynamite = 2;
    // affects held dynamite visuals
    public Vector3 initialIndicatorLocation = new Vector3(-0.1f, -0.3f, 0f);
    public Vector3 indicatorOffset = new Vector3(0.1f, 0f, 0f);
    // dynamite to be thrown
    public GameObject litDynamitePrefab, heldDynamitePrefab;

    float dynamiteCount = 0f;
    float charge1 = 0f;
    float charge2 = 0f;
    Vector2 dir1, dir2;
    List<GameObject> dynamiteIndicator = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("DynamitePickup") && dynamiteCount < maxDynamite) {
            dynamiteCount++;
            Destroy(collider.gameObject);
            // display dynamite
            if(dynamiteIndicator.Count == 0) {
                dynamiteIndicator.Add(Instantiate(heldDynamitePrefab, transform.position + initialIndicatorLocation, transform.rotation, transform));
            }
            else {
                dynamiteIndicator.Add(Instantiate(heldDynamitePrefab, dynamiteIndicator[dynamiteIndicator.Count-1].transform.position + indicatorOffset, transform.rotation, transform));
            }
        }
    }

    // update the aim vector for the dynamite per player
    public void AimDynamite(int dynNum, Vector2 aimDirection) {
        if(dynNum == 1) {
            dir1 = aimDirection;
        }
        else if(dynNum == 2) {
            dir2 = aimDirection;
        }
        else {
            return;
        }
    }

    // charge dynamite throw per player
    // triggered by holding X
    public void ChargeThrow(int dynNum) {
        if(dynNum == 1) {
            if(dynamiteCount > 0) {
                charge1 += Time.deltaTime * throwChargeRate;
                if(charge1 > maxThrow) {
                    charge1 = maxThrow;
                }
            }
            else {
                charge1 = 0f;
            }
        }
        else if(dynNum == 2) {
            if(dynamiteCount > 0) {
                charge2 += Time.deltaTime * throwChargeRate;
                if(charge2 > maxThrow) {
                    charge2 = maxThrow;
                }
            }
            else {
                charge2 = 0f;
            }
        }
        else {
            return;
        }
    }

    // throw dynamite per player based on current charge and aim
    // triggered by releasing X
    public void ThrowDynamite(int dynNum) {
        Vector2 dir = Vector3.zero;
        //float charge = 0f;
        if(dynNum == 1) {
            dir = dir1;
            //charge = charge1;
            charge1 = 0f;
        }
        else if(dynNum == 2) {
            dir = dir2;
            //charge = charge2;
            charge2 = 0f;
        }
        else {
            return;
        }
        if(dynamiteCount > 0 && dir != Vector2.zero) {
            GameObject litDynamite = Instantiate(litDynamitePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = litDynamite.GetComponent<Rigidbody2D>();
            rb.velocity = GetComponent<Rigidbody2D>().velocity + dir * maxThrow; // charge;
            rb.angularVelocity = initialAngularVelocity;
            dynamiteCount--;
            // display dynamite
            Destroy(dynamiteIndicator[dynamiteIndicator.Count-1]);
            dynamiteIndicator.RemoveAt(dynamiteIndicator.Count-1);
        }
    }
}
