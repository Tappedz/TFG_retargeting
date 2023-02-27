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

    public float xLimit = 45f;
    public float yLimit = 45f;
    public float zLimit = 45f;

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
            saveAnimation();
        }
    }

    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            captureAnimation();
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
                //Debug.Log("Nombres: " + aux.nm);
                j++;
            }
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("fuera");
        Debug.Log(timer);
        Debug.Log("Lista length:" + childsData[5].coordinates.Count);
        //maniqui.SetActive(false);
        //clon.SetActive(true);
        /*for (int i=0;i<childsData[5].coordinates.Count;i++)
        {
            Debug.Log("Datos: "+ childsData[5].coordinates[i].ToString());
        }*/
        
        Transform leg = clon.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg");
        //Debug.Log(childsData[4].coordinates[0].z);
        //Debug.Log(clon.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg").name);
        Quaternion orRotation = new Quaternion(leg.rotation.x, leg.rotation.y, leg.rotation.z-0.1f, leg.rotation.w);
        Quaternion newRotation = new Quaternion(childsData[4].coordinates[0].x, childsData[4].coordinates[0].y, childsData[4].coordinates[0].z, childsData[4].coordinates[0].w);
        leg.Rotate(orRotation.eulerAngles, Space.Self);
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
        //leg.Rotate(newRotation,Space.Self);
        //Debug.Log(newRotation.eulerAngles);
        //leg.rotation = Quaternion.Lerp(leg.rotation,newRotation,1);
        //leg.rotation = Quaternion.RotateTowards(leg.rotation, newRotation.normalized, Time.deltaTime);
    }

    public void playCustomAnimation()
    {
        timer = 0f;
        float rotateSpeed = 20f;
        for (int i = 0; i < childsData.Count; i++)
        {
            for (int j = 0; j < childsData[i].coordinates.Count; j++)
            {
                Quaternion aux = childsClon[j].rotation;
                Quaternion target = Change(childsData[i].coordinates[j].x, childsData[i].coordinates[j].y, childsData[i].coordinates[j].z);
                childsClon[j].rotation = Quaternion.RotateTowards(aux, target, rotateSpeed*Time.deltaTime);
                timer += Time.deltaTime * 0.05f;
                //aux.time = timer;
                //childsData.Add(aux);
            }
        }
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
        animation2.Play();
    }
}
