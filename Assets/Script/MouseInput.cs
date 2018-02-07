using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{

    public int key;
    private GameManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.FindGameObjectWithTag("gamemanager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        manager.InitButtonPress(key);
    }
}
