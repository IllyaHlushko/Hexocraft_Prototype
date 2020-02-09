using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCustomHex1 : MonoBehaviour {
	public GameObject outlineHex;
	public Vector3[] newVertices;
	public Vector2[] newUV;
	public int[] newTriangles;
	public node[] nodes;

	void Start () {
		//setting up nodes
		nodes = new node[14];
		PopulateNodes ();
	
		PopulateVertices ();
		PopulateTriangles ();
		//newUV = UvCalculator.CalculateUVs (newVertices, 1.0f);
		UpdateMesh ();

	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.R)) {
			transform.Rotate (Vector3.up * 60);
			outlineHex.transform.Rotate (Vector3.up * 60);
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			transform.Rotate (Vector3.up * -60);
			outlineHex.transform.Rotate (Vector3.up * -60);
		}

		if (Input.GetKeyDown (KeyCode.T)) {
			//setting up nodes
			nodes = new node[14];
			PopulateNodes ();

			PopulateVertices ();
			PopulateTriangles ();
			UpdateMesh ();
		}
    }

	//test method to be deleted
	public void ValuesFromAPanel () {
		/*
		int oldNode;
		int newNode;
		GameObject myObj = GameObject.Find ("InputField4old");
		myObj = myObj.transform.FindChild ("Text").gameObject;
		oldNode = int.Parse(myObj.GetComponent<UnityEngine.UI.Text> ().text);

		myObj = GameObject.Find ("InputField4new");
		myObj = myObj.transform.FindChild ("Text").gameObject;
		newNode = int.Parse(myObj.GetComponent<UnityEngine.UI.Text> ().text);

		UnInitiateNodes ();
		MergeVertices (oldNode, newNode);
		PopulateVertices ();
		PopulateTriangles ();
		UpdateMesh ();
		*/
	}

	public void MergeVerticesPublic(int oldVert, int newVert) {
		UnInitiateNodes ();
		MergeVertices (oldVert, newVert);
		PopulateVertices ();
		PopulateTriangles ();
		UpdateMesh ();
	}

	public void UnInitiateNodes () {
		for (int i = 0; i < nodes.Length; i++) {
			nodes [i].setInitiated (false);
		}
	}
		
	public void MergeVertices (int oldVert, int newVert) {
		//************************************************************************* Stage 1: replacing links of the old node ****************************************************************************
		for (int i = 0; i < nodes [oldVert].links.Length; i++) {
			//check if the linked nodes have a link to the new node
			if (nodes [nodes [oldVert].links [i]].checkLinks (newVert)) {
				//if they already do have a link to the new node, delete the link to the old node
				nodes [nodes [oldVert].links [i]].deleteLink(oldVert);
			} else {
				//doesn't have a link to the new node
				//check if the node being tested is the new node
				if (nodes [oldVert].links [i] == newVert) {
					//the node that is being checked is the same as the new node go to the next stage
				} else {
					//the node is not the new node so replace the old connection with new
					nodes [nodes [oldVert].links [i]].links [nodes [nodes [oldVert].links [i]].getLinkID (oldVert)] = newVert;
				}
			}
		}
		//*************************************************************************************************************************************************************************************************

		//************************************************************************* Stage 2: moving the old node to the new node **************************************************************************
		//the ID of the link to the old node in the new node
		int oldLinkID = nodes[newVert].getLinkID(oldVert);
		//the links on the left and right sides of the link to the old ID
		int leftOfOldID;
		int rightOfOldID;
		//setting left side
		if (oldLinkID == 0) {
			//if the link ID is the first link, then the left of it will be the last link
			leftOfOldID = nodes [newVert].links.Length - 1;
		} else {
			//else it's just the left one
			leftOfOldID = oldLinkID - 1;
		}
		//setting right side
		if (oldLinkID == nodes [newVert].links.Length - 1) {
			//if the link ID is the last link, then the right of it will be the first link
			rightOfOldID = 0;
		} else {
			//else it's just the right one
			rightOfOldID = oldLinkID + 1;
		}
		//finding the new size for the links of the new node
		int newSize = nodes[newVert].links.Length - 1 + nodes[oldVert].links.Length - 3;
		//using size to create a new links array
		int[] newLinks = new int[newSize];
		//external tracker for the new list
		int y = 0;
		//populate the new links with the links up to the link to the old node
		for (int i = 0; i < oldLinkID; i++) {
			newLinks [y] = nodes [newVert].links [i];
			y++;
		}

		//the ID of the link to the new node
		int newLinkID = nodes[oldVert].getLinkID(newVert);
		//the links on the left and right sides of the link to the old ID
		int leftOfNewID;
		int rightOfNewID;
		//setting left side
		if (newLinkID == 0) {
			//if the link ID is the first link, then the left of it will be the last link
			leftOfNewID = nodes [oldVert].links.Length - 1;
		} else {
			//else it's just the left one
			leftOfNewID = newLinkID - 1;
		}
		//setting right side
		if (newLinkID == nodes [oldVert].links.Length - 1) {
			//if the link ID is the last link, then the right of it will be the first link
			rightOfNewID = 0;
		} else {
			//else it's just the right one
			rightOfNewID = newLinkID + 1;
		}
		//add the links between the right and lef
		if (rightOfNewID > 1){
			if (rightOfNewID < nodes [oldVert].links.Length - 1) {
				for (int i = rightOfNewID + 1; i < nodes [oldVert].links.Length; i++) {
					newLinks [y] = nodes [oldVert].links [i];
					y++;
				}
			} else {
				for (int i = 0; i < leftOfNewID; i++) {
					newLinks [y] = nodes [oldVert].links [i];
					y++;
				}
			}
		} else {
			for (int i = rightOfNewID + 1; i < leftOfNewID; i++) {
				newLinks [y] = nodes [oldVert].links [i];
				y++;
			}
		}
		//adding the rest of the links
		for (int i = oldLinkID + 1; i < nodes[newVert].links.Length; i++) {
			newLinks [y] = nodes [newVert].links [i];
			y++;
		}
		//slapping the old links with the new ones
		nodes[newVert].links = newLinks;
		//*************************************************************************************************************************************************************************************************

		//***************************************************************************** Stage 3: deleting the vertex **************************************************************************************
		//external counter for new nodes
		int z = 0;
		//new nodes that are 1 less than the old ones
		node[] newNodes = new node[nodes.Length - 1];
		//adding the half before the node that is being deleted
		for (int i = 0; i < oldVert; i++) {
			newNodes [z] = nodes [i];
			z++;
		}
		//adding the half after the node that is being deleted
		for (int i = oldVert + 1; i < nodes.Length; i++) {
			newNodes [z] = nodes [i];
			z++;
		}
		//replacing the old nodes with the new ones
		nodes = newNodes;
		//*************************************************************************************************************************************************************************************************

		//***************************************************************************** Stage 4: reorganize the node links ********************************************************************************
		//do it with every node
		for (int i = 0; i < nodes.Length; i++) {
			//for every link of the node
			for (int j = 0; j < nodes [i].links.Length; j++) {
				if (nodes [i].links [j] > oldVert) {
					nodes [i].links [j]--;
				}
			}
		}
		//*************************************************************************************************************************************************************************************************
	}

	public void UpdateMesh () {
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		outlineHex.GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = newVertices;
		mesh.triangles = newTriangles;
		mesh.uv = newUV;
		mesh.RecalculateNormals ();
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	//need to re-think this
	public void PopulateTriangles () {
		int tryLength = 72 - (14 - nodes.Length) * 6;
		newTriangles = new int[tryLength];

		int x = 0;
		//for each node
		for (int i = 0; i < nodes.Length; i++) {
			//note, maybe check if the node has been initiated
			if (!nodes[i].initiated) {
				//creating triangles for this node, all but the last one
				for (int j = 0; j < nodes[i].links.Length - 1; j++) {
					if ((!nodes [nodes[i].links [j]].initiated) && (!nodes [nodes[i].links [j + 1]].initiated)) {
						newTriangles [x] = nodes[i].links [j];
						x++;
						newTriangles [x] = i;
						x++;
						newTriangles [x] = nodes[i].links [j + 1];
						x++;
					}
				}
				//creating the last triangle
				if ((!nodes [nodes[i].links [0]].initiated) && (!nodes [nodes[i].links [nodes[i].links.Length - 1]].initiated)) {
					newTriangles [x] = nodes[i].links [nodes[i].links.Length - 1];
					x++;
					newTriangles [x] = i;
					x++;
					newTriangles [x] = nodes[i].links [0];
					x++;
				}

				nodes[i].setInitiated (true);
			}
		}

	}

	public void PopulateVertices () {
		newVertices = new Vector3[nodes.Length];
		for (int i = 0; i < nodes.Length; i++) {
			newVertices [i] = nodes [i].location;
		}
	}

	public void PopulateNodes() {
		nodes [0].location = new Vector3 (0, -0.5f, 0);
		nodes [0].ID = 0;
		nodes [0].links = new int[6];
		nodes [0].links [0] = 1;
		nodes [0].links [1] = 2;
		nodes [0].links [2] = 3;
		nodes [0].links [3] = 4;
		nodes [0].links [4] = 5;
		nodes [0].links [5] = 6;

		nodes [1].location = new Vector3 (0, -0.5f, 1);
		nodes [1].ID = 1;
		nodes [1].links = new int[4];
		nodes [1].links [0] = 0;
		nodes [1].links [1] = 6;
		nodes [1].links [2] = 8;
		nodes [1].links [3] = 2;

		nodes [2].location = new Vector3 (Mathf.Sqrt(0.75f), -0.5f, 0.5f);
		nodes [2].ID = 2;
		nodes [2].links = new int[6];
		nodes [2].links [0] = 0;
		nodes [2].links [1] = 1;
		nodes [2].links [2] = 8;
		nodes [2].links [3] = 9;
		nodes [2].links [4] = 10;
		nodes [2].links [5] = 3;

		nodes [3].location = new Vector3 (Mathf.Sqrt(0.75f), -0.5f, -0.5f);
		nodes [3].ID = 3;
		nodes [3].links = new int[4];
		nodes [3].links [0] = 0;
		nodes [3].links [1] = 2;
		nodes [3].links [2] = 10;
		nodes [3].links [3] = 4;

		nodes [4].location = new Vector3 (0, -0.5f, -1);
		nodes [4].ID = 4;
		nodes [4].links = new int[6];
		nodes [4].links [0] = 0;
		nodes [4].links [1] = 3;
		nodes [4].links [2] = 10;
		nodes [4].links [3] = 11;
		nodes [4].links [4] = 12;
		nodes [4].links [5] = 5;

		nodes [5].location = new Vector3 (-Mathf.Sqrt(0.75f), -0.5f, -0.5f);
		nodes [5].ID = 5;
		nodes [5].links = new int[4];
		nodes [5].links [0] = 0;
		nodes [5].links [1] = 4;
		nodes [5].links [2] = 12;
		nodes [5].links [3] = 6;

		nodes [6].location = new Vector3 (-Mathf.Sqrt(0.75f), -0.5f, 0.5f);
		nodes [6].ID = 6;
		nodes [6].links = new int[6];
		nodes [6].links [0] = 0;
		nodes [6].links [1] = 5;
		nodes [6].links [2] = 12;
		nodes [6].links [3] = 13;
		nodes [6].links [4] = 8;
		nodes [6].links [5] = 1;

		nodes [7].location = new Vector3 (0, 0.5f, 0);
		nodes [7].ID = 7;
		nodes [7].links = new int[6];
		nodes [7].links [0] = 13;
		nodes [7].links [1] = 12;
		nodes [7].links [2] = 11;
		nodes [7].links [3] = 10;
		nodes [7].links [4] = 9;
		nodes [7].links [5] = 8;

		nodes [8].location = new Vector3 (0, 0.5f, 1);
		nodes [8].ID = 8;
		nodes [8].links = new int[6];
		nodes [8].links [0] = 9;
		nodes [8].links [1] = 2;
		nodes [8].links [2] = 1;
		nodes [8].links [3] = 6;
		nodes [8].links [4] = 13;
		nodes [8].links [5] = 7;

		nodes [9].location = new Vector3 (Mathf.Sqrt(0.75f), 0.5f, 0.5f);
		nodes [9].ID = 9;
		nodes [9].links = new int[4];
		nodes [9].links [0] = 10;
		nodes [9].links [1] = 2;
		nodes [9].links [2] = 8;
		nodes [9].links [3] = 7;

		nodes [10].location = new Vector3 (Mathf.Sqrt(0.75f), 0.5f, -0.5f);
		nodes [10].ID = 10;
		nodes [10].links = new int[6];
		nodes [10].links [0] = 11;
		nodes [10].links [1] = 4;
		nodes [10].links [2] = 3;
		nodes [10].links [3] = 2;
		nodes [10].links [4] = 9;
		nodes [10].links [5] = 7;

		nodes [11].location = new Vector3 (0, 0.5f, -1);
		nodes [11].ID = 11;
		nodes [11].links = new int[4];
		nodes [11].links [0] = 12;
		nodes [11].links [1] = 4;
		nodes [11].links [2] = 10;
		nodes [11].links [3] = 7;

		nodes [12].location = new Vector3 (-Mathf.Sqrt(0.75f), 0.5f, -0.5f);
		nodes [12].ID = 12;
		nodes [12].links = new int[6];
		nodes [12].links [0] = 13;
		nodes [12].links [1] = 6;
		nodes [12].links [2] = 5;
		nodes [12].links [3] = 4;
		nodes [12].links [4] = 11;
		nodes [12].links [5] = 7;

		nodes [13].location = new Vector3 (-Mathf.Sqrt(0.75f), 0.5f, 0.5f);
		nodes [13].ID = 13;
		nodes [13].links = new int[4];
		nodes [13].links [0] = 8;
		nodes [13].links [1] = 6;
		nodes [13].links [2] = 12;
		nodes [13].links [3] = 7;
	}
}
