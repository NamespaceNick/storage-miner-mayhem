using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaddleRotateSprite : MonoBehaviour {

    Transform spriteTransform;

    public float rotationLimit = 10f;
    public float rotationSpeed = 2f;
    public enum Axis { X, Y, Z };
    public Axis selectedAxis = Axis.X;
    public bool rotating = false;

	// Use this for initialization
	void Start () {
		// Grab the sprite, whether from a sprite renderer or from an image
        if (gameObject.GetComponent<Image>())
            spriteTransform = gameObject.GetComponent<Image>().transform;
        else if (gameObject.GetComponent<SpriteRenderer>())
            spriteTransform = gameObject.GetComponent<SpriteRenderer>().transform;
        else
            Debug.Log("WaddleRotateSprite called on something without an image or a sprite renderer");
	}
	
	// Update is called once per frame
	void Update () {
		if (rotating) {
            switch(selectedAxis) {
                case Axis.X:
                    spriteTransform.rotation = Quaternion.Euler(rotationLimit * Mathf.Sin(Time.time * rotationSpeed), 0, 0);
                    break;
                case Axis.Y:
                    spriteTransform.rotation = Quaternion.Euler(0, rotationLimit * Mathf.Sin(Time.time * rotationSpeed), 0);
                    break;
                case Axis.Z:
                    spriteTransform.rotation = Quaternion.Euler(0, 0, rotationLimit * Mathf.Sin(Time.time * rotationSpeed));
                    break;
            };
        }
            
	}
}
