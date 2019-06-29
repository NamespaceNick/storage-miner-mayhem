using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOscillate : MonoBehaviour 
{
    public float lowerVal, upperVal;
    public float scaleIncrement = 1f;
    public float differenceBuffer = 0.5f;
    public float scaleLerp = 0.01f;
    public float scaleTimeInterval = 2f;
    public AnimationCurve curve;
    public Image img;



    Coroutine oscillateCR;
    Vector3 newScale;
    Vector3 lowerScale, upperScale;

	void Start () 
	{
        upperScale.Set(upperVal, upperVal, 1);
        lowerScale.Set(lowerVal, lowerVal, 1);
        oscillateCR = StartCoroutine(StartOscillating());
	}
	
    IEnumerator StartOscillating()
    {
        float timer;
        while (true)
        {
            timer = 0f;
            while (Vector3.Distance(img.rectTransform.localScale, upperScale) > differenceBuffer)
            {
                img.rectTransform.localScale = Vector3.Lerp(img.rectTransform.localScale, upperScale, curve.Evaluate(timer/scaleTimeInterval));
                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0f;
            while (Vector3.Distance(img.rectTransform.localScale, lowerScale) > differenceBuffer)
            {
                img.rectTransform.localScale = Vector3.Lerp(img.rectTransform.localScale, lowerScale, curve.Evaluate(timer/scaleTimeInterval));
                timer += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }
}
