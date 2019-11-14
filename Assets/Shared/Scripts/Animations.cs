using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Animations: MonoBehaviour
{
    public GameObject dialogAndAudioMan;
    public GameObject[] personajes;
    int scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
    }
    // Update is called once per frame
    void Update()
    {
        switch(scene)
        {
            // Escena: "Introducción"
            case 6:
                switch(dialogAndAudioMan.GetComponent<Dialog>().index)
                {
                    /*case 0:
                        personajes[0].SetActive(true);
                        break;*/
                    case 1:
                        personajes[0].SetActive(false);
                        personajes[1].SetActive(true);
                        break;
                    case 2:
                        personajes[1].SetActive(false);
                        personajes[0].SetActive(true);
                        break;
                    case 3:
                        personajes[0].SetActive(false);
                        personajes[2].SetActive(false);
                        personajes[3].SetActive(true);
                        break;
                }
                break;

            case 9:
                switch(dialogAndAudioMan.GetComponent<Dialog>().index)
                {
                    /*case 0:
                        personajes[0].SetActive(true);
                        break;*/
                    case 1:
                        personajes[0].SetActive(false);
                        personajes[1].SetActive(true);
                        break;
                }
                break;
        }
    }
}
