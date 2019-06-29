using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMusic : MonoBehaviour {
    static LobbyMusic instance;
    SceneInformation info;
    AudioSource audio;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        audio = GetComponent<AudioSource>();
    }

    void Start() {
    }

    IEnumerator FadeOutAndDie() {
        while (audio.volume > 0f) {
            audio.volume -= 0.01f;
            yield return null;
        }

        // instance = null;
        // Destroy(this.gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        info = GameObject.Find("Scene Information").GetComponent<SceneInformation>();

        if (info && info.IsLobby())
        {
            audio.volume = 1f;
        }
        if (info && !info.IsLobby()) {
            if (instance != null)
            {
                StartCoroutine(FadeOutAndDie());
            }
        }
    }
}
