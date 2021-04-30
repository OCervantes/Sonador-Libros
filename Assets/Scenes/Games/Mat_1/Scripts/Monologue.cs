using System.Collections;
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
    // Initialize variables
    void Start()
    {
        typingSpeed = 0.02f;        
        
        dialogBackground.SetActive(true);

        source = GetComponent<AudioSource>();        
        
        // See that values given by Basket.cs (fruits) and SliderNumber.cs (sliderValue) are correct.
        Debug.Log("Monologue Total Fruits: " + fruits + 
                  "\nSlider Value: " + sliderValue);

        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        // Play instructions audio
        source.PlayOneShot(audios);

        // Display instructions text
        foreach(char letter in sentence.ToCharArray())
        {            
            UIText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Once instructions have finished displaying, the slider and the Continue Button will activate.
        continueButton.SetActive(true);                  
        sliderText.SetActive(true);
        slider.SetActive(true);        
    }

    public void Verify()
    {              
        // If the Slider's value concurs with the amount of fruits collected in the previous Scene ("Juego 1"):             
        if (sliderValue == fruits)
        {
            /* (This might be redundant, but just in case.)
             * The FruitInitialization Script will generate new fruits to collect, once the Player comes back to the "Juego 1" Scene.
            */            
            FruitInitialization.playerCollectedCorrectAmountOfFruits = true;

            // Player will be taken to the "Agradecimiento" Scene, to celebrate their achievement.
            SceneManager.LoadScene("Agradecimiento");
        }
        
        // Otherwise...
        else
        {
            /* The FruitInitialization Script will generate the same fruits that were requested this time, in order to have the player practice 
               with the fruits they did not collect correctly.
             */
            FruitInitialization.playerCollectedCorrectAmountOfFruits = false;

            // Player will be taken to the "New Corrección" Scene, in order to learn from their mistakes, and have another go at the game.
            SceneManager.LoadScene("New Corrección");
        }
    } 
}
