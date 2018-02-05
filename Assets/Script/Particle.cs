using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    public Sprite[] particleSprites;
    public float animRate;
    float animTimer = 0.0f;
    int animCounter = 0;
    private bool active;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start ()
	{
	    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	    active = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (active)
	    {
	        animTimer += Time.deltaTime;
	        if (animTimer >= animRate)
	        {
	            animTimer = 0.0f;
	            if (animCounter >= particleSprites.Length)
	            {
	                // deactivate spriteRenderer
	                spriteRenderer.enabled = false;
                    // reset variables
	                active = false;
	                animCounter = 0;
	            }
	            else
	            {
	                GetComponent<SpriteRenderer>().sprite = particleSprites[animCounter];
	                animCounter++;
	            }
	        }
	    }
	}

    public void activateParticle()
    {
        active = true;
    }

    public bool isActive()
    {
        return active;
    }
}
