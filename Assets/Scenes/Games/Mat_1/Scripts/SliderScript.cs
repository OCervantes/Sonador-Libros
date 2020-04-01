using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;
    /*int originalValue;
    
    void Start()
    {
        originalValue = (int)slider.value;
        Debug.Log("Original value: " + originalValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (originalValue != (int)slider.value)
        {
            printValue();
        }        
    }*/

    public void printValue(float someNumber)
    {
        Debug.Log("New value: " + slider.value);
        Debug.Log("What's this? " + someNumber);
        //originalValue = slider.value;
    }
}
