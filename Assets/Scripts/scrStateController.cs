using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrStateController : MonoBehaviour {
	public bool editing;
	public GameObject panel;
	public GameObject camera;
	public GameObject player;
	// Use this for initialization
	void Start () {
		editing = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (editing)
            {
                //go back in game
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                panel.SetActive(false);
                camera.SetActive(false);
                player.SetActive(true);
                GameObject myObj = GameObject.Find("Custom Hex");
                myObj.layer = 0;
                myObj.GetComponent<scrHexController>().StopEditing();
            }
            else
            {
                //go into editing
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                panel.SetActive(true);
                camera.SetActive(true);
                player.SetActive(false);
                GameObject myObj = GameObject.Find("Custom Hex");
                myObj.layer = 8;
                myObj.GetComponent<scrHexController>().EnterEditMode(-1);
            }
            editing = !editing;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
