using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangetoRetos : MonoBehaviour
{
void Active()
    {
       Loadnextscene();
    }

    public void Loadnextscene()
    {

        SceneManager.LoadScene("GameIntro_Intro1");
    }
}
