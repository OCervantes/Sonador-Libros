using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FruitsToCollect : MonoBehaviour
{
    public FruitInitialization fruitInitializer;     // Referenced by Basket and Dialog
    public AudioSource audioSource;
    [SerializeField] AudioClip[] un, singleFruitsFeru, numbersFeru, pluralFruitsFeru;
    static uint[] NUMBER_OF_FRUITS_PER_TYPE;          // Declarado al inicio, porque no se permite declarar vars estáticas dentro de Start()
    [SerializeField] Image[] instructionFruitImages;
    
    // Debido a que se manda a llamar en SayInstructions, no se puede declarar después del ciclo for
    private float elapsedInstructionAudioTime = 1.0f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        NUMBER_OF_FRUITS_PER_TYPE  = new uint [3];
        GameObject[] FRUIT_OBJECTS = new GameObject [3];

        Text instructions = GetComponent<Text>();
        instructions.text = "";
        
        uint NUMBER_OF_FRUIT_TYPES = (uint) fruitInitializer.fruits.Length;     // = 3

        // For each fruit type:
        for (int i = 0; i < NUMBER_OF_FRUIT_TYPES; i++)
        {
            // Get corresponding fruit index:
            /*
                0 = Manzana
                1 = Pera
                2 = Durazno
             */
            uint CURRENT_FRUIT_INDEX = (uint) FruitInitialization.fruitIndexes[i];

            // Get corresponding fruit and the number of its instances
            FRUIT_OBJECTS[i]             = fruitInitializer.fruits[CURRENT_FRUIT_INDEX];
            NUMBER_OF_FRUITS_PER_TYPE[i] = (uint) FruitInitialization.numFruits[CURRENT_FRUIT_INDEX];

            // Print it in the instructions
            const string SPACES_BETWEEN_NUMBER_OF_FRUITS = "    ";
            instructions.text += NUMBER_OF_FRUITS_PER_TYPE[i] + SPACES_BETWEEN_NUMBER_OF_FRUITS;

            if ( i != (NUMBER_OF_FRUIT_TYPES - 1) )            
                instructions.text += ", ";

            // Replace the instructions' default Image sprites to those belonging to the corresponding fruits to be collected
            instructionFruitImages[i].sprite = FRUIT_OBJECTS[i].GetComponent<Image>().sprite;
        
        }   // End for

        // Ciclo por aparte, para evitar suspender la carga de sprites de las instruccioens  
        for (int i = 0; i < NUMBER_OF_FRUIT_TYPES; i++)
        {
            uint CURRENT_FRUIT_INDEX = (uint) FruitInitialization.fruitIndexes[i];

            // Play corresponding audio
            yield return new WaitForSeconds(1.5f);
            StartCoroutine( SelectInstructionsAudio(NUMBER_OF_FRUITS_PER_TYPE[i], FRUIT_OBJECTS[i], CURRENT_FRUIT_INDEX) );
        }

    }   // End Start
    
    IEnumerator SelectInstructionsAudio(uint numberOfFruits, GameObject fruitType, uint fruitIndex)
    {
        const float TIMEBREAK_BEFORE_NUMBER_OF_FRUITS = 0.73f;
        const float TIMEBREAK_BEFORE_FRUIT_NAMES = 0.5f;

        // One fruit
        if (numberOfFruits == 1)
        {
            // Is the fruit a peach (masculine)?
            yield return new WaitForSeconds(TIMEBREAK_BEFORE_NUMBER_OF_FRUITS);
            if (fruitType.name == "Durazno")
                PlayInstructionsAudio(un[0]);                       // "Un"

            // If not, it's feminine (Manzana, Pera)
            else
                PlayInstructionsAudio(numbersFeru[0]);              // "Una"

            
            yield return new WaitForSeconds(TIMEBREAK_BEFORE_FRUIT_NAMES);
            PlayInstructionsAudio(singleFruitsFeru[fruitIndex]);    // (fruit)
        }   // End if

        // More than one fruit
        else if (numberOfFruits > 1)
        {
            // (Number of fruits)
            yield return new WaitForSeconds(TIMEBREAK_BEFORE_NUMBER_OF_FRUITS);
            PlayInstructionsAudio(numbersFeru[numberOfFruits - 1]);

            // (fruits)
            yield return new WaitForSeconds(TIMEBREAK_BEFORE_FRUIT_NAMES);
            PlayInstructionsAudio(pluralFruitsFeru[fruitIndex]);

        }   // End else
        
    }   // End SelectInstructionsAudio

    void PlayInstructionsAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);

    }   // End PlayInstructionsAudio

}   // End class FruitsToCollect
