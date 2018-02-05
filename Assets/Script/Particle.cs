using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    public Sprite[] particleSprites;
    public float animRate;
    float animTimer = 0.0f;
    int animCounter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        animTimer += Time.deltaTime;
        if (animTimer >= animRate)
        {
            animTimer = 0.0f;
            Debug.Log(animCounter);
            if (animCounter >= particleSprites.Length)
            {
                Destroy(this.gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = particleSprites[animCounter];
                animCounter++;
            }
        }
    }
}
