using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public Text UIText;
    // Public access due to reference in Animation.cs 
    public /*static*/ string[] sentences;
    public /*static*/ int index;
    public GameObject continueButton, dialogBackground, endgame;
    int sceneIndex;
    [SerializeField] float typingSpeed;    
    [SerializeField] AudioClip[] audios;
    AudioSource source;  

    /* More convenient to track the Slider's value from SliderNumber's printValue() than from the Slider's attribute it-
       self, given that printValue() is the method in charge of printing the slider's value each time it has been modi-
       ied, and trying to register the slider's value from the Slider would require being part of the Update() method.
       Less efficient.
     */

    void Start()
    {
        typingSpeed = 0.02f;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        dialogBackground.SetActive(true);

        source = GetComponent<AudioSource>();        

        StartCoroutine(Type());                
    } 

    IEnumerator Type()
    { 
        source.PlayOneShot(audios[index]);

        foreach(char letter in sentences[index].ToCharArray())
        {            
            UIText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        //Debug.Log("Finished tying.");      
        continueButton.SetActive(true);                            
    }

    // Public due to it being referenced by Dialog Background's ContinueButton.
    public void NextSentence()
    {        
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
            dialogBackground.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Agradecimiento" || SceneManager.GetActiveScene().name == "New Corrección")
                endgame.SetActive(true);                         
            else
                SceneManager.LoadScene(sceneIndex+1);
        }                
    } 
}