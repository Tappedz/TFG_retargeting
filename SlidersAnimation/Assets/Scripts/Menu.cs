using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    float timer = 0f;

    public CameraMovement camera;

    Vector3[] vertex;
    List<GameObject> collidersList = new List<GameObject>();
    public GameObject vertexPoint;

    public GameObject dummyPrefab;
    public GameObject bigHeadDummy;
    public GameObject bigArmsDummy;
    public GameObject bigLegsDummy;

    GameObject dummy;
    GameObject clone;
    public Transform[] dummyChilds;
    public Transform[] cloneChilds;
    Animation animation1;
    Animation animation2;
    public AnimationClip firstAnim;
    AnimationClip animModified;

    List<ChildCoordinates> dummyChildsData = new List<ChildCoordinates>();
    List<ChildCoordinates> cloneChildsData = new List<ChildCoordinates>();
    List<ChildCoordinates> cloneChildsPaths = new List<ChildCoordinates>(); // placeholder of paths, can't iterate either way
    List<ChildCoordinates> auxDataCopy; // used to change value on sliders without losing original values


    Slider legsSlider;
    Slider armsSlider;
    Toggle ch1Toggle;
    Toggle ch2Toggle;
    Toggle ch3Toggle;

    bool animationRecorded;

    public void Start()
    {
        dummy = GameObject.FindGameObjectWithTag("originalDummy");
        legsSlider = GameObject.FindGameObjectWithTag("legsSlider").GetComponent<Slider>();
        armsSlider = GameObject.FindGameObjectWithTag("armsSlider").GetComponent<Slider>();
        ch1Toggle = GameObject.FindGameObjectWithTag("ch1Toggle").GetComponent<Toggle>();
        ch2Toggle = GameObject.FindGameObjectWithTag("ch2Toggle").GetComponent<Toggle>();
        ch3Toggle = GameObject.FindGameObjectWithTag("ch3Toggle").GetComponent<Toggle>();
        dummyChilds = dummy.GetComponentsInChildren<Transform>();
        getLimbsPaths(dummyChilds, dummyChildsData);
        animation1 = dummy.GetComponent<Animation>();

        animationRecorded = false;
        ch1Toggle.isOn = false; 
        ch2Toggle.isOn = false; 
        ch3Toggle.isOn = false;

        firstAnim.name = "mixamo";
        firstAnim.legacy = true;
        animation1.AddClip(firstAnim, firstAnim.name);
        animation1.clip = firstAnim;
        timer = 0f;

        //legsSliderCustomListener.slider = legsSlider;
        //armsSliderCustomListener.slider = armsSlider;

        legsSlider.value = 0;
        legsSlider.minValue = -0.25f;
        legsSlider.maxValue = 0.25f;

        armsSlider.value = 0;
        armsSlider.minValue = -0.25f;
        armsSlider.maxValue = 0.25f;

        ch1Toggle.onValueChanged.AddListener(delegate
        {
            if(ch1Toggle.isOn)
            {
                ch2Toggle.isOn = false;
                ch3Toggle.isOn = false;

                cloneChildsPaths.Clear();
                cloneChildsData.Clear();
                collidersList.Clear();
                GameObject.Destroy(clone);

                clone = Instantiate(dummyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cloneChilds = clone.GetComponentsInChildren<Transform>();
                getLimbsPaths(cloneChilds, cloneChildsPaths);
                adjustDataToClone();
                createColliders(clone, clone.transform.Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>().sharedMesh);
                Transform head = clone.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head");
                head.localScale = new Vector3(3,3,3);

                auxDataCopy = cloneChildsData.ConvertAll(x => new ChildCoordinates(x));

                animation2 = clone.GetComponent<Animation>();
                saveAnimation();
            }
        });

        ch2Toggle.onValueChanged.AddListener(delegate
        {
            if (ch2Toggle.isOn)
            {
                ch1Toggle.isOn = false;
                ch3Toggle.isOn = false;

                cloneChildsPaths.Clear();
                cloneChildsData.Clear();
                collidersList.Clear();
                GameObject.Destroy(clone);

                clone = Instantiate(dummyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cloneChilds = clone.GetComponentsInChildren<Transform>();
                getLimbsPaths(cloneChilds, cloneChildsPaths);
                adjustDataToClone();
                createColliders(clone, clone.transform.Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>().sharedMesh);
                Transform leftSh = clone.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder");
                Transform rightSh = clone.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder");
                leftSh.localScale = new Vector3(2,2,2);
                rightSh.localScale = new Vector3(2,2,2);

                auxDataCopy = cloneChildsData.ConvertAll(x => new ChildCoordinates(x));

                animation2 = clone.GetComponent<Animation>();
                saveAnimation();
            }
        });

        ch3Toggle.onValueChanged.AddListener(delegate
        {
            if (ch3Toggle.isOn)
            {
                ch1Toggle.isOn = false;
                ch2Toggle.isOn = false;

                cloneChildsPaths.Clear();
                cloneChildsData.Clear();
                collidersList.Clear();
                GameObject.Destroy(clone);

                clone = Instantiate(dummyPrefab, new Vector3(0, 0.45f, 0), Quaternion.identity);
                cloneChilds = clone.GetComponentsInChildren<Transform>();
                getLimbsPaths(cloneChilds, cloneChildsPaths);
                adjustDataToClone();
                createColliders(clone, clone.transform.Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>().sharedMesh);
                Transform leftLeg = clone.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg");
                Transform rightLeg = clone.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg");
                leftLeg.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                rightLeg.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                
                auxDataCopy = cloneChildsData.ConvertAll(x => new ChildCoordinates(x));

                animation2 = clone.GetComponent<Animation>();
                saveAnimation();
            }
        });

        legsSlider.onValueChanged.AddListener(delegate
        {
            int dataCount = 0;
            foreach (ChildCoordinates coords in cloneChildsData)
            {
                int coordCount;
                if (coords.nm.Equals("mixamorig:LeftUpLeg"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        /*
                        Quaternion auxQuat = new Quaternion(auxDataCopy[dataCount].coordinates[coordCount].rotX, auxDataCopy[dataCount].coordinates[coordCount].rotY, auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value, auxDataCopy[dataCount].coordinates[coordCount].rotW);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - legsSlider.value*100);
                        //Quaternion changeQuat = auxQuat * Quaternion.Euler(-legsSlider.value * 100, 0f, 0f);
                        coord.rotX = auxQuat.x;
                        coord.rotY = auxQuat.y;
                        coord.rotZ = auxQuat.z;
                        coord.rotW = auxQuat.w;
                        coordCount++;
                        */
                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value * 1.5f;
                        //coord.rotX = auxDataCopy[dataCount].coordinates[coordCount].rotX + legsSlider.value;
                        coordCount++;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightUpLeg"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        /*
                        Quaternion auxQuat = new Quaternion(auxDataCopy[dataCount].coordinates[coordCount].rotX, auxDataCopy[dataCount].coordinates[coordCount].rotY, auxDataCopy[dataCount].coordinates[coordCount].rotZ + legsSlider.value, auxDataCopy[dataCount].coordinates[coordCount].rotW);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + legsSlider.value * 100);
                        //Quaternion changeQuat = auxQuat * Quaternion.Euler(legsSlider.value * 100, 0f, 0f);
                        coord.rotX = auxQuat.x;
                        coord.rotY = auxQuat.y;
                        coord.rotZ = auxQuat.z;
                        coord.rotW = auxQuat.w;
                        coordCount++;
                        */
                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value * 1.5F;
                        //coord.rotX = auxDataCopy[dataCount].coordinates[coordCount].rotX + legsSlider.value;
                        coordCount++;
                    }
                }

                if (coords.nm.Equals("mixamorig:LeftLeg"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        /*
                        Quaternion auxQuat = new Quaternion(auxDataCopy[dataCount].coordinates[coordCount].rotX, auxDataCopy[dataCount].coordinates[coordCount].rotY, auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value, auxDataCopy[dataCount].coordinates[coordCount].rotW);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - legsSlider.value*100);
                        //Quaternion changeQuat = auxQuat * Quaternion.Euler(-legsSlider.value * 100, 0f, 0f);
                        coord.rotX = auxQuat.x;
                        coord.rotY = auxQuat.y;
                        coord.rotZ = auxQuat.z;
                        coord.rotW = auxQuat.w;
                        coordCount++;
                        */
                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value / 2;
                        coordCount++;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightLeg"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        /*
                        Quaternion auxQuat = new Quaternion(auxDataCopy[dataCount].coordinates[coordCount].rotX, auxDataCopy[dataCount].coordinates[coordCount].rotY, auxDataCopy[dataCount].coordinates[coordCount].rotZ + legsSlider.value, auxDataCopy[dataCount].coordinates[coordCount].rotW);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + legsSlider.value * 100);
                        //Quaternion changeQuat = auxQuat * Quaternion.Euler(legsSlider.value * 100, 0f, 0f);
                        coord.rotX = auxQuat.x;
                        coord.rotY = auxQuat.y;
                        coord.rotZ = auxQuat.z;
                        coord.rotW = auxQuat.w;
                        coordCount++;
                        */
                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ + legsSlider.value / 2;
                        coordCount++;
                    }
                }
                if (coords.nm.Equals("mixamorig:LeftFoot"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        /*
                        Quaternion auxQuat = new Quaternion(auxDataCopy[dataCount].coordinates[coordCount].rotX, auxDataCopy[dataCount].coordinates[coordCount].rotY, auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value, auxDataCopy[dataCount].coordinates[coordCount].rotW);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - legsSlider.value*100);
                        //Quaternion changeQuat = auxQuat * Quaternion.Euler(-legsSlider.value * 100, 0f, 0f);
                        coord.rotX = auxQuat.x;
                        coord.rotY = auxQuat.y;
                        coord.rotZ = auxQuat.z;
                        coord.rotW = auxQuat.w;
                        coordCount++;
                        */
                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ + legsSlider.value / 2;
                        coordCount++;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightFoot"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        /*
                        Quaternion auxQuat = new Quaternion(auxDataCopy[dataCount].coordinates[coordCount].rotX, auxDataCopy[dataCount].coordinates[coordCount].rotY, auxDataCopy[dataCount].coordinates[coordCount].rotZ + legsSlider.value, auxDataCopy[dataCount].coordinates[coordCount].rotW);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + legsSlider.value * 100);
                        //Quaternion changeQuat = auxQuat * Quaternion.Euler(legsSlider.value * 100, 0f, 0f);
                        coord.rotX = auxQuat.x;
                        coord.rotY = auxQuat.y;
                        coord.rotZ = auxQuat.z;
                        coord.rotW = auxQuat.w;
                        coordCount++;
                        */
                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value / 2;
                        coordCount++;
                    }
                }
                dataCount++;
            }
            saveAnimation();
            
        });

        armsSlider.onValueChanged.AddListener(delegate
        {
            int dataCount = 0;
            foreach (ChildCoordinates coords in cloneChildsData)
            {
                int coordCount;
                /*
                if (coords.nm.Equals("mixamorig:LeftForeArm"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ + armsSlider.value;
                        coordCount++;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightForeArm"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - armsSlider.value;
                        coordCount++;
                    }
                }
                */
                if (coords.nm.Equals("mixamorig:LeftArm"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ + armsSlider.value;
                        coord.rotY = auxDataCopy[dataCount].coordinates[coordCount].rotY + armsSlider.value;

                        coordCount++;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightArm"))
                {
                    coordCount = 0;
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - armsSlider.value;
                        coord.rotY = auxDataCopy[dataCount].coordinates[coordCount].rotY - armsSlider.value;

                        coordCount++;
                    }
                }
                dataCount++;
            }
            saveAnimation();
        });
        //clon.SetActive(false);
        //captureAnimation();
    }

    public void Update()
    {
        foreach (GameObject vertexPoint in collidersList)
        {
            vertexPoint.transform.rotation = vertexPoint.transform.parent.rotation;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("animacion en clon");
            playCustomAnimation();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("animacion en clon");
            if(ch1Toggle.isOn || ch2Toggle.isOn || ch3Toggle.isOn)
            {
                animation2.Play();
                Debug.Log(animation2.clip);
            }
        }
        if (!animationRecorded || !camera.camLocked)
        {
            legsSlider.interactable = false;
            armsSlider.interactable = false;
            ch1Toggle.interactable = false;
            ch2Toggle.interactable = false;
            ch3Toggle.interactable = false;
        }
        else
        {
            legsSlider.interactable = true;
            armsSlider.interactable = true;
            ch1Toggle.interactable = true;
            ch2Toggle.interactable = true;
            ch3Toggle.interactable = true;
        }
    }

    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!animationRecorded)
            {
                captureAnimation();
                 
            }
            else
            {
                playAnimation();
            }
        }
    }

    public void playAnimation()
    {
        animation1.Play();
    }

    public void captureAnimation()
    {
        playAnimation();
        Debug.Log("Comienza grabacion");
        StartCoroutine(captureFrames());  
    }

    String getPath(Transform tr)
    {
        if(tr.parent != null)
        {
            return getPath(tr.parent) + "/" + tr.name;
        }
        else
        {
            return tr.name;
        }
    }

    public void getLimbsPaths(Transform[] childs, List<ChildCoordinates> childCoordinatesList)
    {
        foreach (Transform child in childs)
        {
            //coger el path de cada hijo y guardarlo en parentNm ****
            ChildCoordinates chCoord = new ChildCoordinates();
            chCoord.nm = child.name;
            //Debug.Log(chCoord.nm);
            try
            {
                if (chCoord.nm.Equals(childs[0].name))
                {
                    chCoord.path = "/" + childs[0].name;
                }
                else
                {
                    chCoord.path = "/" + getPath(child);
                    //Debug.Log(chCoord.parentNm);
                }
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("ERROR");
            }
            childCoordinatesList.Add(chCoord);
        }
    }

    IEnumerator captureFrames()
    {
        //meter Time.deltaTime (referencia al tiempo real de ejecucion) --> ya esta con coroutine
        //Debug.Log("firstAnim.length en capture: "+firstAnim.length);
        for (timer = 0f; timer < firstAnim.length; timer += 0.05f)
        {
            int j = 0;
            foreach (Transform child in dummyChilds)
            {
                Coordinates aux = new Coordinates();
                aux.rotX = child.localRotation.x;
                aux.rotY = child.localRotation.y;
                aux.rotZ = child.localRotation.z;
                aux.rotW = child.localRotation.w;
                if (child.name.Equals("mixamorig:Hips"))
                {
                    aux.posX = child.localPosition.x;
                    aux.posY = child.localPosition.y;
                    aux.posZ = child.localPosition.z;
                } 
                aux.time = timer;
                /*if (child.name.Equals("mixamorig:Hips") || child.name.Equals("mixamorig:Spine") || child.name.Equals("mixamorig:Spine1") || child.name.Equals("mixamorig:Spine2") 
                    || child.name.Equals("mixamorig:LeftShoulder") || child.name.Equals("mixamorig:Neck") || child.name.Equals("mixamorig:RightShoulder") || child.name.Equals("mixamorig:RightArm")
                    || child.name.Equals("mixamorig:LeftArm") || child.name.Equals("mixamorig:LeftForeArm") || child.name.Equals("mixamorig:RightForeArm"))
                {
                    aux.posX = child.localPosition.x;
                    aux.posY = child.localPosition.y;
                    aux.posZ = child.localPosition.z;
                }*/
                /*if(childsData[j].nm.Equals("mixamorig:LeftHand"))
                {
                    Debug.Log("Frame 0.2f: x="+ aux.x +", y="+ aux.y +", z="+ aux.z);
                }*/
                dummyChildsData[j].coordinates.Add(aux);
                j++;
            }
            yield return new WaitForSeconds(0.05f);
        }
        
        animationRecorded = true;
        Debug.Log("Animacion grabada sobre maniqui objetivo");
        //Debug.Log(timer);
        //Debug.Log("Lista length:" + childsData[20].coordinates.Count);
        /*
        Transform leg = clon.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg");
        Transform hips = clon.transform.Find("mixamorig:Hips");
        Transform legleg = clon.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg");
        Transform foot = clon.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");

        //Debug.Log(childsData[4].coordinates[0].z);
        //Debug.Log(clon.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg").name);
        
        Quaternion orRotation = new Quaternion(leg.rotation.x, leg.rotation.y, leg.rotation.z-0.1f, leg.rotation.w);
        Quaternion or2Rotation = new Quaternion(hips.rotation.x, hips.rotation.y, hips.rotation.z, hips.rotation.w);
        Quaternion newRotation = new Quaternion(childsData[4].coordinates[0].x, childsData[4].coordinates[0].y, childsData[4].coordinates[0].z-0.1f, childsData[4].coordinates[0].w);
        Quaternion new2Rotation = new Quaternion(childsData[3].coordinates[0].x, childsData[3].coordinates[0].y, childsData[3].coordinates[0].z, childsData[3].coordinates[0].w);
        Quaternion new3Rotation = new Quaternion(childsData[5].coordinates[0].x, childsData[5].coordinates[5].y, childsData[5].coordinates[0].z-0.1f, childsData[5].coordinates[0].w);
        Quaternion new4Rotation = new Quaternion(childsData[6].coordinates[0].x, childsData[6].coordinates[6].y, childsData[6].coordinates[0].z-0.1f, childsData[6].coordinates[0].w);
        leg.Rotate(newRotation.eulerAngles, Space.Self);
        hips.Rotate(new2Rotation.eulerAngles, Space.Self);
        legleg.Rotate(new3Rotation.eulerAngles, Space.Self);
        foot.Rotate(new4Rotation.eulerAngles, Space.Self);
        foreach (ChildCoordinates coords in childsData)
        {
            if (coords.nm.Equals("mixamorig:LeftUpLeg"))
            {
                foreach (Coordinates coord in coords.coordinates)
                {
                    coord.z = coord.z - 0.1f;
                }
            }
        }
        */
        //leg.Rotate(newRotation,Space.Self);
        //Debug.Log(newRotation.eulerAngles);
        //leg.rotation = Quaternion.Lerp(leg.rotation,newRotation,1);
        //leg.rotation = Quaternion.RotateTowards(leg.rotation, newRotation.normalized, Time.deltaTime);  
    }

    public void playCustomAnimation()
    {
        /*
        int i = 0;
        foreach(Transform child in childsClon)
        {
            Debug.Log("En objeto:  "+child.transform.name);
            Debug.Log("En array de datos:  "+childsData[i].nm);
            i++;
        }
        */
        StartCoroutine(playFrames());
    }

    IEnumerator playFrames()
    {
        /*
        int i = 0;
        for (timer = 0f; timer < 0.05f; timer += 0.05f)
        {
            int j = 0;
            foreach (Transform child in childsClon)
            {
                if (i == 0)
                {
                    Debug.Log("En objeto:  " + child.name);
                    Debug.Log("En array de datos:  " + childsData[j].nm);
                }
                Quaternion newRotation = new Quaternion(childsData[j].coordinates[i].x, childsData[j].coordinates[i].y, childsData[j].coordinates[i].z, childsData[j].coordinates[i].w);
                child.Rotate(newRotation.eulerAngles, Space.Self);
                child.rotation = newRotation;
                j++;
            }
            i++;
            yield return new WaitForSeconds(0.05f);
        }  
        */
        int i = 0;
        //Debug.Log("firstAnim.length en play: " + firstAnim.length);
        for (timer = 0f; timer < firstAnim.length; timer += 0.05f)
        {
            foreach (ChildCoordinates chCoord in cloneChildsData)
            {  
                Transform child = clone.transform.Find(chCoord.path);
                Quaternion newRotation = new Quaternion(chCoord.coordinates[i].rotX, chCoord.coordinates[i].rotY, chCoord.coordinates[i].rotZ, chCoord.coordinates[i].rotW);
                //Debug.Log(newRotation.eulerAngles);
                child.Rotate(newRotation.eulerAngles, Space.Self);
                child.localRotation = newRotation;
                if (chCoord.nm.Equals("mixamorig:Hips"))
                {
                    child.Translate(new Vector3(chCoord.coordinates[i].posX, chCoord.coordinates[i].posY, chCoord.coordinates[i].posZ), Space.Self);
                    child.localPosition = new Vector3(chCoord.coordinates[i].posX, chCoord.coordinates[i].posY, chCoord.coordinates[i].posZ);
                }
            }
            i++;
            yield return new WaitForSeconds(0.05f);
        }
        //Debug.Log("Veces: "+i);
    }

    public void saveAnimation()
    {
        animModified = new AnimationClip();
        foreach (ChildCoordinates chCoords in cloneChildsData)
        {
            //Debug.Log(chCoords.path);
            if (chCoords.nm.Equals("mixamorig:Hips")) {
                List<Keyframe> ksX = new List<Keyframe>();
                List<Keyframe> ksY = new List<Keyframe>();
                List<Keyframe> ksZ = new List<Keyframe>();
                List<Keyframe> ksW = new List<Keyframe>();
                List<Keyframe> ksPosX = new List<Keyframe>();
                List<Keyframe> ksPosY = new List<Keyframe>();
                List<Keyframe> ksPosZ = new List<Keyframe>();
                foreach (Coordinates chCoord in chCoords.coordinates)
                {

                    //Quaternion aux = new Quaternion(chCoord.rotX, chCoord.rotY, chCoord.rotZ, chCoord.rotW) * new Quaternion(chCoords.originalRot.rotX, chCoords.originalRot.rotY, chCoords.originalRot.rotZ, chCoords.originalRot.rotW);

                    ksX.Add(new Keyframe(chCoord.time, chCoord.rotX));
                    ksY.Add(new Keyframe(chCoord.time, chCoord.rotY));
                    ksZ.Add(new Keyframe(chCoord.time, chCoord.rotZ));
                    ksW.Add(new Keyframe(chCoord.time, chCoord.rotW));
                    ksPosX.Add(new Keyframe(chCoord.time, chCoord.posX));
                    ksPosY.Add(new Keyframe(chCoord.time, chCoord.posY));
                    ksPosZ.Add(new Keyframe(chCoord.time, chCoord.posZ));

                    //cambiar implementacion --> childcoordinates contiene listas de coordenadas, de forma que tengo todas las coordenadas de la animacion de un objeto en la misma clase 
                    //ya esta cambiado
                }
                AnimationCurve curveX = new AnimationCurve(ksX.ToArray());
                AnimationCurve curveY = new AnimationCurve(ksY.ToArray());
                AnimationCurve curveZ = new AnimationCurve(ksZ.ToArray());
                AnimationCurve curveW = new AnimationCurve(ksW.ToArray());
                AnimationCurve curvePosX = new AnimationCurve(ksPosX.ToArray());
                AnimationCurve curvePosY = new AnimationCurve(ksPosY.ToArray());
                AnimationCurve curvePosZ = new AnimationCurve(ksPosZ.ToArray());
                //Debug.Log(chCoords.path + "," + chCoords.nm);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.x", curveX);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.y", curveY);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.z", curveZ);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.w", curveW);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localPosition.x", curvePosX);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localPosition.y", curvePosY);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localPosition.z", curvePosZ);
            }
            else
            {
                List<Keyframe> ksX = new List<Keyframe>();
                List<Keyframe> ksY = new List<Keyframe>();
                List<Keyframe> ksZ = new List<Keyframe>();
                List<Keyframe> ksW = new List<Keyframe>();
                foreach (Coordinates chCoord in chCoords.coordinates)
                {
                    //Quaternion aux = new Quaternion(chCoord.rotX, chCoord.rotY, chCoord.rotZ, chCoord.rotW) * new Quaternion(chCoords.originalRot.rotX, chCoords.originalRot.rotY, chCoords.originalRot.rotZ, chCoords.originalRot.rotW);

                    ksX.Add(new Keyframe(chCoord.time, chCoord.rotX));
                    ksY.Add(new Keyframe(chCoord.time, chCoord.rotY));
                    ksZ.Add(new Keyframe(chCoord.time, chCoord.rotZ));
                    ksW.Add(new Keyframe(chCoord.time, chCoord.rotW));

                    //cambiar implementacion --> childcoordinates contiene listas de coordenadas, de forma que tengo todas las coordenadas de la animacion de un objeto en la misma clase 
                    //ya esta cambiado
                }
                AnimationCurve curveX = new AnimationCurve(ksX.ToArray());
                AnimationCurve curveY = new AnimationCurve(ksY.ToArray());
                AnimationCurve curveZ = new AnimationCurve(ksZ.ToArray());
                AnimationCurve curveW = new AnimationCurve(ksW.ToArray());
                //Debug.Log(chCoords.path + "," + chCoords.nm);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.x", curveX);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.y", curveY);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.z", curveZ);
                animModified.SetCurve(chCoords.path, typeof(Transform), "localRotation.w", curveW);
            } 
        }
        animModified.name = "modified";
        animModified.legacy = true;
        animation2.AddClip(animModified, animModified.name);
        animation2.clip = animModified;
        //AssetDatabase.CreateAsset(animation2, "3DModels/animModified.anim");
    }

    public void adjustDataToClone()
    {
        bool firstCopied = false;
        foreach (ChildCoordinates coords in dummyChildsData)
        {
            foreach (ChildCoordinates coordsPath in cloneChildsPaths)
            {
                ChildCoordinates aux;
                if(!firstCopied)
                {
                    aux = new ChildCoordinates(coords);
                    aux.path = coordsPath.path;
                    cloneChildsData.Add(aux);
                    firstCopied = true;
                }
                if (coords.nm.Equals(coordsPath.nm))
                {
                    aux = new ChildCoordinates(coords);
                    aux.path = coordsPath.path;
                    cloneChildsData.Add(aux);
                }
            }
        }
        //Debug.Log(cloneChildsData.Count);
        //getCloneOriginalRotations(cloneChilds, cloneChildsData);
        auxDataCopy = cloneChildsData.ConvertAll(x => new ChildCoordinates(x));
    }

    public void createColliders(GameObject dummy, Mesh meshSurface)
    {
        vertex = meshSurface.vertices;
        for (int i = 0; i < meshSurface.vertices.Length; i++)
        {
            GameObject child;
            Transform parent;
            if ((i >= 7994 && i < 8474) || (i >= 15193 && i < 15488)) //RIGHT FOOT
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");
                child = Instantiate(vertexPoint, Vector3.Scale(vertex[i], parent.localScale) + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if ((i >= 3138 && i < 3618) || (i >= 14769 && i < 14900) || (i >= 15489 && i < 15784)) //LEFT FOOT
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if (i >= 8914 && i < 9623) //RIGHT UPPER LEG
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if (i >= 2426 && i < 3135) //LEFT UPPER LEG
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            //RIGHT HAND
            if ((i >= 7100 && i < 7230) || (i >= 12133 && i < 12330)) //HAND PALM
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if (i >= 11013 && i < 11185) //MIDDLE
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandMiddle1/mixamorig:RightHandMiddle2/mixamorig:RightHandMiddle3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if (i >= 9893 && i < 10065) //PINKY
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandPinky1/mixamorig:RightHandPinky2/mixamorig:RightHandPinky3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }

            if (i >= 7230 && i < 7427) //THUMB
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandThumb1/mixamorig:RightHandThumb2/mixamorig:RightHandThumb3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            //LEFT HAND
            if ((i >= 5221 && i < 5350) || (i >= 14571 && i < 14767)) //HAND PALM
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if (i >= 13451 && i < 13623) //MIDDLE
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandMiddle1/mixamorig:LeftHandMiddle2/mixamorig:LeftHandMiddle3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if (i >= 12331 && i < 12503) //PINKY
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandPinky1/mixamorig:LeftHandPinky2/mixamorig:LeftHandPinky3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            if (i >= 5350 && i < 5548) //THUMB
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandThumb1/mixamorig:LeftHandThumb2/mixamorig:LeftHandThumb3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                collidersList.Add(child);
            }
            //HEAD
            if (i >= 1911 && i < 2426) //HEAD
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                //child.transform.position = new Vector3(child.transform.position.x * parent.localScale.x, child.transform.position.y * parent.localScale.y, child.transform.position.z * parent.localScale.z);
                collidersList.Add(child);
            }
        }
    }
    /* trying to fix T-pose original offset
    public void getCloneOriginalRotations(Transform[] childs, List<ChildCoordinates> childCoordinatesList)
    {
        foreach(Transform child in childs)
        {
            foreach(ChildCoordinates chCoord in childCoordinatesList)
            {
                if (child.name.Equals(chCoord.nm))
                {
                    chCoord.originalRot.rotX = child.localRotation.x;
                    chCoord.originalRot.rotY = child.localRotation.y;
                    chCoord.originalRot.rotZ = child.localRotation.z;
                    chCoord.originalRot.rotW = child.localRotation.w;
                }
            }
        }
    }
    */
}
