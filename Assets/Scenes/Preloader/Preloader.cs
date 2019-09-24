using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour {
    
    private CanvasGroup fadeGroup;
    private float loadTime;
    // Minimum time of that scene
    private float minimumLogoTime = 3.0f;

    private void Start() {
        //Grab the only CanvasGroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();
        //Start with a white screen;
        fadeGroup.alpha = 1;

        //Get a timestamp of the completion time
        if (Time.time < minimumLogoTime)
            loadTime = minimumLogoTime;
        else
            loadTime = Time.time;
    }//end Start

    private void Update() {
        //Fade in Logo
        if (Time.time < minimumLogoTime) {
            fadeGroup.alpha = 1 - Time.time;
        }
        //Fade out Logo
        if (Time.time > minimumLogoTime && loadTime != 0) {
            fadeGroup.alpha = Time.time - minimumLogoTime;
            if (fadeGroup.alpha >= 1) {
                if (FirebaseManager.User != null) {
                    SceneManager.LoadScene ("MainMap");
                } else {
                    SceneManager.LoadScene ("MainMenu");   
                }
            }
        }
    }//end Update

}//end Preloader


