using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrHexController : MonoBehaviour {
	public float durability;
	public Color startColor;
	public Color highlightColor;
    public float highlightModifier;
	public GameObject[] myPoints;

    public UnityEngine.UI.Text redText;
    public UnityEngine.UI.Text greenText;
    public UnityEngine.UI.Text blueText;
    public UnityEngine.UI.Text alphaText;

    // Use this for initialization
    void Start () {
		//setting up colors
        GetComponent<Renderer>().material.color = startColor;
        highlightModifier = 0.2f;
        //highlightColor = new Color(0.8f, 0.8f, 0.8f);
        durability = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (durability < 0) {
			Destroy (this.gameObject);
		} else {

		}
	}

	public void Highlight () {
        highlightColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer> ().material.color = new Color(highlightColor.r + highlightModifier, highlightColor.g + highlightModifier, highlightColor.b + highlightModifier, highlightColor.a);
	}

	public void UnHighlight () {
		GetComponent<Renderer> ().material.color = highlightColor;
	}

	public void EnterEditMode(int ID) {
		if (ID < 0) {
			GameObject point = GameObject.Find ("point");
			Vector3[] myVert = GetComponent<MeshFilter> ().mesh.vertices;
			myPoints = new GameObject[myVert.Length];
			for (int i = 0; i < myVert.Length; i++) {
				myPoints [i] = Instantiate (point, transform.position + myVert [i], point.transform.rotation, transform);
				myPoints [i].transform.localPosition = myVert [i];
				myPoints [i].layer = 8;
				myPoints [i].GetComponent<scrPoint> ().myID = i;
				myPoints [i].GetComponent<scrPoint> ().previousID = -1;
			}
		} else {
			DeletePoints ();
			GameObject point = GameObject.Find ("point");
			Vector3[] myVert = new Vector3[GetComponent<scrCustomHex1>().nodes[ID].links.Length];
			for (int i = 0; i < myVert.Length; i++) {
				myVert[i] = GetComponent<scrCustomHex1> ().nodes [GetComponent<scrCustomHex1>().nodes[ID].links[i]].location;
			}
			myPoints = new GameObject[myVert.Length];
			for (int i = 0; i < myVert.Length; i++) {
				myPoints [i] = Instantiate (point, transform.position + myVert [i], point.transform.rotation, transform);
				myPoints [i].transform.localPosition = myVert [i];
				myPoints [i].layer = 8;
				myPoints [i].GetComponent<scrPoint> ().myID = GetComponent<scrCustomHex1>().nodes[ID].links[i];
				myPoints [i].GetComponent<scrPoint> ().previousID = ID;
			}
		}
	}

	public void StopEditing() {
		DeletePoints ();
	}

	public void DeletePoints () {
		for (int i = 0; i < myPoints.Length; i++) {
			Destroy (myPoints [i]);
		}
		myPoints = null;
	}

	public void ChangeColour (string RGB) {
		string[] RGBs = RGB.Split (new string[]{" "}, System.StringSplitOptions.None);

        Color myColor = new Color (float.Parse(RGBs[0]), float.Parse(RGBs[1]), float.Parse(RGBs[2]), float.Parse(RGBs[3]));
        //Clamping the ranges for each colour to 0-1, 0 being 0% colour and 1 being 100%
        startColor = new Color(Mathf.Clamp((startColor.r + myColor.r), 0, 1), Mathf.Clamp((startColor.g + myColor.g), 0, 1), Mathf.Clamp((startColor.b + myColor.b), 0, 1), Mathf.Clamp((startColor.a + myColor.a), 0, 1));
        GetComponent<Renderer> ().material.color = startColor;

        ResetColorText();
    }

    public void ResetColorText ()
    {
        redText.text = "Red " + Mathf.Round(startColor.r * 100.0f).ToString() + "%";
        greenText.text = "Green " + Mathf.Round(startColor.g * 100.0f).ToString() + "%";
        blueText.text = "Blue " + Mathf.Round(startColor.b * 100.0f).ToString() + "%";

        alphaText.text = "Alpha " + Mathf.Round(startColor.a * 100.0f).ToString() + "%";
    }
}
