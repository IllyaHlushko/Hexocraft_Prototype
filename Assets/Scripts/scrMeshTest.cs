using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrMeshTest : MonoBehaviour {
	public Vector3[] newVertices;
	public Vector2[] newUV;
	public int[] newTriangles;

	// Use this for initialization
	void Start () {
		meshUpdate ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			meshUpdate ();
		}
	}

	void meshUpdate() {
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		mesh.vertices = newVertices;
		mesh.uv = newUV;
		mesh.triangles = newTriangles;
	}
}

[System.Serializable]
public struct node {
	public Vector3 location;
	public int ID;
	public int[] links;
	public bool initiated;

	public void setInitiated (bool myState) {
		initiated = myState;
	}

	public string getNodeData() {
		string myData = "";
		myData += links.Length;
		myData += ">" + ID;
		foreach (int link in links) {
			myData += "," + link;
		}
		myData += "," + location.x + "," + location.y + "," + location.z;

		return myData;
	}

	//check if the link value is part of the array
	public bool checkLinks (int linkValue) {
		bool result = false;
		for (int i = 0; i < links.Length; i++) {
			if (links [i] == linkValue) {
				result = true;
				break;
			}
		}
		return result;
	}

	//get the ID of the link of value linkValue
	public int getLinkID (int linkValue) {
		int linkID = -1;
		for (int i = 0; i < links.Length; i++) {
			if (links [i] == linkValue) {
				linkID = i;
				break;
			}
		}
		return linkID;
	}

	/// <summary>
	/// Deletes the link.
	/// </summary>
	/// <param name="linkValue">Link value.</param>
	public void deleteLink(int linkValue) {
		if (checkLinks(linkValue)) {
			//creating new array that will be 1 shorter then the current one
			int[] newLinks = new int[links.Length - 1];
			//geting the ID of the link that needs to be deleted
			int linkID = getLinkID (linkValue);
			//setting an exteran counter for the new array and starting to work
			int x = 0;
			//adding all of the links before the link that is being deleted
			for (int i = 0; i < linkID; i++) {
				newLinks [x] = links [i];
				x++;
			}
			//adding all of the lnks after the one that is being deleted
			if (linkID + 1 < links.Length) {
				for (int i = linkID + 1; i < links.Length; i++) {
					newLinks [x] = links [i];
					x++;
				}
			}
			//putting the new links into the old ones
			links = newLinks;
		}
	}
}
