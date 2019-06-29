using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithOffset : MonoBehaviour {

    public GameObject target;
    public Vector3 offset = Vector3.zero;

    void LateUpdate() {
        transform.position = target.transform.position + offset;
    }
}
