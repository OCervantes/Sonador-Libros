//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderNumber : MonoBehaviour
{
    //public GameObject DialogAndAudioMan;
    //public static int numberOfFruits;
    Text UIText;

    void Start() 
    {
        UIText = GetComponent<Text>();            
        //Debug.Log("Number of Fruits: " + numberOfFruits);
    }

    public void printValue(float f)
    {    
        UIText.text = f.ToString();
        Monologue.sliderValue = (int)f;
    }    
}
