using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public float w;
    public float time;


    public Coordinates()
    {
        this.time = 0f;
        this.x = 0f;
        this.y = 0f;
        this.z = 0f;
        this.w = 0f;
}
    override
    public string ToString()
    {
        return time.ToString()+", x="+x.ToString()+", y="+y.ToString()+", z="+z.ToString();
    }
}
