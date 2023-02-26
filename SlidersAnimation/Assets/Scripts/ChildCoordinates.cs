using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCoordinates : MonoBehaviour
{
    public List<Coordinates> coordinates;
    public string path;
    public string nm;


    public ChildCoordinates()
    {
        this.path = "";
        this.nm = "";
        this.coordinates = new List<Coordinates>();
    }
}
