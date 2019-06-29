using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainAngle : MonoBehaviour {

    // keeps the object in it's inital orientation forever
    Quaternion initialRotation;

	void Start() {
        initialRotation = Quaternion.identity;
    }

	void LateUpdate() {
        transform.rotation = initialRotation;
	}
}
