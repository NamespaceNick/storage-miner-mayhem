using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour {

    public float spawnDelay = 1f;
    public GameObject itemPrefab;

    bool spawning = false;
    GameObject item;

	void Update() {
        if(item == null) {
            Debug.Log("spawning dynamite");
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn() {
        if(!spawning) {
            spawning = true;
            yield return new WaitForSeconds(spawnDelay);
            item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            spawning = false;
        }
    }
}
