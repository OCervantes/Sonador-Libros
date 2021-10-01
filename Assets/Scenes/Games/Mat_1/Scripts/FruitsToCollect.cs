using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FruitsToCollect : MonoBehaviour
{
    public FruitInitialization fruitInitializer;     // Referenced by Basket and Dialog
    public AudioSource audioSource;
    [SerializeField] AudioClip[] un, singleFruitsFeru, numbersFeru, pluralFruitsFeru;
    static int[] NUMBER_OF_FRUITS_PER_TYPE;          // Declarado al inicio, porque no se permite declarar vars estáticas dentro de Start()

    // Start is called before the first frame update
    void Start()
    {
        NUMBER_OF_FRUITS_PER_TYPE  = new int [3];
        GameObject[] FRUIT_OBJECTS = new GameObject [3];

        Text instructions = GetComponent<Text>();
        instructions.text = "";


        // Debido a que se manda a llamar en SayInstructions, no se puede declarar después del ciclo for
        float elapsedInstructionAudioTime = 0f;
        
        int NUMBER_OF_FRUIT_TYPES = fruitInitializer.fruits.Length;     // = 3

        // For each fruit type:
        for (int i = 0; i < NUMBER_OF_FRUIT_TYPES; i++)
        {
            // Get corresponding fruit index:
            /*
                0 = Manzana
                1 = Pera
                2 = Durazno
             */
            int CURRENT_FRUIT_INDEX = FruitInitialization.fruitIndexes[i];

            // Get corresponding fruit and the number of its instances
            FRUIT_OBJECTS[i]             = fruitInitializer.fruits[CURRENT_FRUIT_INDEX];
            NUMBER_OF_FRUITS_PER_TYPE[i] = FruitInitialization.numFruits[CURRENT_FRUIT_INDEX];

            // Print it in the instructions
            const string SPACES_BETWEEN_NUMBER_OF_FRUITS = "        ";
            instructions.text += NUMBER_OF_FRUITS_PER_TYPE[i] + SPACES_BETWEEN_NUMBER_OF_FRUITS;

            if ( i != (NUMBER_OF_FRUIT_TYPES - 1) )            
                instructions.text += ", ";
            
            // Play corresponding audio
            StartCoroutine( SayInstructions(i, CURRENT_FRUIT_INDEX) );
        
        }   // End for
        
        IEnumerator SayInstructions(int i, int CURRENT_FRUIT_INDEX)
        {
            Debug.Log("Instructions Coroutine");

            // Fruta (singular)
            if (FruitInitialization.numFruits[CURRENT_FRUIT_INDEX] == 1)
            {
                // Masculina (Durazno)
                if (fruitInitializer.fruits[CURRENT_FRUIT_INDEX].name == "Durazno")
                {
                    // Un                    
                    yield return new WaitForSeconds(un[0].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(un[0]);
                    elapsedInstructionAudioTime += un[0].length;
                    
                    // Durazno
                    yield return new WaitForSeconds(singleFruitsFeru[2].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(singleFruitsFeru[2]);
                    elapsedInstructionAudioTime += singleFruitsFeru[2].length;
                
                }   // End if

                // Femenina (Manzana, Pera)
                else
                {
                    // Una                    
                    yield return new WaitForSeconds(numbersFeru[0].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(numbersFeru[0]);
                    elapsedInstructionAudioTime += numbersFeru[0].length;
                    
                    // (fruta)
                    yield return new WaitForSeconds(singleFruitsFeru[CURRENT_FRUIT_INDEX].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(singleFruitsFeru[CURRENT_FRUIT_INDEX]);
                    elapsedInstructionAudioTime += singleFruitsFeru[CURRENT_FRUIT_INDEX].length;
                
                }   // End else

            }   // End if

            // Frutas (plural)
            else
            {
                // Masculinas (Duraznos)
                if (fruitInitializer.fruits[CURRENT_FRUIT_INDEX].name == "Durazno")
                {
                    // (número variable)                    
                    yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[CURRENT_FRUIT_INDEX]-1].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(numbersFeru[FruitInitialization.numFruits[CURRENT_FRUIT_INDEX]-1]);
                    elapsedInstructionAudioTime += numbersFeru[FruitInitialization.numFruits[CURRENT_FRUIT_INDEX]-1].length;
                                        
                    // Duraznos
                    yield return new WaitForSeconds(pluralFruitsFeru[2].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(pluralFruitsFeru[2]);
                    elapsedInstructionAudioTime += pluralFruitsFeru[2].length;
                
                }   // End if

                // Femeninas (Manzanas, Peras)
                else
                {
                    // (número variable)                
                    yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[CURRENT_FRUIT_INDEX]-1].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(numbersFeru[FruitInitialization.numFruits[CURRENT_FRUIT_INDEX]-1]);
                    elapsedInstructionAudioTime += numbersFeru[FruitInitialization.numFruits[CURRENT_FRUIT_INDEX]-1].length;
                    
                    // (frutas)
                    yield return new WaitForSeconds(pluralFruitsFeru[CURRENT_FRUIT_INDEX].length + elapsedInstructionAudioTime);
                    audioSource.PlayOneShot(pluralFruitsFeru[CURRENT_FRUIT_INDEX]);
                    elapsedInstructionAudioTime += pluralFruitsFeru[CURRENT_FRUIT_INDEX].length;
                
                }   // End else
            
            }   // End else
        
        }   // End SayInstructions
        
        instructions.fontSize = 100;

        const int FRUIT_Y_COORDINATE = 965;
        const float FRUIT_SIZE = 1.25f;

        // Show first fruit in instructions
        float FRUIT_X_COORDINATE = 325;
        GameObject fruit = Instantiate(FRUIT_OBJECTS[0], new Vector2(FRUIT_X_COORDINATE, FRUIT_Y_COORDINATE), Quaternion.identity, gameObject.transform
                                                                                                                        .parent            // Instructions Background
                                                                                                                        .parent            // Canvas
                                                                                                                        .GetChild(1)       // Game Frame
                                                                                                                        .GetChild(0));     // Fruit A Panel (Source)
        fruit.transform.localScale = Vector3.one * FRUIT_SIZE;

        
        // Show second fruit in instructions
        FRUIT_X_COORDINATE = 615;
        fruit = Instantiate(FRUIT_OBJECTS[1], new Vector2(FRUIT_X_COORDINATE, FRUIT_Y_COORDINATE), Quaternion.identity, gameObject.transform
                                                                                                                        .parent            // Instructions Background
                                                                                                                        .parent            // Canvas
                                                                                                                        .GetChild(1)       // Game Frame
                                                                                                                        .GetChild(1));     // Fruit B Panel (Source)
        fruit.transform.localScale = Vector3.one * FRUIT_SIZE;

        
        // Show third fruit in instructions
        FRUIT_X_COORDINATE = 925;
        fruit = Instantiate(FRUIT_OBJECTS[2], new Vector2(FRUIT_X_COORDINATE, FRUIT_Y_COORDINATE), Quaternion.identity, gameObject.transform
                                                                                                                        .parent            // Instructions Background
                                                                                                                        .parent            // Canvas
                                                                                                                        .GetChild(1)       // Game Frame
                                                                                                                        .GetChild(2));     // Fruit C Panel (Source)
        fruit.transform.localScale = Vector3.one * FRUIT_SIZE;

    }   // End Start

}   // End class FruitsToCollect
