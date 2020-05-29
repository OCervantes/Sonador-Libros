using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NoDestruir : MonoBehaviour
{
    void Update() {
        if(SceneManager.GetActiveScene().name == "GameSelector"){
            Destroy(this.gameObject);
        }
    }
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if(objs.Length >1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
