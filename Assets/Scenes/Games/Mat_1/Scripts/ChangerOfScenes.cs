using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangerOfScenes : MonoBehaviour
{    
    public void Accept()
    {
        //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex)+1);
        SceneManager.LoadScene("Juego 1");
        //SceneManager.Load
    }

    public void Denied()
    {
        Application.Quit();
    }
}
