using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelloWorld : MonoBehaviour
{
    public Text text;

    public void SayHello()
    {
        text.text = "¡Hola mundo!";        
    }  
}
