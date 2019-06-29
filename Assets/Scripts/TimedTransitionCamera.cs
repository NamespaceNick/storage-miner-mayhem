
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimedTransitionCamera : MonoBehaviour {

    public Material transitMat;
    public Texture transitImage;
    public GameObject renderCanvas;

    private float speedMult = 2f;
    private GameObject tRenderer;

    private void Awake() {
        //FadeCameraIn();
    }

    private void Start() {
        Debug.Log("START");
        GameObject[] cams = GameObject.FindGameObjectsWithTag("MainCamera");
        transitMat.SetTexture("_TransitionTex", transitImage);

        InstantiateRenderTexture();
        StartCoroutine(FadeOut());
        StartCoroutine(FadeIn());
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, transitMat);
    }

    public void FadeCameraIn() {
        StartCoroutine(FadeIn());
    }

    public void FadeCameraOut() {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut() {
        Debug.Log("FADING OUT");
        yield return new WaitForSeconds(1f);
        float transitionValue = transitMat.GetFloat("_Transition");
        Debug.Log("Fading out");
        while (transitionValue < 1) {
            transitMat.SetFloat("_Transition", transitionValue += Time.deltaTime * speedMult);
            yield return null;
        }
    }

    public IEnumerator FadeIn() {
        Debug.Log("FADING IN");
        yield return new WaitForSeconds(2f);
        InstantiateRenderTexture();

        float transitionValue = transitMat.GetFloat("_Transition");
        Debug.Log("Fading in");
        while (transitionValue > 0) {
            transitMat.SetFloat("_Transition", transitionValue -= Time.deltaTime * speedMult);
            yield return null;
        }
    }

    private void InstantiateRenderTexture() {
        // Debug.Log("RENDERER " + tRenderer);
        if(tRenderer == null) {
            // Debug.Log("RENDERER " + (tRenderer == null));
            RenderTexture rend =  new RenderTexture(Screen.width, Screen.height, 24);
            tRenderer = Instantiate(renderCanvas);
            RawImage[] renderers = tRenderer.GetComponentsInChildren<RawImage>();
            renderers[0].texture = rend;
            GetComponent<Camera>().targetTexture = rend;
        }
    }
}
