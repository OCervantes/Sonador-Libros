using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangerOfScenes : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void Accept()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex)+1);
        //SceneManager.Load
    }

    public void Denied()
    {
        Application.Quit();
    }
}
