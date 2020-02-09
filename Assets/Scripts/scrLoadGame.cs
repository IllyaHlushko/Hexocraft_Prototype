using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrLoadGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadGame()
    {
        Application.LoadLevel("test");
    }
}
