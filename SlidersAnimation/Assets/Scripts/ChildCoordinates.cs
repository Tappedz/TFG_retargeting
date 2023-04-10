using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCoordinates : MonoBehaviour
{
    public List<Coordinates> coordinates;
    //public Coordinates originalRot;
    public string path;
    public string nm;


    public ChildCoordinates()
    {
        this.path = "";
        this.nm = "";
        //this.originalRot = new Coordinates();
        this.coordinates = new List<Coordinates>();
    }

    public ChildCoordinates(ChildCoordinates obj)
    {
        this.path = obj.path;
        this.nm = obj.nm;
        //this.originalRot = obj.originalRot;
        this.coordinates = new List<Coordinates>();
        foreach (Coordinates coord in obj.coordinates)
        {
            this.coordinates.Add(new Coordinates(coord));
        }
    }
}
