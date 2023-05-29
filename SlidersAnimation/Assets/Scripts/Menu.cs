using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    float timer = 0f;
    int frames = 0;

    public CameraMovement camera;

    Vector3[] vertex;
    List<GameObject> collidersList = new List<GameObject>();
    public GameObject vertexPoint;
    public Material good;
    public Material bad;

    public GameObject dummyPrefab;

    GameObject dummy;
    GameObject clone;
    public Transform[] dummyChilds;
    public Transform[] cloneChilds;
    Animation animation1;
    Animation animation2;

    public AnimationClip originalAnim1;
    public AnimationClip originalAnim2;

    AnimationClip firstAnim;
    AnimationClip animModified;

    List<ChildCoordinates> dummyChildsData = new List<ChildCoordinates>();
    List<ChildCoordinates> cloneChildsData = new List<ChildCoordinates>();
    List<ChildCoordinates> cloneChildsPaths = new List<ChildCoordinates>(); // placeholder of paths, can't iterate either way
    List<ChildCoordinates> auxDataCopy; // used to change value on sliders without losing original values


    Slider legsSlider;
    Slider armsSlider;
    Slider runByFrames;

    Toggle ch1Toggle;
    Toggle ch2Toggle;
    Toggle ch3Toggle;

    Toggle anim1Toggle;
    Toggle anim2Toggle;

    Button runByButton;
    Button resetColorButton;

    public static List<Transform> collidedObjects = new List<Transform>();

    bool animationRecorded;
    bool animationPlayed;

    public void Start()
    {
        dummy = GameObject.FindGameObjectWithTag("originalDummy");

        legsSlider = GameObject.FindGameObjectWithTag("legsSlider").GetComponent<Slider>();
        armsSlider = GameObject.FindGameObjectWithTag("armsSlider").GetComponent<Slider>();
        runByFrames = GameObject.FindGameObjectWithTag("sliderByFrames").GetComponent<Slider>();

        ch1Toggle = GameObject.FindGameObjectWithTag("ch1Toggle").GetComponent<Toggle>();
        ch2Toggle = GameObject.FindGameObjectWithTag("ch2Toggle").GetComponent<Toggle>();
        ch3Toggle = GameObject.FindGameObjectWithTag("ch3Toggle").GetComponent<Toggle>();

        anim1Toggle = GameObject.FindGameObjectWithTag("anim1Toggle").GetComponent<Toggle>();
        anim2Toggle = GameObject.FindGameObjectWithTag("anim2Toggle").GetComponent<Toggle>();

        runByButton = GameObject.FindGameObjectWithTag("runByButton").GetComponent<Button>();
        resetColorButton = GameObject.FindGameObjectWithTag("resetColorButton").GetComponent<Button>();
        dummyChilds = dummy.GetComponentsInChildren<Transform>();
        getLimbsPaths(dummyChilds, dummyChildsData);
        animation1 = dummy.GetComponent<Animation>();

        animationRecorded = false;
        animationPlayed = false;

        ch1Toggle.isOn = false;
        ch2Toggle.isOn = false;
        ch3Toggle.isOn = false;

        if (SceneManager.GetActiveScene().name.Equals("MainAnim1"))
        {
            anim1Toggle.isOn = true;
            anim2Toggle.isOn = false;
            firstAnim = originalAnim1;
            firstAnim.name = originalAnim1.name;
        }
        else if (SceneManager.GetActiveScene().name.Equals("MainAnim2"))
        {
            anim1Toggle.isOn = false;
            anim2Toggle.isOn = true;
            firstAnim = originalAnim2;
            firstAnim.name = originalAnim2.name;
        }
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
        armsSlider.minValue = -0.3f;
        armsSlider.maxValue = 0.3f;

        runByFrames.value = 0;
        runByFrames.minValue = 0;
        runByFrames.wholeNumbers = true;

        anim1Toggle.onValueChanged.AddListener(delegate
        {
            SceneManager.LoadScene("MainAnim1");
        });
        anim2Toggle.onValueChanged.AddListener(delegate
        {
            SceneManager.LoadScene("MainAnim2");
        });

        ch1Toggle.onValueChanged.AddListener(delegate
        {
            if (ch1Toggle.isOn)
            {
                ch2Toggle.isOn = false;
                ch3Toggle.isOn = false;

                legsSlider.value = 0;
                armsSlider.value = 0;

                cloneChildsPaths.Clear();
                cloneChildsData.Clear();
                collidersList.Clear();
                collidedObjects.Clear();
                GameObject.Destroy(clone);

                clone = Instantiate(dummyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cloneChilds = clone.GetComponentsInChildren<Transform>();
                getLimbsPaths(cloneChilds, cloneChildsPaths);
                adjustDataToClone();
                createColliders(clone, clone.transform.Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>().sharedMesh);
                Transform head = clone.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head");
                head.localScale = new Vector3(3, 3, 3);

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

                legsSlider.value = 0;
                armsSlider.value = 0;

                cloneChildsPaths.Clear();
                cloneChildsData.Clear();
                collidersList.Clear();
                collidedObjects.Clear();
                GameObject.Destroy(clone);

                clone = Instantiate(dummyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cloneChilds = clone.GetComponentsInChildren<Transform>();
                getLimbsPaths(cloneChilds, cloneChildsPaths);
                adjustDataToClone();
                createColliders(clone, clone.transform.Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>().sharedMesh);
                Transform leftSh = clone.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder");
                Transform rightSh = clone.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder");
                leftSh.localScale = new Vector3(2, 2, 2);
                rightSh.localScale = new Vector3(2, 2, 2);

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

                legsSlider.value = 0;
                armsSlider.value = 0;

                cloneChildsPaths.Clear();
                cloneChildsData.Clear();
                collidersList.Clear();
                collidedObjects.Clear();
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
                        Quaternion auxQuat = new Quaternion(auxDataCopy[dataCount].coordinates[coordCount].rotX, auxDataCopy[dataCount].coordinates[coordCount].rotY,
                         auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value, auxDataCopy[dataCount].coordinates[coordCount].rotW);
                        Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - legsSlider.value*100);
                        Quaternion changeQuat = auxQuat * Quaternion.Euler(-legsSlider.value * 100, 0f, 0f);
                        coord.rotX = auxQuat.x;
                        coord.rotY = auxQuat.y;
                        coord.rotZ = auxQuat.z;
                        coord.rotW = auxQuat.w;
                        coordCount++;
                        */
                        //coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value * 1.5f;
                        coord.rotX = auxDataCopy[dataCount].coordinates[coordCount].rotX - legsSlider.value;
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
                        //coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - legsSlider.value * 1.5F;
                        coord.rotX = auxDataCopy[dataCount].coordinates[coordCount].rotX + legsSlider.value;
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

                        //coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ + armsSlider.value;
                        //coord.rotY = auxDataCopy[dataCount].coordinates[coordCount].rotY + armsSlider.value;
                        coord.rotX = auxDataCopy[dataCount].coordinates[coordCount].rotX - armsSlider.value;

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

                        //coord.rotZ = auxDataCopy[dataCount].coordinates[coordCount].rotZ - armsSlider.value;
                        //coord.rotY = auxDataCopy[dataCount].coordinates[coordCount].rotY - armsSlider.value;
                        coord.rotX = auxDataCopy[dataCount].coordinates[coordCount].rotX - armsSlider.value;

                        coordCount++;
                    }
                }
                dataCount++;
            }
            saveAnimation();
        });
        runByFrames.onValueChanged.AddListener(delegate
        {
            foreach (ChildCoordinates chCoord in cloneChildsData)
            {
                Transform child = clone.transform.Find(chCoord.path);
                Quaternion newRotation = 
                    new Quaternion(chCoord.coordinates[(int)runByFrames.value].rotX, chCoord.coordinates[(int)runByFrames.value].rotY, chCoord.coordinates[(int)runByFrames.value].rotZ, chCoord.coordinates[(int)runByFrames.value].rotW);
                //Debug.Log(newRotation.eulerAngles);
                child.Rotate(newRotation.eulerAngles, Space.Self);
                child.localRotation = newRotation;
                if (chCoord.nm.Equals("mixamorig:Hips"))
                {
                    child.Translate(new Vector3(chCoord.coordinates[(int)runByFrames.value].posX, chCoord.coordinates[(int)runByFrames.value].posY, chCoord.coordinates[(int)runByFrames.value].posZ), Space.Self);
                    child.localPosition = new Vector3(chCoord.coordinates[(int)runByFrames.value].posX, chCoord.coordinates[(int)runByFrames.value].posY, chCoord.coordinates[(int)runByFrames.value].posZ);
                }
            }
        });
        runByButton.onClick.AddListener(delegate
        {
            playCustomAnimation((int)runByFrames.value);
        });
        resetColorButton.onClick.AddListener(delegate
        {
            foreach (Transform tr in collidedObjects)
            {
                changeColor(tr, good);
            }
        });
    }

    public void Update()
    {
        foreach (GameObject vertexPoint in collidersList)
        {
            vertexPoint.transform.rotation = vertexPoint.transform.parent.rotation;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Animation playing on clone");
            playCustomAnimation(frames);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Animation playing on clone");
            if (ch1Toggle.isOn || ch2Toggle.isOn || ch3Toggle.isOn)
            {
                animation2.Play();
                animationPlayed = true;
            }
        }
        if (!animationRecorded || !camera.camLocked)
        {
            legsSlider.interactable = false;
            armsSlider.interactable = false;
            runByFrames.interactable = false;
            ch1Toggle.interactable = false;
            ch2Toggle.interactable = false;
            ch3Toggle.interactable = false;
        }
        else
        {
            legsSlider.interactable = true;
            armsSlider.interactable = true;
            runByFrames.interactable = true;
            ch1Toggle.interactable = true;
            ch2Toggle.interactable = true;
            ch3Toggle.interactable = true;
        }
        if (animationPlayed && !animation2.isPlaying)
        {
            foreach (Transform tr in collidedObjects)
            {
                Debug.Log("Collision in " + tr.name);
                //changeColor(tr, bad);
            }
            animationPlayed = false;
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

    void playAnimation()
    {
        animation1.Play();
    }

    void captureAnimation()
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

    void getLimbsPaths(Transform[] childs, List<ChildCoordinates> childCoordinatesList)
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

        for (timer = 0f; timer < firstAnim.length; timer += 0.02f)
        {
            int j = 0;
            if (timer != 0)
            {
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
                    dummyChildsData[j].coordinates.Add(aux);
                    j++;
                }
                frames++;
            }
            yield return new WaitForSeconds(0.02f);
        }
        runByFrames.maxValue = frames - 1;
        animationRecorded = true;
        Debug.Log("Animation recorded");
    }

    void playCustomAnimation(int fr)
    {
        StartCoroutine(playFrames(fr));
    }

    IEnumerator playFrames(int fr)
    {
        for (int i = 0; i < fr; i++)
        {
            foreach (ChildCoordinates chCoord in cloneChildsData)
            {  
                Transform child = clone.transform.Find(chCoord.path);
                Quaternion newRotation = new Quaternion(chCoord.coordinates[i].rotX, chCoord.coordinates[i].rotY,
                 chCoord.coordinates[i].rotZ, chCoord.coordinates[i].rotW);

                child.Rotate(newRotation.eulerAngles, Space.Self);
                child.localRotation = newRotation;
                if (chCoord.nm.Equals("mixamorig:Hips"))
                {
                    child.Translate(new Vector3(chCoord.coordinates[i].posX, chCoord.coordinates[i].posY,
                     chCoord.coordinates[i].posZ), Space.Self);
                    child.localPosition = new Vector3(chCoord.coordinates[i].posX, chCoord.coordinates[i].posY,
                     chCoord.coordinates[i].posZ);
                }
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    void saveAnimation()
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
                    ksX.Add(new Keyframe(chCoord.time, chCoord.rotX));
                    ksY.Add(new Keyframe(chCoord.time, chCoord.rotY));
                    ksZ.Add(new Keyframe(chCoord.time, chCoord.rotZ));
                    ksW.Add(new Keyframe(chCoord.time, chCoord.rotW));
                    ksPosX.Add(new Keyframe(chCoord.time, chCoord.posX));
                    ksPosY.Add(new Keyframe(chCoord.time, chCoord.posY));
                    ksPosZ.Add(new Keyframe(chCoord.time, chCoord.posZ));

                }
                AnimationCurve curveX = new AnimationCurve(ksX.ToArray());
                AnimationCurve curveY = new AnimationCurve(ksY.ToArray());
                AnimationCurve curveZ = new AnimationCurve(ksZ.ToArray());
                AnimationCurve curveW = new AnimationCurve(ksW.ToArray());
                AnimationCurve curvePosX = new AnimationCurve(ksPosX.ToArray());
                AnimationCurve curvePosY = new AnimationCurve(ksPosY.ToArray());
                AnimationCurve curvePosZ = new AnimationCurve(ksPosZ.ToArray());

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

    void adjustDataToClone()
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

    //parent localScale moves object to parent's local position
    void createColliders(GameObject dummy, Mesh meshSurface)
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
                child.tag = "RightFoot";

                collidersList.Add(child);
            }
            if ((i >= 3138 && i < 3618) || (i >= 14769 && i < 14900) || (i >= 15489 && i < 15784)) //LEFT FOOT
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "LeftFoot";

                collidersList.Add(child);
            }
            if (i >= 8914 && i < 9623) //RIGHT UPPER LEG
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "RightUpperLeg";

                collidersList.Add(child);
            }
            if (i >= 2426 && i < 3135) //LEFT UPPER LEG
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "LeftUpperLeg";

                collidersList.Add(child);
            }
            //RIGHT HAND
            if ((i >= 7100 && i < 7230) || (i >= 12133 && i < 12330)) //HAND PALM
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "RightHandPalm";

                collidersList.Add(child);
            }
            if (i >= 11013 && i < 11185) //MIDDLE
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandMiddle1/mixamorig:RightHandMiddle2/mixamorig:RightHandMiddle3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "RightHandMiddle";

                collidersList.Add(child);
            }
            if (i >= 9893 && i < 10065) //PINKY
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandPinky1/mixamorig:RightHandPinky2/mixamorig:RightHandPinky3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "RightHandPinky";

                collidersList.Add(child);
            }

            if (i >= 7230 && i < 7427) //THUMB
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandThumb1/mixamorig:RightHandThumb2/mixamorig:RightHandThumb3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "RightHandThumb";

                collidersList.Add(child);
            }
            //LEFT HAND
            if ((i >= 5221 && i < 5350) || (i >= 14571 && i < 14767)) //HAND PALM
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "LeftHandPalm";

                collidersList.Add(child);
            }
            if (i >= 13451 && i < 13623) //MIDDLE
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandMiddle1/mixamorig:LeftHandMiddle2/mixamorig:LeftHandMiddle3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "LeftHandMiddle";

                collidersList.Add(child);
            }
            if (i >= 12331 && i < 12503) //PINKY
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandPinky1/mixamorig:LeftHandPinky2/mixamorig:LeftHandPinky3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "LeftHandPinky";

                collidersList.Add(child);
            }
            if (i >= 5351 && i < 5548) //THUMB
            {
                parent = dummy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandThumb1/mixamorig:LeftHandThumb2/mixamorig:LeftHandThumb3");
                child = Instantiate(vertexPoint, vertex[i] + dummy.transform.position, Quaternion.identity);
                child.transform.parent = parent;
                child.transform.localPosition = Vector3.Scale(child.transform.localPosition, parent.localScale);
                child.tag = "LeftHandThumb";

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
                child.tag = "Head";

                collidersList.Add(child);
            }
        }
    }

    void changeColor(Transform parent, Material mat)
    {
        int childNum = parent.childCount;
        for (int i = 0; i < childNum; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name.Equals("Vertex Point(Clone)"))
            {
                MeshRenderer childMesh = child.GetComponent<MeshRenderer>();
                Material[] materials = childMesh.materials;
                materials[0] = mat;
                childMesh.materials = materials;
                childMesh.material = materials[0];
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
