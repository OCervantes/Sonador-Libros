using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangerOfScenes : MonoBehaviour
{    
    public void Accept()
    {
        //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex)+1);
        Debug.Log("Pues no");
        //SceneManager.Load
    }

    public void Denied()
    {
        Application.Quit();
    }
}
