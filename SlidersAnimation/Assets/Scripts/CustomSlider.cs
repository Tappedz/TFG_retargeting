using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public Slider slider;
    public float oldValue;
    public bool pointerDown;
    public bool pointerUp;

    void Start()
    {
        slider = GetComponent<Slider>();
        oldValue = slider.value;
        pointerUp = false;
        pointerDown = false;
}

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerUp = true;
        if (slider.value != oldValue && pointerDown)
        {  
            oldValue = slider.value;
            pointerDown = false;
        }
    }
}
