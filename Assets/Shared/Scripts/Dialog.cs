using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public Text UIText;
    public string[] sentences;
    public int index;
    private int clickCounter=0, sceneIndex;
    public float typingSpeed = 0.02f;
    //public VideoPlayer videoPlayer;
    public GameObject continueButton;
    public AudioClip[] audios;
    public AudioSource source;
    public GameObject dialogBackground;
    public GameObject endgame;

    //public bool isItWorking;      Serves to determine if an imported Script is incompatible

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("clickCounter original: " + clickCounter);   
        // Porque luego inicia en un número mayor, por razones desconocidas.
        clickCounter = 0;
        Debug.Log("clickCounter asignado: " + clickCounter);   

    
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        dialogBackground.SetActive(true);

        Debug.Log("Index: " + index);
        StartCoroutine(Type());
        Debug.Log("Number of sentences: " + sentences.Length);
        Debug.Log("Audios Start: " + audios.Length);

        source = GetComponent<AudioSource>();    
    }
    
    private void Update() 
    {        
        if (UIText.text == sentences[index])
        {
            continueButton.SetActive(true);
        }

        if (clickCounter==sentences.Length)
        {
            //videoPlayer.Play();
            // Quizás llame al Script LoadScene en otra iteración.
//            if (sceneIndex == 0)

            //No funciona si se pone SceneIndex++
            SceneManager.LoadScene(sceneIndex+1);

            if (sceneIndex == 3)
                endgame.SetActive(true);            
            /*else if (sceneIndex == 3)
                endgame.SetActive(true);*/

        }        
    }

    IEnumerator Type()
    {
        Debug.Log("Couroutine Audio: " + audios.Length);            
        //Debug.Log("This index: " + index);
        foreach(char letter in sentences[index].ToCharArray())
        {            
            UIText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        //audios = this.audios;

        Debug.Log("clickCounter Pre: " + clickCounter);
        clickCounter++;
        Debug.Log("clickCounter Pos: " + clickCounter);
        Debug.Log("Next Sentence Audios: " + audios.Length);                        
        source.PlayOneShot(audios[clickCounter]);
        continueButton.SetActive(false);        
        
        if (index < sentences.Length - 1)
        {
            index++;
            UIText.text = "";
            StartCoroutine(Type());
        }
        else
        {                        
            UIText.text = "";
            continueButton.SetActive(false);
        }        
    }

    /* 
    // Update is called once per frame
    void Update()
    {
        
    }*/

}
