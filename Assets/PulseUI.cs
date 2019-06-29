using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseUI : MonoBehaviour 
{
	public float startScale, maxScale;
	public float periodInterval = 3f;
	public bool is3D = false;
	public AnimationCurve easingPulseAnimation;

	SpriteRenderer rend;
	float scaleModifier;

	void Start () 
	{
		rend = GetComponentInChildren<SpriteRenderer> ();
		if (rend == null) Debug.LogWarning ("pulse script has no sprite renderer attached");
		scaleModifier = maxScale - startScale;
		StartCoroutine (Pulse());
	}

	IEnumerator Pulse()
	{
		Debug.Log ("Entered Pulse()");
		float startTime = Time.time;
		float scaleNum;
		while (true)
		{
			yield return null;
			scaleNum = startScale + (scaleModifier * easingPulseAnimation.Evaluate (startTime));
			rend.transform.localScale = new Vector3 (scaleNum, scaleNum, is3D ? scaleNum : 0);
			startTime += Time.unscaledDeltaTime;
			Debug.Log ("Pulsing, value now = " + scaleNum);
		}
	}

}
