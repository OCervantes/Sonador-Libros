using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FruitsToCollect : MonoBehaviour
{
    public FruitInitialization fruitInitializer;     // Referenced by Basket and Dialog
    public AudioSource audioSource;
    [SerializeField] AudioClip[] un, singleFruitsFeru, numbersFeru, pluralFruitsFeru;
    static int[] NUMBER_OF_FRUITS_PER_TYPE;          // Declarado al inicio, porque no se permite declarar vars estáticas dentro de Start()

    [SerializeField] Image[] instructionFruitImages;

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
            const string SPACES_BETWEEN_NUMBER_OF_FRUITS = "    ";
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
        
        // instructions.fontSize = 87;

        /*
        float FRUIT_Y_COORDINATE = Screen.height * 0.9058f;
        Debug.Log("Fruit Y coordinate: " + FRUIT_Y_COORDINATE);
        const float FRUIT_SIZE = 0.475f;

        Debug.Log(Screen.currentResolution);
        Debug.Log(Screen.width + ", " + Screen.height);

        // 
        float Vector2_X_to_RectTransform_PosX_Factor = ( Screen.width * (float) 5.83e-4 ) + 1.905f;
        switch (Screen.width)
        {
            case 800:
                Vector2_X_to_RectTransform_PosX_Factor = 1.6f;
                break;
            
            case 1280:
                Vector2_X_to_RectTransform_PosX_Factor = 1.066666f + (float) 6.8557e-5;
                break;

            case 1920:
            case 2160:
                Vector2_X_to_RectTransform_PosX_Factor = 0.711111f + (float) 1.1742e-4;
                break;

            case 2560:
            case 2960:
                Vector2_X_to_RectTransform_PosX_Factor = 0.533333f + (float) 5.2576e-5;
                break;
        }

        // Show first fruit in instructions
        float FRUIT_X_COORDINATE = (98 / Vector2_X_to_RectTransform_PosX_Factor);
        switch (Screen.width)
        {
            case 800:
                FRUIT_X_COORDINATE = 98;
                break;
            
            case 1280:
                FRUIT_X_COORDINATE = 144.5334f;
                break;

            case 1920:
            case 2560:
                FRUIT_X_COORDINATE = 147;
                break;
            
            case 2160:
                FRUIT_X_COORDINATE = 233;
                break;

            case 2960:
                FRUIT_X_COORDINATE = 257.85f;
                break;
        }
        // FRUIT_X_COORDINATE /= Vector2_X_to_RectTransform_PosX_Factor;
        Debug.Log("FRUIT X coordinate: " + FRUIT_X_COORDINATE);
        */

        /*GameObject fruit = Instantiate(FRUIT_OBJECTS[0], new Vector2(FRUIT_X_COORDINATE, FRUIT_Y_COORDINATE), Quaternion.identity, gameObject.transform
                                                                                                                                             .parent);          // Instructions Background
                                                                                                                                            /*.parent            // Canvas
                                                                                                                                            .GetChild(1)       // Game Frame
                                                                                                                                            .GetChild(0));     // Fruit A Panel (Source)
        fruit.transform.localScale = Vector3.one * FRUIT_SIZE;*/
        instructionFruitImages[0].sprite = FRUIT_OBJECTS[0].GetComponent<Image>().sprite;

        
        // Show second fruit in instructions
        /*
        FRUIT_X_COORDINATE = (327 / Vector2_X_to_RectTransform_PosX_Factor);
        GameObject fruit = Instantiate(FRUIT_OBJECTS[1], new Vector2(FRUIT_X_COORDINATE, FRUIT_Y_COORDINATE), Quaternion.identity, gameObject.transform
                                                                                                                                    .parent);            // Instructions Background
                                                                                                                                    /*.parent            // Canvas
                                                                                                                                    .GetChild(1)       // Game Frame
                                                                                                                                    .GetChild(1));     // Fruit B Panel (Source)
        fruit.transform.localScale = Vector3.one * FRUIT_SIZE;
        */
        instructionFruitImages[1].sprite = FRUIT_OBJECTS[1].GetComponent<Image>().sprite;

        
        // Show third fruit in instructions
        /*FRUIT_X_COORDINATE = (459 / Vector2_X_to_RectTransform_PosX_Factor);
        fruit = Instantiate(FRUIT_OBJECTS[2], new Vector2(FRUIT_X_COORDINATE, FRUIT_Y_COORDINATE), Quaternion.identity, gameObject.transform
                                                                                                                        .parent);            // Instructions Background
                                                                                                                        /*.parent            // Canvas
                                                                                                                        .GetChild(1)       // Game Frame
                                                                                                                        .GetChild(2));     // Fruit C Panel (Source)
        fruit.transform.localScale = Vector3.one * FRUIT_SIZE;*/
        instructionFruitImages[2].sprite = FRUIT_OBJECTS[2].GetComponent<Image>().sprite;

    }   // End Start

}   // End class FruitsToCollect
