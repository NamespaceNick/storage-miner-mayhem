using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour {

    public GameObject[] prefabs;

    // Use this for initialization
    void Awake () {
        foreach (GameObject prefab in prefabs) {
            Instantiate(prefab);
        }
    }

    // Update is called once per frame
    void Update () {

    }
}
