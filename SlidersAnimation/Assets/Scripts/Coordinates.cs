using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;
    public float time;

    //Used for hips for animations which translates a bit the character
    public float posX;
    public float posY;
    public float posZ;


    public Coordinates()
    {
        this.time = 0f;
        this.rotX = 0f;
        this.rotY = 0f;
        this.rotZ = 0f;
        this.rotW = 0f;
    }

    public Coordinates(Coordinates obj)
    {
        this.time = obj.time;
        this.rotX = obj.rotX;
        this.rotY = obj.rotY;
        this.rotZ = obj.rotZ;
        this.rotW = obj.rotW;
        this.posX = obj.posX;
        this.posY = obj.posY;
        this.posZ = obj.posZ;
    }
    override
    public string ToString()
    {
        return time.ToString()+", x="+rotX.ToString()+", y="+rotY.ToString()+", z="+rotZ.ToString();
    }
}
