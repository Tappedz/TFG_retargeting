using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    Animation animation1; 
    Animation animation2;
    float timer = 0f;
    public AnimationClip firstAnim;
    public GameObject mainMenu;
    public GameObject slidersMenu;
    public GameObject armsMenu;
    public GameObject legsMenu;

    public GameObject maniqui;
    public GameObject clon;
    public Transform[] childsManiqui;
    public Transform[] childsClon;
    public Transform transformInUse;
    AnimationClip animModified;
    
    List<ChildCoordinates> childsData = new List<ChildCoordinates>();

    public Slider xRotationSlider;
    public Slider yRotationSlider;
    public Slider zRotationSlider;

    public Slider legsSlider;
    public Slider armsSlider;
    public Button button;

    public float xLimit = 45f;
    public float yLimit = 45f;
    public float zLimit = 45f;
    Boolean animationRecorded = false;

    public void Start()
    {
        childsManiqui = maniqui.GetComponentsInChildren<Transform>();
        childsClon = clon.GetComponentsInChildren<Transform>();
        animation1 = maniqui.GetComponent<Animation>();
        animation2 = clon.GetComponent<Animation>();
        animation2.enabled = false;
        firstAnim.name = "mixamo";
        firstAnim.legacy = true;
        animation1.AddClip(firstAnim, firstAnim.name);
        animation1.clip = firstAnim;
        timer = 0f;

        legsSlider.value = 0;
        legsSlider.minValue = -0.1f;
        legsSlider.maxValue = 0.1f;

        armsSlider.value = 0;
        armsSlider.minValue = -0.1f;
        armsSlider.maxValue = 0.1f;

        button.onClick.AddListener(delegate{
            foreach (ChildCoordinates coords in childsData)
            {
                if (coords.nm.Equals("mixamorig:LeftUpLeg"))
                {
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        /*
                        Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - legsSlider.value*100);
                        Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        coord.x = changeQuat.x;
                        coord.y = changeQuat.y;
                        coord.z = changeQuat.z;
                        coord.w = changeQuat.w;
                        */

                        coord.z = coord.z - legsSlider.value;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightUpLeg"))
                {
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + legsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.z = coord.z + legsSlider.value;
                    }
                }
                if (coords.nm.Equals("mixamorig:LeftForeArm"))
                {
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.z = coord.z - armsSlider.value;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightForeArm"))
                {
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.z = coord.z + armsSlider.value;
                    }
                }
                if (coords.nm.Equals("mixamorig:LeftArm"))
                {
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z - armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.z = coord.z - armsSlider.value;
                    }
                }
                if (coords.nm.Equals("mixamorig:RightArm"))
                {
                    foreach (Coordinates coord in coords.coordinates)
                    {
                        //Quaternion auxQuat = new Quaternion(coord.x, coord.y, coord.z, coord.w);
                        //Vector3 eulerAng = new Vector3(auxQuat.eulerAngles.x, auxQuat.eulerAngles.y, auxQuat.eulerAngles.z + armsSlider.value * 100);
                        //Quaternion changeQuat = Quaternion.Euler(eulerAng);
                        //coord.x = changeQuat.x;
                        //coord.y = changeQuat.y;
                        //coord.z = changeQuat.z;
                        //coord.w = changeQuat.w;

                        coord.z = coord.z + armsSlider.value;
                    }
                }
            }
            saveAnimation();
        });
        //clon.SetActive(false);
        //captureAnimation();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("animacion en clon");
            playCustomAnimation();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("animacion en clon");
            animation2.Play();
            
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

    //Metodos para activar el menu para la zona del cuerpo seleccionada
    public void displayHeadMenu()
    {
        mainMenu.SetActive(false);
        slidersMenu.SetActive(true);
    }

    public void displayArmsMenu()
    {
        mainMenu.SetActive(false);
        armsMenu.SetActive(true);
    }

    public void displayLegsMenu()
    {
        mainMenu.SetActive(false);
        legsMenu.SetActive(true);
    }


    //Metodos para mostrar los sliders de la zona escogida
    //Brazos
    //Brazo derecho
    public void displaySlidersRightArm()
    {
        armsMenu.SetActive(false);
        transformInUse = childsManiqui[47];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }

    //Antebrazo derecho
    public void displaySlidersRightForearm()
    {
        armsMenu.SetActive(false);

        transformInUse = childsManiqui[48];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }

    //Brazo izquierdo
    public void displaySlidersLeftArm()
    {
        armsMenu.SetActive(false);

        transformInUse = childsManiqui[18];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }

    //Antebrazo izquierdo
    public void displaySlidersLeftForearm()
    {
        armsMenu.SetActive(false);

        transformInUse = childsManiqui[19];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }

    //Piernas
    //Pierna derecha
    public void displaySlidersRightLeg()
    {
        legsMenu.SetActive(false);

        transformInUse = childsManiqui[9];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }

    //Gemelo derecho
    public void displaySlidersRightCalf()
    {
        legsMenu.SetActive(false);

        transformInUse = childsManiqui[10];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }

    //Pierna izquierda
    public void displaySlidersLeftLeg()
    {
        legsMenu.SetActive(false);

        transformInUse = childsManiqui[4];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }

    //Gemelo izquierdo
    public void displaySlidersLeftCalf()
    {
        legsMenu.SetActive(false);

        transformInUse = childsManiqui[5];
        setSliders(transformInUse);
        slidersMenu.SetActive(true);
    }


    //Preparamos los sliders
    public void setSliders(Transform bodyPartTr)
    {
        //Slider xRotationSlider = slidersMenu.transform.Find("XRotation").GetComponent<Slider>();
        //Slider yRotationSlider = slidersMenu.transform.Find("YRotation").GetComponent<Slider>();
        //Slider zRotationSlider = slidersMenu.transform.Find("ZRotation").GetComponent<Slider>();
        //xRotation
        xRotationSlider.value = bodyPartTr.transform.rotation.x;
        xRotationSlider.minValue = bodyPartTr.transform.rotation.x - xLimit; ;
        xRotationSlider.maxValue = bodyPartTr.transform.rotation.x + xLimit; ;
        //yRotation
        yRotationSlider.value = bodyPartTr.transform.rotation.y;
        yRotationSlider.minValue = bodyPartTr.transform.rotation.y - yLimit; ;
        yRotationSlider.maxValue = bodyPartTr.transform.rotation.y + yLimit; ;
        //zRotation
        zRotationSlider.value = bodyPartTr.transform.rotation.z;
        zRotationSlider.minValue = bodyPartTr.transform.rotation.z - zLimit;
        zRotationSlider.maxValue = bodyPartTr.transform.rotation.z + zLimit;

        xRotationSlider.onValueChanged.AddListener(delegate
        {
            setRotation(bodyPartTr, 1);
        });
        yRotationSlider.onValueChanged.AddListener(delegate
        {
            setRotation(bodyPartTr, 2);
        });
        zRotationSlider.onValueChanged.AddListener(delegate
        {
            setRotation(bodyPartTr, 3);
        });
    }

    //Manejamos la rotacion
    public void setRotation(Transform tr, int axis)
    {
        if(axis == 1) //EJE X
        {
            Quaternion initRotation = tr.transform.rotation;
            Quaternion newRotation = new Quaternion(xRotationSlider.value, tr.rotation.y,  tr.rotation.z, 1);
            tr.rotation = Quaternion.Slerp(initRotation, newRotation, 1);
        }
        else if(axis == 2) //EJE Y
        {
            Debug.Log(yRotationSlider.value);
            tr.localEulerAngles = new Vector3(tr.rotation.x, yRotationSlider.value, tr.rotation.z);
        }
        else if(axis == 3) //EJE Z
        {
            tr.localEulerAngles = new Vector3(tr.rotation.x, tr.rotation.y, zRotationSlider.value);
        }
        else
        {
            Debug.Log("Eje introducido incorrecto");
        }
    }

    public void resetSliders()
    {
        xRotationSlider.onValueChanged.RemoveAllListeners();
        yRotationSlider.onValueChanged.RemoveAllListeners();
        zRotationSlider.onValueChanged.RemoveAllListeners();
    }

    public void pauseAnimation()
    {
        animation1.Stop();
        setSliders(transformInUse);
        //leftArm.transform.rotation = Quaternion.Euler(0, 5, 0);
    }

    public void playAnimation()
    {
        animation1.Play();
    }

    public void captureAnimation()
    {
        
        foreach (Transform child in childsClon)
        {
            //coger el path de cada hijo y guardarlo en parentNm ****
            ChildCoordinates chCoord = new ChildCoordinates();
            chCoord.nm = child.name;
            //Debug.Log(chCoord.nm);
            try {
                if (chCoord.nm.Equals(childsClon[0].name))
                {
                    chCoord.path = "/"+childsClon[0].name;
                }
                else
                {
                    chCoord.path = "/" + getPath(child);
                    //Debug.Log(chCoord.parentNm);
                }
            }
            catch (NullReferenceException ex) {
                Debug.Log("ERROR");
            }
            childsData.Add(chCoord);
        }
        playAnimation();
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

    IEnumerator captureFrames()
    {
        //meter Time.deltaTime (referencia al tiempo real de ejecucion) --> ya esta con coroutine
        //Debug.Log("firstAnim.length en capture: "+firstAnim.length);
        for (timer = 0f; timer < firstAnim.length; timer += 0.05f)
        {
            int j = 0;
            foreach (Transform child in childsManiqui)
            {
                Coordinates aux = new Coordinates();
                aux.x = child.localRotation.x;
                aux.y = child.localRotation.y;
                aux.z = child.localRotation.z;
                aux.w = child.localRotation.w;
                aux.time = timer;
                /*if(childsData[j].nm.Equals("mixamorig:LeftHand"))
                {
                    Debug.Log("Frame 0.2f: x="+ aux.x +", y="+ aux.y +", z="+ aux.z);
                }*/
                childsData[j].coordinates.Add(aux);
                j++;
            }
            yield return new WaitForSeconds(0.05f);
        }
        saveAnimation();
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
            foreach (ChildCoordinates chCoord in childsData)
            {  
                Transform child = clon.transform.Find(chCoord.path);
                Quaternion newRotation = new Quaternion(chCoord.coordinates[i].x, chCoord.coordinates[i].y, chCoord.coordinates[i].z, chCoord.coordinates[i].w);
                //Debug.Log(newRotation.eulerAngles);
                child.Rotate(newRotation.eulerAngles, Space.Self);
                child.localRotation = newRotation;
            }
            i++;
            yield return new WaitForSeconds(0.05f);
        }
        //Debug.Log("Veces: "+i);
    }

    private static Quaternion Change(float x, float y, float z)
    {
        Quaternion newQuaternion = Quaternion.Euler(x,y,z);
        //Return the new Quaternion
        return newQuaternion;
    }

    public void saveAnimation()
    {
        animModified = new AnimationClip();
        foreach (ChildCoordinates chCoords in childsData)
        {
            List<Keyframe> ksX = new List<Keyframe>();
            List<Keyframe> ksY = new List<Keyframe>();
            List<Keyframe> ksZ = new List<Keyframe>();
            List<Keyframe> ksW = new List<Keyframe>();
            foreach (Coordinates chCoord in chCoords.coordinates)
            {
                if (chCoords.nm.Equals("mixamorig:LeftUpLeg"))
                {
                    ksX.Add(new Keyframe(chCoord.time, chCoord.x));
                    ksY.Add(new Keyframe(chCoord.time, chCoord.y));
                    ksZ.Add(new Keyframe(chCoord.time, chCoord.z));
                    ksW.Add(new Keyframe(chCoord.time, chCoord.w));
                }
                else if (chCoords.nm.Equals("mixamorig:RightUpLeg"))
                {
                    ksX.Add(new Keyframe(chCoord.time, chCoord.x));
                    ksY.Add(new Keyframe(chCoord.time, chCoord.y));
                    ksZ.Add(new Keyframe(chCoord.time, chCoord.z));
                    ksW.Add(new Keyframe(chCoord.time, chCoord.w));
                }
                else
                {
                    ksX.Add(new Keyframe(chCoord.time, chCoord.x));
                    ksY.Add(new Keyframe(chCoord.time, chCoord.y));
                    ksZ.Add(new Keyframe(chCoord.time, chCoord.z));
                    ksW.Add(new Keyframe(chCoord.time, chCoord.w));
                }
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
        animModified.name = "modified";
        animModified.legacy = true;
        animation2.enabled = true;
        animation2.AddClip(animModified, animModified.name);
        animation2.clip = animModified;
        //AssetDatabase.CreateAsset(animation2, "3DModels/animModified.anim");
    }
}
