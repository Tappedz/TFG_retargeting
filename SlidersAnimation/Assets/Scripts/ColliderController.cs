using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    SphereCollider coll;
    MeshRenderer meshR;

    public Material good;
    public Material bad;
    // Start is called before the first frame update
    void Start()
    {
        //coll = this.GetComponent<SphereCollider>();
        meshR = this.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider col)
    {
        bool added = false;
        if (!col.tag.Equals(this.tag))
        {
            foreach (Transform parent in Menu.collidedObjects)
            {
                if(parent.name.Equals(this.transform.parent.name))
                {
                    added = true;
                }
            }
            if (!added)
            {
                Menu.collidedObjects.Add(this.transform.parent);
            }
            changeColor(bad);
        }
    }

    void changeColor(Material mat)
    {
        Material[] materials = meshR.materials;
        materials[0] = mat;
        meshR.materials = materials;
        meshR.material = materials[0];
    }
}
