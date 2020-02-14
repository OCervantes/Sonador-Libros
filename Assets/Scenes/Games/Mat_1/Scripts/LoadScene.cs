using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene(1);
    }
}
