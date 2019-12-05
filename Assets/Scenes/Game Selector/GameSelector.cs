using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelector : MonoBehaviour
{

    public void Select(string name) {
    	SceneManager.LoadScene(name);
    }

    public void Select(int index) {
    	SceneManager.LoadScene(index);
    }

}
