using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Automagically pulse in size. Only used by PMT logo.
public class SizePulse : MonoBehaviour {

    public bool shrink;

    Vector3 small = new Vector3(0.25f, 0.25f, 0.25f);
    Vector3 big = new Vector3(1.3f, 1.3f, 1.3f);

    // Update is called once per frame
    void Update () {
        Vector3 targetSize;

        if (shrink) {
            targetSize = small;
        } else {
            targetSize = big;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, 0.01f);

        if (Vector3.Distance(transform.localScale, targetSize) < 0.05f ||
            Random.Range(0f, 1f) > 0.99f) {
            shrink = !shrink;
        }
    }
}
