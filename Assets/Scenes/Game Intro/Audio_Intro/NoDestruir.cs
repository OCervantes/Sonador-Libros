using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/* 
 This program is useful when you don't want to destroy an object when loading a new scene.

*/
public class NoDestruir : MonoBehaviour
{
    void Update() {
        if(SceneManager.GetActiveScene().name == "GameSelector" || SceneManager.GetActiveScene().name == "MainMenu" ){
            Destroy(this.gameObject); //Destroys the object if the user is in one od this two scenes.
        }
    }
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music"); //Find all the objects with the tag "Music" and strores them in an array.
        if(objs.Length >1){ //If there are more than 1 object
            Destroy(this.gameObject); //Destroy it
        }
        DontDestroyOnLoad(this.gameObject);//If there is only one the don't destroy it when loading a new scene. 
    }
}
