using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPoint : MonoBehaviour {
	public int myID;
	public int previousID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver () {
		Debug.Log ("Mouse is over " + myID);
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if (previousID < 0) {
				//first press
				GetComponentInParent<scrHexController>().EnterEditMode (myID);
			} else {
				//second press
				GetComponentInParent<scrCustomHex1>().MergeVerticesPublic(previousID,myID);
				GetComponentInParent<scrHexController> ().DeletePoints ();
				GetComponentInParent<scrHexController>().EnterEditMode (-1);
			}
		}
	}

	void OnMouseExit () {
		Debug.Log ("Mouse left " + myID);
	}
}
