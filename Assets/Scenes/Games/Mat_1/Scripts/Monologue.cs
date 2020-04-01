using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Monologue : MonoBehaviour
{
    public Text UIText;
    public GameObject continueButton, dialogBackground, sliderText, slider;    
    public static int fruits, sliderValue;     
    [SerializeField] string sentence;
    [SerializeField] float typingSpeed;    
    [SerializeField] AudioClip audios;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        typingSpeed = 0.02f;        
        
        dialogBackground.SetActive(true);

        source = GetComponent<AudioSource>();     
        
        Debug.Log("Total Fruits: " + fruits + "\nSlider Value: " + sliderValue);

        StartCoroutine(Type());
    }

    IEnumerator Type()
    { 
        source.PlayOneShot(audios);

        foreach(char letter in sentence.ToCharArray())
        {            
            UIText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        //Debug.Log("Finished tying.");      
        continueButton.SetActive(true);            
                    
        sliderText.SetActive(true);
        slider.SetActive(true);        
    }

    public void NextSentence()
    {                   
        if (sliderValue == fruits)
            SceneManager.LoadScene("Agradecimiento");
        else
            SceneManager.LoadScene("New Corrección");         
    } 
}
