using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelloader : MonoBehaviour
{
    public Animator transition; 
    public float transitiontime = 1f;
    //public GameObject Lode_level;
    // Update is called once per frame
    void Start()
    {
            
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelindex){
        //Lode_level = GameObject.FindWithTag("Cross_Fade");
        transition = GetComponent<Animator>();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitiontime);
        SceneManager.LoadScene(levelindex);
    }

    public void MethodA(){
        Debug.Log("este es el metodo a");
        SceneManager.LoadScene(6);
    }
}
