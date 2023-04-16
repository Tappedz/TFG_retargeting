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
        coll = this.GetComponent<SphereCollider>();
        meshR = this.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            changeColor(good);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals(this.tag))
        {
            Debug.Log(this.transform.parent.name);
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
