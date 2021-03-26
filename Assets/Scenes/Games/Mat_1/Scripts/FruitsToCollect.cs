﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FruitsToCollect : MonoBehaviour
{
    public FruitInitialization initializer;
    static int[] fruitNumbers;
    //static string[] fruitNames;
    Text instructions;
    GameObject[] fruits;
    /*Vector2*/float fruitXPosition;
    public AudioSource source;
    [SerializeField] AudioClip[] un, singleFruitsFeru, numbersFeru, pluralFruitsFeru;

    float accumulatedInstructionDuration;

    // Start is called before the first frame update
    void Start()
    {
        GameObject fruit;
        fruitNumbers = new int [3];
        fruits = new GameObject [3];
        //fruitPosition = new Vector2();

        //fruitNames = new string [3];        
        instructions = GetComponent<Text>();
        instructions.text = "";

        accumulatedInstructionDuration = 0f;
        /*
        fruitNames[0] = initializer.fruits[FruitInitialization.fruitIndexes[0]].name;
        fruitNames[1] = initializer.fruits[FruitInitialization.fruitIndexes[1]].name;
        fruitNames[2] = initializer.fruits[FruitInitialization.fruitIndexes[2]].name;
        */

        

        for (int i=0; i<initializer.fruits.Length; i++)
        {
            fruitNumbers[i] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]];
            fruits[i] = initializer.fruits[FruitInitialization.fruitIndexes[i]];

            instructions.text += fruitNumbers[i] + "        ";

            StartCoroutine(SayInstructions(i));

            if(i !=2)
            {
                instructions.text += ", ";
            } 
        }        

        IEnumerator SayInstructions(int i)
        {
            Debug.Log("Instructions Coroutine");
            if (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]] == 1)
            {
                // Masculina (Durazno)
                if (initializer.fruits[FruitInitialization.fruitIndexes[i]].name == "Durazno")
                {
                    // Un                    
                    yield return new WaitForSeconds(un[0].length + accumulatedInstructionDuration);
                    source.PlayOneShot(un[0]);
                    accumulatedInstructionDuration += un[0].length;
                    
                    // Durazno
                    yield return new WaitForSeconds(singleFruitsFeru[2].length + accumulatedInstructionDuration);
                    source.PlayOneShot(singleFruitsFeru[2]);
                    accumulatedInstructionDuration += singleFruitsFeru[2].length;
                }

                // Femenina (Manzana, Pera)
                else
                {
                    // una                    
                    yield return new WaitForSeconds(numbersFeru[0].length + accumulatedInstructionDuration);
                    source.PlayOneShot(numbersFeru[0]);
                    accumulatedInstructionDuration += numbersFeru[0].length;
                    
                    // (fruta)
                    yield return new WaitForSeconds(singleFruitsFeru[FruitInitialization.fruitIndexes[i]].length + accumulatedInstructionDuration);
                    source.PlayOneShot(singleFruitsFeru[FruitInitialization.fruitIndexes[i]]);
                    accumulatedInstructionDuration += singleFruitsFeru[FruitInitialization.fruitIndexes[i]].length;
                }
            }

            // Frutas (plural)
            else
            {
                // Masculinas (Duraznos)
                if (initializer.fruits[FruitInitialization.fruitIndexes[i]].name == "Durazno")
                {
                    // (número variable)                    
                    yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]]-1].length + accumulatedInstructionDuration);
                    source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]]-1]);
                    accumulatedInstructionDuration += numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]]-1].length;
                                        
                    // Duraznos
                    yield return new WaitForSeconds(pluralFruitsFeru[2].length + accumulatedInstructionDuration);
                    source.PlayOneShot(pluralFruitsFeru[2]);
                    accumulatedInstructionDuration += pluralFruitsFeru[2].length;
                }

                // Femeninas (Manzanas, Peras)
                else
                {
                    // (número variable)                
                    yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]]-1].length + accumulatedInstructionDuration);
                    source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]]-1]);
                    accumulatedInstructionDuration += numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]]-1].length;
                    
                    // (frutas)
                    yield return new WaitForSeconds(pluralFruitsFeru[FruitInitialization.fruitIndexes[i]].length + accumulatedInstructionDuration);
                    source.PlayOneShot(pluralFruitsFeru[FruitInitialization.fruitIndexes[i]]);
                    accumulatedInstructionDuration += pluralFruitsFeru[FruitInitialization.fruitIndexes[i]].length;
                }
            }
        }
        /*
        for (int i=0; i<instructions.text.Length; i++)
        {
            Debug.Log("Char #" + i +  ": " + instructions.text.ToCharArray()[i]);
        }

        instructions.text.ToCharArray()[/*instructions.text.Length - 231] = ' ';//.Replace(',', ' ');
        instructions.text.Trim();
        */
        instructions.fontSize = 52;

        /*
        if (fruits[0].name == "manzana")
        {
            fruitPosition.x = 120;
            fruitPosition.y = 55;
        }
        else if (fruits[0].name == "pera")
        {
            fruitPosition.x = 105;
            fruitPosition.y = 70;
        }
        else
        {
            fruitPosition.x = 120;
            fruitPosition.y = 60;            
        }
        */        

        if (fruits[0].name == "pera")
            fruitXPosition = 111.5f;
        else
            fruitXPosition = 118;                

        fruit = Instantiate(fruits[0], /*new Vector2(0, 0)*/new Vector2(fruitXPosition, 669), Quaternion.identity, gameObject.transform
                                                                                                                                 .parent
                                                                                                                                 .GetChild(1)
                                                                                                                                 .GetChild(0));
        fruit.transform.localScale = Vector3.one * 0.65f;


        if (fruits[1].name == "manzana")
            fruitXPosition = 264;
        else if (fruits[1].name == "durazno")
            fruitXPosition = 271.8f;
        else
            fruitXPosition = 260;

        fruit = Instantiate(fruits[1], /*new Vector2(0, 0)*/new Vector2(fruitXPosition, 669), Quaternion.identity, gameObject.transform
                                                                                                                                 .parent
                                                                                                                                 .GetChild(1)
                                                                                                                                 .GetChild(0));
        fruit.transform.localScale = Vector3.one * 0.65f;


        if (fruits[2].name == "pera")
            fruitXPosition = 421.2f;
        else
            fruitXPosition = 430;

        fruit = Instantiate(fruits[2], /*new Vector2(0, 0)*/new Vector2(fruitXPosition, 669), Quaternion.identity, gameObject.transform
                                                                                                                                 .parent
                                                                                                                                 .GetChild(1)
                                                                                                                                 .GetChild(0));
        fruit.transform.localScale = Vector3.one * 0.65f;                                                                                                                             
        /*
        fruitNumbers[0] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]];
        fruitNumbers[1] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]];
        fruitNumbers[2] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]];

        fruits[0] = initializer.fruits[FruitInitialization.fruitIndexes[0]];
        fruits[1] = initializer.fruits[FruitInitialization.fruitIndexes[1]];
        fruits[2] = initializer.fruits[FruitInitialization.fruitIndexes[2]];
        */

        /*
        instructions = GetComponent<Text>();
        instructions.text = "";
         */

        /*    
        for (int i=0; i<3; i++)
        {
            if (fruitNumbers[i] > 1)
            {
                if (fruitNames[i] == "Peach")
                    fruitNames[i] += "es";
                else
                    fruitNames[i] += "s";
            }                
        }*/

        //printInstructions();

        /*
        instructions.text = fruitNumbers[0] + "      " /*+ fruitNames[0] + ", " 
                          + fruitNumbers[1] + "      " /*+ fruitNames[1] + ", "
                          + fruitNumbers[2] + "      " /*+ fruitNames[2];
        */


    }

    /*
    void printInstructions()
    {
        instructions.text = fruitNumbers[0] + "             " /*+ fruitNames[0] + ", " 
                          + fruitNumbers[1] + "             " /*+ fruitNames[1] + ", "
                          + fruitNumbers[2] + "             " /*+ fruitNames[2];
    }

    void instantiateFruits()
    {

    }
    */

}
