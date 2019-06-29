using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAvatar : MonoBehaviour 
{
    public GameObject[] imagesToDisable;
    public GameObject[] imagesToEnable;

    bool hasShot = false;
    GameObject ghost;
    public GrapplingHook gHook;
    Rigidbody2D rb;

    RigidbodyConstraints2D frozen = RigidbodyConstraints2D.FreezePositionX |
        RigidbodyConstraints2D.FreezePositionY |
        RigidbodyConstraints2D.FreezeRotation;

	void Start () 
	{
        gHook = GetComponent<GrapplingHook>();
        rb = GetComponent<Rigidbody2D>();
        ghost = transform.Find("Ghost").gameObject;
        ghost.SetActive(false);
	}

    void Update()
    {
        if (!hasShot)
        {
            hasShot = true;
            Debug.Log("Shooting");
            gHook.AimHook(1, Vector2.up);
            gHook.ThrowHook(1);
            gHook.lReticle.SetActive(false);
            gHook.rReticle.SetActive(false);
            gHook.lIndicator.SetActive(false);
            gHook.rIndicator.SetActive(false);
            ModifyImages(imagesToEnable, false);
        }
    }

    void ModifyImages(GameObject[] images, bool makeFlash)
    {
        for (int i = 0; i < images.Length; ++i)
        {
            if (makeFlash)
                images[i].GetComponent<FlashText>().StartFlashing();
            else
                images[i].GetComponent<FlashText>().StopFlashing();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Hook"))
        {

            Debug.Log("I hit myself");
            if (imagesToDisable.Length > 0)
            {
                ModifyImages(imagesToDisable, false);
            }
            if (imagesToEnable.Length > 0)
            {
                ModifyImages(imagesToEnable, true);
            }
        }
        if (collision.transform.CompareTag("Danger"))
        {
            rb.constraints = frozen;
            SpriteRenderer sprite = ghost.GetComponent<SpriteRenderer>();
            sprite.color = Color.white;

        }
    }
}
