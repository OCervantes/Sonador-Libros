using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour {

    public string nextScene = "10";
    public string authScene = "1";

    // Minimum time dedicated to waiting for everything to load
    public float minLoadTime = 3.0f;

    private CanvasGroup fadeGroup;
    private float loadTime;

    private void Start() {
        //Grab the only CanvasGroup in the scene
        fadeGroup = FindObjectOfType<CanvasGroup>();
        //Start with a white screen;
        fadeGroup.alpha = 1;

        //Get a timestamp of the completion time.
        // To avoid issues an absolute min time of 0.1f is used.
        if (Time.time < minLoadTime)
            loadTime = minLoadTime;
        else if (minLoadTime > 0) {
            loadTime = Time.time;
        } else {
            loadTime = 0.1f;
        }
    }//end Start

    private void Update() {
        if (Time.time <= minLoadTime) {  //Fade in Logo
            fadeGroup.alpha = 1 - Time.time;
        } else {  //Fade out Logo
            fadeGroup.alpha = Time.time - minLoadTime;
            if (fadeGroup.alpha >= 1) {
                string scene;

                // Choose what scene to load based on whether a user
                // is logged in or not.
                if (FirebaseManager.User != null) {
                    scene = nextScene;
                } else {
                    scene = authScene;
                }

                // Try to utilize scene to load as an id,
                // otherwise default to loading it as a name
                if (Int32.TryParse(scene, out int id)) {
                    SceneManager.LoadScene(id);
                } else {
                    SceneManager.LoadScene(scene);
                }
            }
        }
    }//end Update

}//end Preloader


