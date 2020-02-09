using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class scrSpawnTerrain : MonoBehaviour {
	public GameObject hex;
	public int z;
	public int x;
	public int y;
    private bool spawnedTerrain;

    // Use this for initialization
    void Start () {
        
	}

	// Update is called once per frame
	void Update () {
        if (!spawnedTerrain)
        {
            SpawnTerrain();
            spawnedTerrain = true;
            Debug.Log("Should have spawned the terrain");
        }

        if (Input.GetKeyDown (KeyCode.J)) {
			SpawnTerrain ();
		}

        /*
		if (Input.GetKeyDown (KeyCode.K)) {
			SaveTerrain ();

			using (StreamWriter writetext = File.AppendText("F:\\Unity\\Hex Based MC\\Assets\\Resources\\myTerrain.txt"))
			{
				writetext.Write("First Line");
				writetext.Close();
			}
		}

		if (Input.GetKeyDown (KeyCode.L)) {
			LoadTerrain ();
		}
        */
	}

	void SaveTerrain () {
		string myStr = "";
		int childNumber = 0;
		foreach (Transform child in GameObject.Find("Terrain").transform) {
			childNumber++;
			Debug.Log (child.name);
			child.GetComponent<scrCustomHex1> ().enabled = true;
			myStr += " " + child.transform.position.x + "$" + child.transform.position.y + "$" + child.transform.position.z + "$" + child.transform.eulerAngles.y + "$" + child.GetComponent<scrCustomHex1> ().nodes.Length + "$";
			foreach (node mNode in child.GetComponent<scrCustomHex1> ().nodes) {
				string newString = mNode.getNodeData();
				myStr += "^" + newString;
			}
			child.GetComponent<scrCustomHex1> ().enabled = false;
		}
		myStr = childNumber + myStr;
		System.IO.File.WriteAllText("F:\\Unity\\Hex Based MC\\Assets\\Resources\\myTerrain.txt", myStr);
	}

	void LoadTerrain () {
		//getting the string
		string myStr = System.IO.File.ReadAllText ("F:\\Unity\\Hex Based MC\\Assets\\Resources\\myTerrain.txt");
		//getting the number of hexes saved
		int hexNumber = int.Parse(myStr.Substring (0, myStr.IndexOf (' ')));
		//Debug.Log (hexNumber);
		//getting rid off the number of hexes and only leaving hex data
		myStr = myStr.Substring (myStr.IndexOf (' ')+1, myStr.Length - myStr.IndexOf (' ') - 1);
		//Debug.Log (myStr);
		//spliting string with all of the hexes into an array of individual hexes
		string[] myHexs = new string[hexNumber];
		myHexs = myStr.Split (new string[] {" "}, System.StringSplitOptions.None);
		//Debug.Log (myHexs.Length);
		foreach (string myHex in myHexs) {
			//spliting overall hex data
			string[] myHexData = new string[5];
			myHexData = myHex.Split (new string[] {"$"}, System.StringSplitOptions.None);
			//Debug.Log (myHexData [0]);
			//Debug.Log (myHexData [1]);
			//Debug.Log (myHexData [2]);
			Vector3 newPos = new Vector3 (float.Parse (myHexData [0]), float.Parse (myHexData [1]), float.Parse (myHexData [2]));
			//getting the number of nodes
			int nodeNumber = int.Parse(myHexData[4]);
			string[] myNodes = new string[nodeNumber];
			//Debug.Log ("My hex Data 5:" + myHexData [5]);
			myHexData [5] = myHexData [5].Substring (1, myHexData [5].Length-1);
			//Debug.Log ("My hex Data 5:" + myHexData [5]);
			myNodes = myHexData[5].Split (new string[] {"^"}, System.StringSplitOptions.None);
			Quaternion newRot = new Quaternion (0, 0, 0, 0);
			GameObject myObj = Instantiate (hex, newPos, newRot);
			Debug.Log(myHexData[3]);
			myObj.transform.Rotate (Vector3.up, float.Parse (myHexData [3]));

			myObj.GetComponent<scrCustomHex1> ().nodes = new node[nodeNumber];
			//I think this is wrong, fix it. I am sure it needs to start at 0, but that doesn't work.........................................................................................................
			for (int i = 0; i < nodeNumber; i++) {
				
				int linkNumber = int.Parse(myNodes[i].Substring (0, myNodes[i].IndexOf ('>')));
				myNodes[i] = myNodes[i].Substring (2, myNodes[i].Length - myNodes[i].IndexOf ('>') - 1);
				string[] nodeData = new string[linkNumber+4];
				nodeData = myNodes[i].Split (new string[] {","}, System.StringSplitOptions.None);
				myObj.GetComponent<scrCustomHex1> ().nodes [i].ID = int.Parse (nodeData [0]);
				myObj.GetComponent<scrCustomHex1> ().nodes [i].links = new int[linkNumber];
				for (int j = 1; j < linkNumber + 1; j++) {
					myObj.GetComponent<scrCustomHex1> ().nodes [i].links[j-1] = int.Parse (nodeData [j]);
				}
				Vector3 nodeLoc = new Vector3(float.Parse (nodeData [nodeData.Length-3]),float.Parse (nodeData [nodeData.Length-2]),float.Parse (nodeData [nodeData.Length-1]));
				myObj.GetComponent<scrCustomHex1> ().nodes [i].location = nodeLoc;
			}

			myObj.GetComponent<scrHexController> ().enabled = true;
			myObj.GetComponent<scrCustomHex1> ().UnInitiateNodes ();
			myObj.GetComponent<scrCustomHex1> ().PopulateVertices ();
			myObj.GetComponent<scrCustomHex1> ().PopulateTriangles ();
			myObj.GetComponent<scrCustomHex1> ().UpdateMesh ();
			myObj.GetComponent<scrCustomHex1> ().enabled = false;
			myObj.transform.parent = GameObject.Find("Terrain").transform;


			//You got the hex now just instanciating it and filling in the nodes.
			//Note: hex location will need to be recorded since right now it's only node location.
			/*
			GameObject myObj = Instantiate (hex, new Vector3 (Mathf.Sqrt(0.75f) * i, k, 1.5f * (i % 2) + j * 3), transform.rotation);
			myObj.GetComponent<scrHexController> ().enabled = true;
			myObj.GetComponent<scrCustomHex1> ().enabled = false;
			myObj.transform.parent = GameObject.Find("Terrain").transform;
			*/

			//replace with a for loop and fill in each node
		}
	}

	void SpawnTerrain(){
		//spawns an array of prefabs
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < z; j++) {
				for (int k = 0; k < y; k++) {	
					GameObject myObj = Instantiate (hex, new Vector3 (Mathf.Sqrt(0.75f) * i, k, 1.5f * (i % 2) + j * 3), transform.rotation);
					myObj.GetComponent<scrHexController> ().enabled = true;
					myObj.GetComponent<scrCustomHex1> ().enabled = false;
					myObj.transform.parent = GameObject.Find("Terrain").transform;
				}

			}
		}
	}


}
