using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageRope : MonoBehaviour {

    public GameObject player;
    public GameObject hook;
    public GameObject ropeSegmentPrefab;
    public float segmentLength = 1f;

    Grapple grapple;
    List<GameObject> rope;
    float ropeLength = 0f;

    void Start() {
        grapple = hook.GetComponent<Grapple>();
        rope = new List<GameObject>();
        // instantiate rope segments at appropriate intervals to form a maxDistance long rope
        for(float f = 0; f < grapple.maxDistance; f += segmentLength) {
            rope.Add(Instantiate(ropeSegmentPrefab, new Vector3(0, f, 0), Quaternion.identity, transform));
        }
        ropeLength = segmentLength * rope.Count;
    }

    void Update() {
        // if hook is out, display rope
        if(player.activeSelf && hook.activeSelf) {
            // if hook is latched, stretch rope according to rope length
            if(grapple.Hooked()) {
                SetRopeActive(Mathf.Max(1, (int) (grapple.GetRopeDistance() / segmentLength)));
                transform.localScale = new Vector3(transform.localScale.x, (hook.transform.position - player.transform.position).magnitude / ropeLength, transform.localScale.z);
            }
            // else, do not scale rope and add segments according to distance from player
            else {
                SetRopeActive((int) ((hook.transform.position - player.transform.position).magnitude / segmentLength));
                transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
            }
            //set rotation and position
            transform.position = hook.transform.position;
            transform.up = player.transform.position - hook.transform.position;
        }
        // else, hide rope
        else {
            SetRopeActive(0);
        }
    }

    // sets the first numSegments active, modifies ropeLength accordingly
    void SetRopeActive(int numSegments) {
        if(numSegments > rope.Count) {
            numSegments = rope.Count;
        }
        for(int i = 0; i < numSegments; i++) {
            rope[i].SetActive(true);
        }
        for(int i = numSegments; i < rope.Count; i++) {
            rope[i].SetActive(false);
        }
        ropeLength = segmentLength * numSegments;
    }

    public void SetPlayerColor(Color color) {
        for(int i = 0; i < rope.Count; i++) {
            GameObject ropeVis = rope[i].transform.Find("Visual").gameObject;
            foreach (Transform child in ropeVis.transform) {
                child.gameObject.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
