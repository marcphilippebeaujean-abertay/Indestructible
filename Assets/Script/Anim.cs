using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    public Sprite[] preamble;
    public Sprite[] gameplayBackgroundAnim;
    public Sprite[] introAnim;
    public Sprite[] gameOverAnim;
    public Sprite[] tauntAnim;
    public Sprite[] transitionAnim;
    public Sprite menuDisplay;
    public Sprite emptyDisplay;
    public Sprite[] ggOptions;
    bool yup = true;
    Sprite[] curAnim;
    int animCounter = 0;
    float currentAnimTimer = 0.0f;
    float frameTime;
    public float defaultFrameTime = 0.1f;
    public float transitFrameTime = 0.3f;
    bool playAnim = true;
    bool transitioning = true;
	// Use this for initialization
	void Start () {
        curAnim = preamble;
        frameTime = defaultFrameTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (playAnim)
        {
            currentAnimTimer += Time.deltaTime;
            if (currentAnimTimer >= frameTime)
            {
                currentAnimTimer = 0.0f;
                animCounter++;
                if (animCounter >= curAnim.Length)
                {
                    animCounter = 0;
                    if(transitioning)
                    {
                        transitioning = false;
                        playAnim = false;
                        GetComponent<SpriteRenderer>().sprite = emptyDisplay;
                    }
                }
                GetComponent<SpriteRenderer>().sprite = curAnim[animCounter];
            }
        }
	}

    public void toggleAnimPlay(bool shouldPlay)
    {
        playAnim = shouldPlay;
    }

    public bool transitionStatus()
    {
        return transitioning;
    }

    public void clearDisplay()
    {
        playAnim = false;
        GetComponent<SpriteRenderer>().sprite = emptyDisplay;
    }

    public void switchAnim(int animToPlay)
    {
        animCounter = 0;
        switch(animToPlay)
        {
            case 1:
                curAnim = introAnim;
                transitioning = true;
                playAnim = true;
                Debug.Log("switched to intro anim!");
                break;
            case 2:
                playAnim = true;
                frameTime = defaultFrameTime;
                curAnim = gameplayBackgroundAnim;
                break;
            case 3:
                playAnim = true;
                curAnim = gameOverAnim;
                transitioning = true;
                break;
            case 4:
                frameTime = transitFrameTime;
                playAnim = true;
                curAnim = tauntAnim;
                transitioning = true;
                break;
            case 5:
                curAnim = transitionAnim;
                playAnim = true;
                transitioning = true;
                frameTime = transitFrameTime;
                break;
            case 6:
                playAnim = true;
                curAnim = preamble;
                transitioning = true;
                break;
            case 7:
                // display static menu frame
                playAnim = false;
                GetComponent<SpriteRenderer>().sprite = menuDisplay;
                break;
            case 8:
                GetComponent<SpriteRenderer>().sprite = ggOptions[2];
                yup = false;
                break;
            default:
                break;
        }
    }

    public void toggleGiveUpOption(bool option)
    {
        yup = option;
        if(yup)
        {
            GetComponent<SpriteRenderer>().sprite = ggOptions[0];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = ggOptions[1];
        }
    }

    public bool wantsToGiveUp()
    {
        return yup;
    }
}
