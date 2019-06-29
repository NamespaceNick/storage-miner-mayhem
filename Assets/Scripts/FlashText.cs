using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashText : MonoBehaviour 
{
    public float timeEnabled = 0.6f;

    bool isFlashing = false;
    Coroutine flashCR;

	void Start () 
	{
        IterateRenderers(false);
        StartFlashing();
	}
	
    public void StartFlashing()
    {
        if (isFlashing) return;
        isFlashing = true;
        flashCR = StartCoroutine(Flash());
    }

    public void StopFlashing()
    {
        StopCoroutine(flashCR);
        IterateRenderers(false);
        isFlashing = false;
    }

    void IterateRenderers(bool enable)
    {
        foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>())
        {
            rend.enabled = enable;
        }
    }

    IEnumerator Flash()
    {
        while(true)
        {
            IterateRenderers(true);
            yield return new WaitForSeconds(timeEnabled);
            IterateRenderers(false);
            yield return new WaitForSeconds(timeEnabled);
        }
    }
}
