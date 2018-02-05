using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Vector3 positionMod;
    bool active = true;
    MeshRenderer localRenderer;
    public float lifeTime;
    float timer;
    int pressesReq = 1;
    int keyReq = 1;
    public Mesh[] letterMeshes1;
    public Mesh[] letterMeshes2;
    public Mesh[] letterMeshes3;
    public Material[] letterMaterials;
    public GameObject gameManager;

    // Use this for initialization
    void Start () {
        active = true;
        gameManager = GameObject.FindGameObjectWithTag("gamemanager");
        if (gameObject.GetComponent<MeshRenderer>())
        {
            localRenderer = gameObject.GetComponent<MeshRenderer>();
        }
        timer = lifeTime;
        randomiseValues();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(active)
        {
            // update life timer
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                toggleActiveStatus();
                gameManager.GetComponent<GameManager>().missedNode();
                gameManager.GetComponent<GameManager>().updateButtons();
            }
        }
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("collided!");
    //    toggleActiveStatus();
    //}

    public void advanceNode()
    {
        // move node down screen
        transform.position += positionMod;
    }

    public int getKeyReq()
    {
        return keyReq;
    }

    public void toggleActiveStatus()
    {
        // activate / deactivate the object (called in spawner script)
        if(active)
        {
            // deactivate sprite renderer
            localRenderer.enabled = false;
            // toggle active bool
            active = false;
            Debug.Log("setting active to false");
        }
        else
        {
            // reset values for node
            active = true;
            randomiseValues();
            localRenderer.enabled = true;
            timer = lifeTime;
        }
    }

    public void deactivateNode()
    {
        // deactivate sprite renderer
        localRenderer.enabled = false;
        // toggle active bool
        active = false;
    }

    public bool getActiveStatus()
    {
        return active;
    }

    void randomiseValues()
    {
        // randomly generate the nr of button presses required
        pressesReq = Random.Range(1, 4);
        // assign a value used to determine which button needs to be pressed to toggle the number
        keyReq = Random.Range(0, 8);
        Debug.Log(pressesReq + " = required presses");
        // load respective model
        switch(pressesReq)
        {
            // iterate through all mesh lists based (essentially creating a 2D array, since they don't work in unity)
            case 1:
                gameObject.GetComponent<MeshFilter>().mesh = letterMeshes1[keyReq];
                break;
            case 2:
                gameObject.GetComponent<MeshFilter>().mesh = letterMeshes2[keyReq];
                break;
            case 3:
                gameObject.GetComponent<MeshFilter>().mesh = letterMeshes3[keyReq];
                break;
            default:
                Debug.Log("weird RNG");
                break;
        }
        applyMaterial();
        gameManager.GetComponent<GameManager>().updateButtons();
    }

    void applyMaterial()
    {
        localRenderer.material = letterMaterials[(pressesReq - 1)];
    }
    public bool buttonPressed(int pressedButton)
    {
        // check if the node is active
        if (active)
        {
            // check if the pressed button correlates to the one with the node
            if (pressedButton == keyReq)
            {
                // decrement required presses
                pressesReq--;
                // check if the object should be deleted
                if (pressesReq > 0)
                {
                    // update material
                    applyMaterial();
                }
                else
                {
                    // correct button is pressed, deactivate
                    toggleActiveStatus();
                    Debug.Log("Node is being deactivated!");
                    gameManager.GetComponent<GameManager>().updateButtons();
                }
               // we have pressed the right button - return true!
                return true;
            }
        }
        // pressed wrong button or node was not active - return false!
        return false;
    }
}
