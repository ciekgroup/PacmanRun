using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordtrans : MonoBehaviour {
    public float[] DisassembleVector3(Vector3 dvector)//When stuff needs to be disassembled and put into something else, this is the script to shorten the code
    {
        float x = dvector.x;
        float y = dvector.y;
        float z = dvector.z;
        float[] xyz = new float[2];
        xyz[0] = x;
        xyz[1] = y;
        xyz[2] = z;
        
        return (xyz);
    }
    public float[] DisassembleVector2(Vector2 dvector)//When stuff needs to be disassembled and put into something else, this is the script to shorten the code, now with vector 2!
    {
        float x = dvector.x;
        float y = dvector.y;
        float[] xyz = new float[2];
        xyz[0] = x;
        xyz[1] = y;

        return (xyz);
    }
    public Vector2 TexPixeltoWorldVector(float x, float y, Texture2D tex) // translate texture x and y pixel coordinates to a (x,y) world vector
    {
        float worldx;
        float worldy;
        float objectSizex = GetComponent<Renderer>().bounds.size.x;
        float objectSizey = GetComponent<Renderer>().bounds.size.y;

        worldx = (x * objectSizex-1) / 300;//Replace with size of texture
        worldy = (y * objectSizey-1) / 300;//Replace with size of texture

        Vector2 worldPoint = new Vector2(worldx, worldy);
        Debug.Log("TexPixeltoWorldVector called");
        return (worldPoint);
    }
}
