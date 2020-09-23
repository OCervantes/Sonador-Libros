﻿using System;
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
    void Update()
    {
        /* if(Input.GetMouseButtonDown(0)){
             LoadNextLevel();
         } */
    }

    public void LoadNextLevel(bool continuarpresionado)
    {
        if (continuarpresionado == true && SceneManager.GetActiveScene().name == "7Game_Intro")
            SceneManager.LoadScene("GameSelector");        
        else if(continuarpresionado == true)
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));        
        else
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));        
    }

    IEnumerator LoadLevel(int levelindex){
        //Lode_level = GameObject.FindWithTag("Cross_Fade");
        //transition = GetComponent<Animator>();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitiontime);
        SceneManager.LoadScene(levelindex);
    }

    public void MethodA(){
        Debug.Log("este es el metodo a");
        SceneManager.LoadScene(6);
    }
}
