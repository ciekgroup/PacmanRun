using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRoads : MonoBehaviour {
    int totalpixels;
    Texture2D tex;
    int[,] points;
    Vector3[] nodes;
    int counter; // check if there are two lines made
    int nodecounter = 0;
    bool alreadynode = false;
    bool nodeinregion = false; 
    float deltax;
    float deltay;
    public GameObject debugSphere;
    Coordtrans coordtrans;
    GoogleMap googlemap;
    private void Start()
    {
        totalpixels = Screen.width * Screen.height;
        coordtrans = GetComponent<Coordtrans>();
        googlemap = GetComponent<GoogleMap>();
        Gizmos.color = Color.green;
        nodes = new Vector3[100];
        if (googlemap.loadOnStart) { googlemap.Refresh(); }//Will run over, set time as need be. Fix later    
    }
    void SpawnDebugSpheres(Vector3[] nodes, Texture2D tex, GameObject debugSphere, Coordtrans coordtrans)//Just to see where everything is at
    {
        foreach(Vector3 x in nodes)
        {
            Instantiate(debugSphere, coordtrans.TexPixeltoWorldVector(x.x, x.y, tex), Quaternion.identity);
        }
    }
    void EvaluatePoints(int[,] points, Vector3[] nodes, int nodecounter)//Evaluate points that are to be made nodes.
    {
        Debug.Log("EvaluatePoints call");
        for(int x = 0; x < Screen.width; x+=2)
        {
            for(int y = 0; y < Screen.height; y+=2)
            {
                if(points[x,y] == 2)//if ever second pixel (should be measured how much higher it can go for performance reasons)
                {
                    if (x >= 8 && y >= 8 && x <= Screen.width - 8 && y <= Screen.height - 8)//Check not out of range
                    {
    
                        alreadynode = false;//Changes to false at the start of each iteration
                        if (points[x, y - 8] == 2)//Check for below
                        {
                            if (points[x + 12, y] == 2)//Check for right and already node had no chance to be changed to true
                            {
                                foreach (Vector2 node in nodes)
                                {
                                    if (Mathf.Abs(node.x-x) >= 10 && Mathf.Abs(node.y - y) >= 10) // node some distance away? Repeated over scripts
                                    {
                                        nodeinregion = false;
                                    }
                                    else
                                    {
                                        nodeinregion = true;
                                        Debug.Log("Node in region");
                                    }
                                }
                                    if (nodeinregion == false)
                                    {
                                    nodes[nodecounter] = new Vector3(x, y, -2.5f);
                                    alreadynode = true;
                                    Debug.Log(coordtrans.TexPixeltoWorldVector(x, y, tex).x + "," + coordtrans.TexPixeltoWorldVector(x, y, tex).y);
                                    Instantiate(debugSphere, coordtrans.TexPixeltoWorldVector(x, y, tex), Quaternion.identity);
                                }
                               
                            }//right
                            if (points[x - 12, y] == 2 && alreadynode == false)//Check for left
                            {
                                foreach (Vector2 node in nodes)
                                {
                                    if (Mathf.Abs(x - node.x) >= 10 && Mathf.Abs(y - node.y) >= 10)
                                    {
                                        nodeinregion = false;
                                    }
                                    else
                                    {
                                        nodeinregion = true;
                                        Debug.Log("Node in region");
                                    }
                                }
                                if (nodeinregion == false)
                                {
                                    nodes[nodecounter] = new Vector3(x, y, -2.5f);
                                    alreadynode = true;
                                    Debug.Log(coordtrans.TexPixeltoWorldVector(x, y, tex).x + "," + coordtrans.TexPixeltoWorldVector(x, y, tex).y);
                                    Instantiate(debugSphere, coordtrans.TexPixeltoWorldVector(x, y, tex), Quaternion.identity);
                                }
                            }//left
                        }//down
                        if(points[x,y+8] == 2 && alreadynode == false)//check up
                        {
                            if (points[x + 12, y] == 2)//Check for right and already node had no chance to be changed to true
                            {
                                foreach (Vector2 node in nodes)
                                {
                                    if (Mathf.Abs(x - node.x) >= 10 && Mathf.Abs(y - node.y) >= 10)
                                    {
                                        nodeinregion = false;
                                    }
                                    else
                                    {
                                        nodeinregion = true;
                                        Debug.Log("Node in Region");
                                    }
                                }
                                if (nodeinregion == false)
                                {
                                    nodes[nodecounter] = new Vector3(x, y, -2.5f);
                                    alreadynode = true;
                                    Debug.Log(coordtrans.TexPixeltoWorldVector(x, y, tex).x + "," + coordtrans.TexPixeltoWorldVector(x, y, tex).y);
                                    Instantiate(debugSphere, coordtrans.TexPixeltoWorldVector(x, y, tex), Quaternion.identity);
                                }
                            }//right
                            if (points[x - 12, y] == 2 && alreadynode == false)//Check for left
                            {
                                foreach (Vector2 node in nodes)
                                {
                                    if (Mathf.Abs(x - node.x) >= 10 && Mathf.Abs(y - node.y) >= 10)
                                    {
                                        nodeinregion = false;
                                    }
                                    else
                                    {
                                        nodeinregion = true;
                                        Debug.Log("Node in region");
                                    }
                                }
                                if (nodeinregion == false)
                                {
                                    nodes[nodecounter] = new Vector3(x, y, -2.5f);
                                    alreadynode = true;
                                    Debug.Log(coordtrans.TexPixeltoWorldVector(x, y, tex).x + "," + coordtrans.TexPixeltoWorldVector(x, y, tex).y);
                                    Instantiate(debugSphere, coordtrans.TexPixeltoWorldVector(x, y, tex), Quaternion.identity);
                                }
                            }//left
                        }//up
                    }//bounds check
                }//if ponts(x,y) = 2
            }//ScreenHeight
        }//ScreenWidth
        
    }
    public void PointGen ()//called from googlemaps, third script
    {
        
        Debug.Log(Screen.height * Screen.width);
        points = new int[Screen.width, Screen.height];
        Debug.Log("Total pixels: " + totalpixels + "."); 
        Debug.Log(Screen.width + "/" + Screen.height);
        //Total amount of pixels to loop through
        tex = GetComponent<Renderer>().material.mainTexture as Texture2D;
        Debug.Log("Are we doing this or what?");
        //loop through pixels
        
        for(int x = 0; x < Screen.width; x++)//check width
        {
            for(int y = 0; y < Screen.height; y++)//check height
            {
                if(tex.GetPixel(x,y).r >= 0.99f && tex.GetPixel(x,y).g >= .99f && tex.GetPixel(x,y).b >= .99f && tex.GetPixel(x,y).a >= .99f)//if white, blue
                {
                    Debug.Log("White one");
                    tex.SetPixel(x, y, Color.blue);
                    if (x > 0 && x < Screen.width)// if not out of range
                    {
                        if (x%6==0)
                        {
                            points[x, y] = 2;
                            counter += 1;
                        }//5th pixel check 
                        else
                        {
                            points[x, y] = 1;
                        }
                    }//bounds check
                }//color white check
                else
                {
                    points[x, y] = 0;
                }
                
            }//for Screen height
        }//for Screen width
        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;
        EvaluatePoints(points,nodes,nodecounter);
        
    }//End Pointsgen
    void CutPoints(int[,] points)
    {
        for(int x = 0; x < Screen.width; x++)
        {

        }
    }

    // Update is called once per frame
}
