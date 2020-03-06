using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene: MonoBehaviour
{
    // Update is called once per frame
    void Active()
    {
       Loadnextscene();
    }

    public void Loadnextscene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
