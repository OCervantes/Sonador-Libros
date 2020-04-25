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
    private int clickCounter = 0, sceneIndex;
    public float typingSpeed = 0.02f;
    public GameObject continueButton;
    
    public GameObject loader;
    //public GameObject dialog_audio;
    public AudioClip[] audios;
    public AudioSource source;
    public GameObject dialogBackground;
    public GameObject endgame;
    //public Levelloader loader;
    

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        dialogBackground.SetActive(true);
        //dialog_audio = GameObject.FindWithTag("Dialog and Audio Manager"); 
        //loader = dialog_audio.AddComponent<Levelloader>();
        //dialog_audio = GameObject.FindWithTag ("Dialog and Audio Manager");    
        StartCoroutine(Type());
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
            if (sceneIndex == 9){
                 endgame.SetActive(true);
            }
        
        }
    }

    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            UIText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

     public void NextSentence()
    {
        //loader = new GameObject();
        //Levelloader loader = new Levelloader(); 
        //dialog_audio = GameObject.FindWithTag("Dialog and Audio Manager");
        //loader = dialog_audio.AddComponent<Levelloader>();
        loader= GameObject.FindWithTag("Cross_Fade");
        //continueButton= GameObject.FindWithTag("Button");
        //UIText = Text.FindSceneObjectsOfType(Text);
        //source = GetComponent<AudioSource>();
        //source.clip = audio[];
        clickCounter++;
        continueButton.SetActive(false);
        if(clickCounter <= audios.Length){
        source.PlayOneShot(audios[index]);

            if (index < sentences.Length - 1)
            {
                index++;
                UIText.text = "";
                gameObject.SetActive(true);
                StartCoroutine(Type());
                Debug.Log("bueno");
            }
            else
            {   
                //Debug.Log("Hola");
                UIText.text = "";
                continueButton.SetActive(false);
            }
        }
        else{
            Debug.Log("Hola");
            loader.GetComponent<Levelloader>().LoadNextLevel();
        }
    }

}
