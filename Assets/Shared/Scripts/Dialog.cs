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
    public GameObject continueButton;
    public AudioClip[] audios;
    public AudioSource source;
    public GameObject dialogBackground;
    public GameObject endgame;

    void Start()
    {    
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        dialogBackground.SetActive(true);

        StartCoroutine(Type());        

        source = GetComponent<AudioSource>();

        Debug.Log("audios[0]" + audios[0] + "\naudios[1]" + audios[1] + "\naudios[2]" + audios[2]);
    }
    
    private void Update() 
    {        
        if (UIText.text == sentences[index])
        {            
            continueButton.SetActive(true);            
        }

        if (clickCounter==sentences.Length)
        {
            dialogBackground.SetActive(false);

            if (sceneIndex == 9)
                endgame.SetActive(true);             
            else                
                SceneManager.LoadScene(sceneIndex+1);
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
        clickCounter++;
       
        if (clickCounter < sentences.Length)
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

}
