
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionCamera : MonoBehaviour {

    public Material transitMat;
    public Texture transitImage;
    public GameObject renderCanvas;

    private float speedMult = 2f;
    private GameObject tRenderer;

    private void Awake() {
        //FadeCameraIn();
    }

    private void Start() {
        GameObject[] cams = GameObject.FindGameObjectsWithTag("MainCamera");
        transitMat.SetTexture("_TransitionTex", transitImage);

        InstantiateRenderTexture();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) 
    {
        Graphics.Blit(source, destination, transitMat);
    }

    public void FadeCameraIn() 
    {
        StartCoroutine(FadeIn());
    }

    public void FadeCameraOut() 
    {
        // TODO: LEFT OFF HERE
        StartCoroutine(FadeOut());
    }

    // handles screen fade out & loads new scene
    public IEnumerator FadeOut(int nextSceneIndex) 
    {
        float transitionValue = transitMat.GetFloat("_Transition");
        while (transitionValue < 1) 
        {
            transitMat.SetFloat("_Transition", transitionValue += Time.deltaTime * speedMult);
            yield return null;
        }
        SceneManager.LoadSceneAsync(nextSceneIndex);
    }

    public IEnumerator FadeIn() 
    {
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
