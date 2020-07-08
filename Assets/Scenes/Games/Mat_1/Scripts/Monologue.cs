using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Monologue : MonoBehaviour
{
    public Text UIText;
    public GameObject continueButton, dialogBackground, sliderText, slider;    
    public static int fruits, sliderValue;
    //public FruitInitialization fruitInitializer;

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
        
        Debug.Log("Monologue Total Fruits: " + fruits + "\nSlider Value: " + sliderValue);

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

    public void Verify()
    {              
        /* If the Slider's value concurs with the amount of fruits collected in the previous Scene ("Juego 1"):
        
         * The FruitInitialization Script will generate new fruits to collect, once the Player comes back to the "Juego 1"
           Scene.
         * Player will be taken to the "Agradecimiento" Scene, to celebrate their achievement.        
         */     
        if (sliderValue == fruits)
        {
            // This might be redundant, but just in case.
            //fruitInitializer.SetFruitInitializationFlag(true);
            FruitInitialization.playerCollectedCorrectAmountOfFruits = true;
            SceneManager.LoadScene("Agradecimiento");
        }
        /* If not:

         * The FruitInitialization Script will generate the same fruits that were requested this time, in order to have
           the player practice with the fruits they did not collect correctly.
         * Player will be taken to the "New Corrección" Scene, in order to learn from their mistakes, and have another go
           at the game.           
         */
        else
        {
            //fruitInitializer.SetFruitInitializationFlag(false);
            FruitInitialization.playerCollectedCorrectAmountOfFruits = false;
            SceneManager.LoadScene("New Corrección");
        }
    } 
}
