using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrHexPlaceDetector : MonoBehaviour {
	public Camera myCamera;
	public GameObject Hex;
	public GameObject HexOutline;
	public GameObject selectedHex;
    public bool buildMode;
    public bool outlineEnables;

	void Start(){
        buildMode = true;
        outlineEnables = true;
        myCamera = transform.GetComponent<Camera> ();
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            buildMode = !buildMode;
            if (selectedHex)
            {
                if (selectedHex.GetComponent<scrHexController>())
                {
                    selectedHex.GetComponent<scrHexController>().UnHighlight();
                }
                selectedHex = null;
                HexOutline.transform.position = Vector3.zero + Vector3.down * 10.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            outlineEnables = !outlineEnables;
            HexOutline.transform.position = Vector3.zero + Vector3.down * 10.0f;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Hardcoded reset
            this.transform.parent.transform.position = new Vector3(25.0f, 50.0f, 20.0f);
        }

        if (buildMode)
        {
            RaycastHit hit;
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 5))
            {
                if (selectedHex)
                {
                    if (selectedHex.GetComponent<scrHexController>())
                    {
                        selectedHex.GetComponent<scrHexController>().UnHighlight();
                    }
                    selectedHex = hit.transform.gameObject;
                    if (selectedHex.GetComponent<scrHexController>())
                    {
                        selectedHex.GetComponent<scrHexController>().Highlight();
                    }
                }
                else
                {
                    selectedHex = hit.transform.gameObject;
                    if (selectedHex.GetComponent<scrHexController>())
                    {
                        selectedHex.GetComponent<scrHexController>().Highlight();
                    }
                }
            }
            else
            {
                if (selectedHex)
                {
                    if (selectedHex.GetComponent<scrHexController>())
                    {
                        selectedHex.GetComponent<scrHexController>().UnHighlight();
                    }
                    selectedHex = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (Physics.Raycast(ray, out hit, 5))
                {
                    hit.transform.GetComponent<scrHexController>().durability--;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Physics.Raycast(ray, out hit, 5))
                    {
                        // Copy shape
                        hit.transform.GetComponent<scrCustomHex1>().enabled = true;
                        Hex.GetComponent<scrCustomHex1>().nodes = hit.transform.GetComponent<scrCustomHex1>().nodes;
                        hit.transform.GetComponent<scrCustomHex1>().enabled = false;
                        Hex.GetComponent<scrCustomHex1>().UnInitiateNodes();
                        Hex.GetComponent<scrCustomHex1>().PopulateVertices();
                        Hex.GetComponent<scrCustomHex1>().PopulateTriangles();
                        Hex.GetComponent<scrCustomHex1>().UpdateMesh();
                    }
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (Physics.Raycast(ray, out hit, 5))
                    {
                        // Copy colour
                        //Debug.Log("Doing the colour thing");
                        Color HitColor = hit.transform.GetComponent<Renderer>().material.color;
                        float colorModifier = Hex.GetComponent<scrHexController>().highlightModifier;
                        HitColor = new Color(HitColor.r - colorModifier, HitColor.g - colorModifier, HitColor.b - colorModifier, HitColor.a);
                        Hex.GetComponent<Renderer>().material.color = HitColor;
                        Hex.GetComponent<scrHexController>().startColor = HitColor;
                        Hex.GetComponent<scrHexController>().ResetColorText();

                        Color outlineColor = new Color(HitColor.r, HitColor.g, HitColor.b, 0.5f);
                        HexOutline.GetComponent<Renderer>().material.color = outlineColor;
                    }
                }
                else
                {
                    // TODO: move into a separate function instead of having copies of code or change the logic

                    // Copy shape
                    hit.transform.GetComponent<scrCustomHex1>().enabled = true;
                    Hex.GetComponent<scrCustomHex1>().nodes = hit.transform.GetComponent<scrCustomHex1>().nodes;
                    hit.transform.GetComponent<scrCustomHex1>().enabled = false;
                    Hex.GetComponent<scrCustomHex1>().UnInitiateNodes();
                    Hex.GetComponent<scrCustomHex1>().PopulateVertices();
                    Hex.GetComponent<scrCustomHex1>().PopulateTriangles();
                    Hex.GetComponent<scrCustomHex1>().UpdateMesh();

                    // Copy colour
                    //Debug.Log("Doing the colour thing");
                    Color HitColor = hit.transform.GetComponent<Renderer>().material.color;
                    float colorModifier = Hex.GetComponent<scrHexController>().highlightModifier;
                    HitColor = new Color(HitColor.r - colorModifier, HitColor.g - colorModifier, HitColor.b - colorModifier, HitColor.a);
                    Hex.GetComponent<Renderer>().material.color = HitColor;
                    Hex.GetComponent<scrHexController>().startColor = HitColor;
                    Hex.GetComponent<scrHexController>().ResetColorText();

                    Color outlineColor = new Color(HitColor.r, HitColor.g, HitColor.b, 0.5f);
                    HexOutline.GetComponent<Renderer>().material.color = outlineColor;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Physics.Raycast(ray, out hit, 5))
                {
                    Debug.DrawLine(ray.origin, hit.point);
                    //Transform objectHit = hit.transform;
                    Vector3 myPos = new Vector3();

                    if (Mathf.Abs(hit.point.y - hit.transform.position.y) >= 0.49f)
                    {
                        //hit.transform.position = hit.transform.position + Vector3.up;
                        //Debug.Log ("Clicked top or bottom");
                        myPos.y = hit.transform.position.y + 1 * Mathf.Sign(hit.point.y - hit.transform.position.y);
                        myPos.x = hit.transform.position.x;
                        myPos.z = hit.transform.position.z;

                        //Debug.Log ("x=" + myPos.x);
                        //Debug.Log ("y=" + myPos.y);
                        //Debug.Log ("z=" + myPos.z);
                    }
                    else
                    {
                        myPos.y = hit.transform.position.y;
                        //Debug.Log ("y=" + myPos.y);
                        if (Mathf.Abs(hit.point.z - hit.transform.position.z) <= 0.49f)
                        {
                            //Debug.Log ("Clicked flat bit");
                            myPos.z = hit.transform.position.z;
                            myPos.x = hit.transform.position.x + Mathf.Sqrt(0.75f) * Mathf.Sign(hit.point.x - hit.transform.position.x) * 2;
                            //myPos.z = hit.transform.position.z + 1.5f * Mathf.Sign (hit.point.z - hit.transform.position.z);
                            //myPos.x = hit.transform.position.x;
                            //Debug.Log ("z=" + myPos.z);
                        }
                        else
                        {
                            //Debug.Log ("Clicked edges");
                            myPos.x = hit.transform.position.x + Mathf.Sqrt(0.75f) * Mathf.Sign(hit.point.x - hit.transform.position.x);
                            myPos.z = hit.transform.position.z + 1.5f * Mathf.Sign(hit.point.z - hit.transform.position.z);
                            //myPos.z = hit.transform.position.y + 1.5f * Mathf.Sign (hit.point.z - hit.transform.position.z);
                            //myPos.x = hit.transform.position.x + Mathf.Sqrt(0.75f) * Mathf.Sign (hit.point.x - hit.transform.position.x) * 2;
                            //Debug.Log ("z=" + myPos.z);
                            //Debug.Log ("x=" + myPos.x);
                        }
                    }

                    GameObject myObj = Instantiate(Hex, myPos, Hex.transform.rotation);
                    myObj.GetComponent<scrHexController>().enabled = true;
                    //Destroy (myObj.GetComponent<scrCustomHex> ());
                    //Destroy (myObj.GetComponent<scrCustomHex1> ());
                    myObj.GetComponent<scrCustomHex1>().enabled = false;
                    myObj.transform.parent = GameObject.Find("Terrain").transform;
                }
            }

            //outline trial

            if (outlineEnables)
            {
                RaycastHit hit2;
                Ray ray2 = myCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray2, out hit2, 5))
                {
                    Vector3 myPos = new Vector3();
                    if (Mathf.Abs(hit2.point.y - hit2.transform.position.y) >= 0.49f)
                    {
                        //hit.transform.position = hit.transform.position + Vector3.up;
                        //Debug.Log ("Clicked top or bottom");
                        myPos.y = hit2.transform.position.y + 1 * Mathf.Sign(hit2.point.y - hit2.transform.position.y);
                        myPos.x = hit2.transform.position.x;
                        myPos.z = hit2.transform.position.z;
                    }
                    else
                    {
                        myPos.y = hit2.transform.position.y;
                        if (Mathf.Abs(hit2.point.z - hit2.transform.position.z) <= 0.49f)
                        {
                            //Debug.Log ("Clicked flat bit" + Mathf.Abs(hit2.point.y - hit2.transform.position.y).ToString());
                            myPos.z = hit2.transform.position.z;
                            myPos.x = hit2.transform.position.x + Mathf.Sqrt(0.75f) * Mathf.Sign(hit2.point.x - hit2.transform.position.x) * 2;
                        }
                        else
                        {
                            //Debug.Log ("Clicked edges" + Mathf.Abs(hit2.point.y - hit2.transform.position.y).ToString());
                            myPos.x = hit2.transform.position.x + Mathf.Sqrt(0.75f) * Mathf.Sign(hit2.point.x - hit2.transform.position.x);
                            myPos.z = hit2.transform.position.z + 1.5f * Mathf.Sign(hit2.point.z - hit2.transform.position.z);
                        }
                    }
                    HexOutline.transform.position = myPos;
                }
                else
                {
                    HexOutline.transform.position = Vector3.zero + Vector3.down * 10.0f;
                }
            }
        }
	}

    public void ChangeColour(string RGB)
    {
        string[] RGBs = RGB.Split(new string[] { " " }, System.StringSplitOptions.None);

        Color startColor = Hex.transform.GetComponent<Renderer>().material.color;
        Color addColor = new Color(float.Parse(RGBs[0]), float.Parse(RGBs[1]), float.Parse(RGBs[2]));
        //Clamping the ranges for each colour to 0-1, 0 being 0% colour and 1 being 100%
        startColor = new Color(Mathf.Clamp((startColor.r + addColor.r), 0, 1), Mathf.Clamp((startColor.g + addColor.g), 0, 1), Mathf.Clamp((startColor.b + addColor.b), 0, 1));

        startColor = new Color(startColor.r, startColor.g, startColor.b, 0.5f);
        HexOutline.GetComponent<Renderer>().material.color = startColor;
    }
}
