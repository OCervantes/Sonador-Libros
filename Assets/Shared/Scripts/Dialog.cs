using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public static int totalFruits;

    public Text UIText;
    // Public access due to reference in Animation.cs
    public /*static*/ string[] sentences, maleNounsString, femaleNounsString;
    public /*static*/ int index;
    public GameObject ReturntoFirstScene, continueButton, gobackButton, dialogBackground, endgame, skip, loader, handanimation = null, repetir = null, Manzana1, Manzana2, Durazno1, Durazno2, Pera;
    public FruitInitialization initializer;
    public /*[SerializeField]*/ float typingSpeed;

    [SerializeField] AudioClip[] maleNounsFeru, femaleNounsFeru, maleNounsMati, femaleNounsMati, un, numbersFeru, numbersMati, pluralFruitsFeru, pluralFruitsMati, singleFruitsFeru, singleFruitsMati;

    [SerializeField] public AudioClip[] audios;
    Image myImage;
    GameObject inicio, currentFruitsPanel, currentFruitsIndexesObject;
    Text currentFruitsIndexesText;
    int sceneIndex, firstTypeOfFruits, secondTypeOfFruits, thirdTypeOfFruits;
    int[] typesOfFruits;        
    public AudioSource source;
    bool continuarpresionado = true;   
    string SceneState; //Helps telling in which part of the game we are
    void Start()
    {                        
        // Commented so that it may be edited in the Inspector, given that each Scene's typing speed differs.
        typingSpeed = 0.02f;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        dialogBackground.SetActive(true);
        continueButton.SetActive(false);
        gobackButton.SetActive(false);
        skip.SetActive(false);
        ReturntoFirstScene.SetActive(false);
        


        if (SceneManager.GetActiveScene().name == "Tutorial" && index == 0){//Tutorial de conteo
           handanimation =  GameObject.FindGameObjectWithTag("Hand");
           handanimation.SetActive(false);
           Pera.SetActive(false);
                Manzana2.SetActive(false);
                Manzana1.SetActive(false);
                Durazno1.SetActive(false);
                Durazno2.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "7Game_Intro" && index == 0)
        {
           inicio =  GameObject.FindGameObjectWithTag("Inicio");
           inicio.SetActive(false);
        }

        source = GetComponent<AudioSource>();
        //individualSource = new AudioSource();

        // Scene "Corrección"
        if (sceneIndex == 23)
        {
            // Declare typesOfFruits array.
            typesOfFruits = new int[initializer.fruits.Length];

            // Get total number of fruits instantiated.
            totalFruits = 0;

            for (int i=0; i<initializer.fruits.Length; i++)
            {
                totalFruits += FruitInitialization.numFruits[i];                
            }        

            // Reference Panel displaying current fruits
            currentFruitsPanel = GameObject.Find("Current Fruits Panel");
            //currentFruitsPanel.name = "Pat";

            /* Reference Text object which indicates the indexes of each fruit currently displayed in the Current Fruits
               Panel.
             */
            //currentFruitsIndexesObject = GameObject.Find("Fruit Indexes");
            currentFruitsIndexesText = GameObject.Find("Fruit Indexes")
                                                .GetComponent<Text>();//currentFruitsIndexesObject.GetComponent<Text>();

            // Get number of fruits belonging to the first fruit index.
            for (int i=0; i<initializer.fruits.Length; i++)
            {
                typesOfFruits[i] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[i]];
            }
        }
        
        /*
        firstTypeOfFruits = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]];
        secondTypeOfFruits = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]];
        thirdTypeOfFruits = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]];
        */

        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        if (SceneManager.GetActiveScene().name == "New Corrección")
        {
            //int totalFruits = 0;                        
            switch(index)
            {
                // Feru
                case 0:                                                              
                    //Debug.Log("First Fruits Number: " + firstTypeOfFruits);

                    // Set all other of the instantiated Fruit Panels' visibility to false.                    
                    for (int i=typesOfFruits[0]; i<totalFruits; i++)
                    {
                        currentFruitsPanel.transform
                                          .GetChild(i)
                                          .gameObject
                                          .SetActive(false);
                    }                    

                    // Set Text of the Current Fruits Indexes Object to the current number of fruits displayed.
                    currentFruitsIndexesText.text = " 1";

                    for (int i=2; i<=typesOfFruits[0]; i++)
                    {
                        currentFruitsIndexesText.text += "     " + i;
                    }

                    // Fruta (singular)
                    if (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]] == 1)
                    {
                        // Masculina (Durazno)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[0]].name == "Durazno")
                        {
                            // Éste es
                            source.PlayOneShot(maleNounsFeru[0]);
                            foreach(char letter in maleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // un
                            yield return new WaitForSeconds(un[0].length);
                            source.PlayOneShot(un[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Durazno
                            yield return new WaitForSeconds(singleFruitsFeru[2].length);
                            source.PlayOneShot(singleFruitsFeru[2]);
                            foreach(char letter in "durazno.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femenina (Manzana, Pera)
                        else
                        {
                            // Ésta es
                            source.PlayOneShot(femaleNounsFeru[0]);
                            foreach(char letter in femaleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // una
                            yield return new WaitForSeconds(numbersFeru[0].length);
                            source.PlayOneShot(numbersFeru[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (fruta)
                            yield return new WaitForSeconds(singleFruitsFeru[FruitInitialization.fruitIndexes[0]].length);
                            source.PlayOneShot(singleFruitsFeru[FruitInitialization.fruitIndexes[0]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[0]].name + ".").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    // Frutas (plural)
                    else
                    {
                        // Masculinas (Duraznos)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[0]].name == "Durazno")
                        {
                            // Éstos son
                            source.PlayOneShot(maleNounsFeru[1]);
                            foreach(char letter in maleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]-1].length);
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Duraznos
                            yield return new WaitForSeconds(pluralFruitsFeru[2].length);
                            source.PlayOneShot(pluralFruitsFeru[2]);
                            foreach(char letter in "duraznos.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femeninas (Manzanas, Peras)
                        else
                        {
                            // Éstas son
                            source.PlayOneShot(femaleNounsFeru[1]);
                            foreach(char letter in femaleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]-1].length);
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (frutas)
                            yield return new WaitForSeconds(pluralFruitsFeru[FruitInitialization.fruitIndexes[0]].length);
                            source.PlayOneShot(pluralFruitsFeru[FruitInitialization.fruitIndexes[0]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[0]].name + "s.").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    /*foreach(char letter in sentences[index].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }*/

                    break;

                // Mati
                case 1:
                    // Activate all fruits.
                    ActivateChildren();

                    // Set all other of the instantiated Fruit Panels' visibility to false.                    
                    for (int i=0; i<typesOfFruits[0]; i++)
                    {
                        currentFruitsPanel.transform
                                          .GetChild(i)
                                          .gameObject
                                          .SetActive(false);
                    }                    

                    
                    for (int i=typesOfFruits[0] + typesOfFruits[1]; i<totalFruits; i++)
                    {
                        currentFruitsPanel.transform
                                          .GetChild(i)
                                          .gameObject
                                          .SetActive(false);
                    }                    

                    // Set Text of the Current Fruits Indexes Object to the current number of fruits displayed.
                    currentFruitsIndexesText.text =  "1";

                    for (int i=2; i<=typesOfFruits[1]; i++)
                    {
                        currentFruitsIndexesText.text += "     " + i;
                    }

                    // Fruta (singular)
                    if (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]] == 1)
                    {
                        // Masculina (Durazno)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[1]].name == "Durazno")
                        {
                            // Éste es
                            source.PlayOneShot(maleNounsMati[0]);
                            foreach(char letter in maleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // un
                            yield return new WaitForSeconds(un[1].length);
                            source.PlayOneShot(un[1]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Durazno
                            yield return new WaitForSeconds(singleFruitsMati[2].length);
                            source.PlayOneShot(singleFruitsMati[2]);
                            foreach(char letter in "durazno.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femenina (Manzana, Pera)
                        else
                        {
                            // Ésta es
                            source.PlayOneShot(femaleNounsMati[0]);
                            foreach(char letter in femaleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // una
                            yield return new WaitForSeconds(numbersMati[0].length);
                            source.PlayOneShot(numbersMati[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (fruta)
                            yield return new WaitForSeconds(singleFruitsMati[FruitInitialization.fruitIndexes[1]].length);
                            source.PlayOneShot(singleFruitsMati[FruitInitialization.fruitIndexes[1]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[1]].name + ".").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    // Frutas (plural)
                    else
                    {
                        // Masculinas (Duraznos)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[1]].name == "Durazno")
                        {
                            // Éstos son
                            source.PlayOneShot(maleNounsMati[1]);
                            foreach(char letter in maleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            yield return new WaitForSeconds(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]]-1].length);
                            source.PlayOneShot(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Duraznos
                            yield return new WaitForSeconds(pluralFruitsMati[2].length);
                            source.PlayOneShot(pluralFruitsMati[2]);
                            foreach(char letter in "duraznos.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femeninas (Manzanas, Peras)
                        else
                        {
                            // Éstas son
                            source.PlayOneShot(femaleNounsMati[1]);
                            foreach(char letter in femaleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            yield return new WaitForSeconds(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]]-1].length);
                            source.PlayOneShot(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (frutas)
                            yield return new WaitForSeconds(pluralFruitsMati[FruitInitialization.fruitIndexes[1]].length);
                            source.PlayOneShot(pluralFruitsMati[FruitInitialization.fruitIndexes[1]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[1]].name + "s.").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    break;

                // Feru
                case 2:
                    // Activate all fruits.
                    ActivateChildren();

                    // Set all other of the instantiated Fruit Panels' visibility to false.                    
                    for (int i=0; i<typesOfFruits[0] + typesOfFruits[1]; i++)
                    {
                        currentFruitsPanel.transform
                                          .GetChild(i)
                                          .gameObject
                                          .SetActive(false);
                    }                    

                    /*
                    for (int i=typesOfFruits[0] + typesOfFruits[1]; i<totalFruits; i++)
                    {
                        currentFruitsPanel.transform
                                          .GetChild(i)
                                          .gameObject
                                          .SetActive(false);
                    }
                    */

                    // Set Text of the Current Fruits Indexes Object to the current number of fruits displayed.
                    currentFruitsIndexesText.text =  " 1";

                    for (int i=2; i<=typesOfFruits[2]; i++)
                    {
                        currentFruitsIndexesText.text += "     " + i;

                        if (i==10)
                            currentFruitsIndexesText.text += "    " + i;
                    }

                    // Fruta (singular)
                    if (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]] == 1)
                    {
                        // Masculina (Durazno)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[2]].name == "Durazno")
                        {
                            // Éste es
                            source.PlayOneShot(maleNounsFeru[0]);
                            foreach(char letter in maleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // un
                            yield return new WaitForSeconds(un[0].length);
                            source.PlayOneShot(un[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Durazno
                            yield return new WaitForSeconds(singleFruitsFeru[2].length);
                            source.PlayOneShot(singleFruitsFeru[2]);
                            foreach(char letter in "durazno.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femenina (Manzana, Pera)
                        else
                        {
                            // Ésta es
                            source.PlayOneShot(femaleNounsFeru[0]);
                            foreach(char letter in femaleNounsString[0].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // una
                            yield return new WaitForSeconds(numbersFeru[0].length);
                            source.PlayOneShot(numbersFeru[0]);
                            foreach(char letter in "1 ".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (fruta)
                            yield return new WaitForSeconds(singleFruitsFeru[FruitInitialization.fruitIndexes[2]].length);
                            source.PlayOneShot(singleFruitsFeru[FruitInitialization.fruitIndexes[2]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[2]].name + ".").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    // Frutas (plural)
                    else
                    {
                        // Masculinas (Duraznos)
                        if (initializer.fruits[FruitInitialization.fruitIndexes[2]].name == "Durazno")
                        {
                            // Éstos son
                            source.PlayOneShot(maleNounsFeru[1]);
                            foreach(char letter in maleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1].length);
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // Duraznos
                            yield return new WaitForSeconds(pluralFruitsFeru[2].length);
                            source.PlayOneShot(pluralFruitsFeru[2]);
                            foreach(char letter in "duraznos.".ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }

                        // Femeninas (Manzanas, Peras)
                        else
                        {
                            // Éstas son
                            source.PlayOneShot(femaleNounsFeru[1]);
                            foreach(char letter in femaleNounsString[1].ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (número variable)
                            yield return new WaitForSeconds(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1].length);
                            source.PlayOneShot(numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1]);
                            foreach(char letter in (FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]].ToString() + " ").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }

                            // (frutas)
                            yield return new WaitForSeconds(pluralFruitsFeru[FruitInitialization.fruitIndexes[2]].length);
                            source.PlayOneShot(pluralFruitsFeru[FruitInitialization.fruitIndexes[2]]);
                            foreach(char letter in (initializer.fruits[FruitInitialization.fruitIndexes[2]].name + "s.").ToCharArray())
                            {
                                UIText.text += letter;
                                yield return new WaitForSeconds(typingSpeed);
                            }
                        }
                    }

                    break;

                // Mati
                case 3:
                    // Activate all fruits.
                    ActivateChildren();

                    // Set all other of the instantiated Fruit Panels' visibility to false.                    
                    /*
                    for (int i=0; i<typesOfFruits[0] + typesOfFruits[1]; i++)
                    {
                        currentFruitsPanel.transform
                                          .GetChild(i)
                                          .gameObject
                                          .SetActive(false);
                    }
                    */                    

                    /*
                    for (int i=typesOfFruits[0] + typesOfFruits[1]; i<totalFruits; i++)
                    {
                        currentFruitsPanel.transform
                                          .GetChild(i)
                                          .gameObject
                                          .SetActive(false);
                    }
                    */

                    // Set Text of the Current Fruits Indexes Object to the current number of fruits displayed.
                    currentFruitsIndexesText.text =  " 1";

                    for (int i=2; i<=totalFruits; i++)
                    {
                        currentFruitsIndexesText.text += "     " + i;
                    }

                    // "Juntas dan..."
                    source.PlayOneShot(audios[0]);
                    foreach(char letter in sentences[0].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    // (total de frutas)
                    yield return new WaitForSeconds(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1].length);
                    source.PlayOneShot(numbersMati[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]-1]);
                    foreach(char letter in ((FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]] + FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]]).ToString() + " ").ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    // "...frutas."
                    yield return new WaitForSeconds(audios[1].length);
                    source.PlayOneShot(audios[1]);
                    foreach(char letter in sentences[1].ToCharArray())
                    {
                        UIText.text += letter;
                        yield return new WaitForSeconds(typingSpeed);
                    }

                    //Debug.Log("Can you see this?");
                    break;

                /*
                case 4:
                    currentFruitsPanel.gameObject.SetActive(false);
                    currentFruitsIndexesText.gameObject.SetActive(false);
                    //currentFruitsIndexesObject.SetActive(false);
                    //endgame.SetActive(true);
                    break;  
                */                  
            }

            continueButton.SetActive(true);
            /*source.PlayOneShot(audios[index]);

            if (index == 0 || index == 2)
            {
                source.PlayOneShot(audios[0]);
                source.PlayOneShot((numbersFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]]) + 1);
                source.PlayOneShot((pluralFruitsFeru[FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]]]) + 1);
            }*/
        }

        else
        {
            source.Stop();
            source.PlayOneShot(audios[index]);

            foreach(char letter in sentences[index].ToCharArray())
            {
                UIText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            if (SceneManager.GetActiveScene().name == "7Game_Intro"){
                inicio.SetActive(true);
                continueButton.SetActive(true);
                gobackButton.SetActive(true);
                ReturntoFirstScene.SetActive(true);
                skip.SetActive(true);
            }
            else if (sceneIndex >= 15 && sceneIndex <= 21) //This chooses in which scenes activate the buttons in Game Intro.
            {
                ReturntoFirstScene.SetActive(true);
               // if (sceneIndex < 21)
                skip.SetActive(true);
                continueButton.SetActive(true);
                gobackButton.SetActive(true);
                //if (sceneIndex > 15) gobackButton.SetActive(true);
            }
            else if(SceneManager.GetActiveScene().name == "Tutorial"){
                if(index == 0){
                    skip.SetActive(true);
                    continueButton.SetActive(true);
                }
                else if(index == 2){
                   repetir.SetActive(true);
                   continueButton.SetActive(true); 
                }
                else
                continueButton.SetActive(true);
            }
            else {
                continueButton.SetActive(true);
                ReturntoFirstScene.SetActive(true);
                gobackButton.SetActive(true);
                skip.SetActive(true);
            }
        }
    }

    // Public due to it being referenced by Dialog Background's ContinueButton.
    public void NextSentence()
    {
        continueButton.SetActive(false);
        gobackButton.SetActive(false);
        skip.SetActive(false);
        ReturntoFirstScene.SetActive(false);

        if (SceneManager.GetActiveScene().name == "7Game_Intro" && index == 1){
            inicio.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "Tutorial" && index == 1){ // Esto solo sucede en el tutorial de matemáticas de conteo
                
                
                Pera.SetActive(true);
                Manzana2.SetActive(true);
                Manzana1.SetActive(true);
                Durazno1.SetActive(true);
                Durazno2.SetActive(true);
                source.enabled = false;
                handanimation.SetActive(true);
                dialogBackground.SetActive(false);
                
                
        }
        loader= GameObject.FindWithTag("Cross_Fade");
        if (SceneManager.GetActiveScene().name != "New Corrección")
        {
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

                if (SceneManager.GetActiveScene().name == "Agradecimiento"/* || SceneManager.GetActiveScene().name == "New Corrección"*/)
                    endgame.SetActive(true);
                else if (loader != null && loader.GetComponent<Levelloader>() != null)
                    loader.GetComponent<Levelloader>().LoadNextLevel(continuarpresionado);
                else
                    SceneManager.LoadScene(sceneIndex+1);
            }
        }
        else
        {
            if (index < 3)
            {
                index++;
                UIText.text = "";
                StartCoroutine(Type());
            }
            else
            {
                UIText.text = "";
                dialogBackground.SetActive(false);

                currentFruitsPanel.gameObject.SetActive(false);
                currentFruitsIndexesText.gameObject.SetActive(false);
                    //currentFruitsIndexesObject.SetActive(false);
                endgame.SetActive(true);

                /*if (SceneManager.GetActiveScene().name == "Agradecimiento"/* || SceneManager.GetActiveScene().name == "New Corrección")
                    endgame.SetActive(true);
                else
                    SceneManager.LoadScene(sceneIndex+1);
                */

            }
        }
    }
    public void PrevSentence()
    {
        continueButton.SetActive(false);
        gobackButton.SetActive(false);
        ReturntoFirstScene.SetActive(false);
        if (SceneManager.GetActiveScene().name == "7Game_Intro" && index == 1){
            inicio.SetActive(false);
        }
        continuarpresionado = false;
        skip.SetActive(false);
        loader= GameObject.FindWithTag("Cross_Fade");
        if (index >0)
        {
            index--;
            UIText.text = "";
            StartCoroutine(Type());
        }
        else
        {
            UIText.text = "";
            dialogBackground.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Agradecimiento"/* || SceneManager.GetActiveScene().name == "New Corrección"*/)
                endgame.SetActive(true);
            else if (loader != null && loader.GetComponent<Levelloader>() != null)
                loader.GetComponent<Levelloader>().LoadNextLevel(continuarpresionado);
            else
                SceneManager.LoadScene(sceneIndex-1);
        }
    }
    public void notstart(){ //Used for skiping tutorials or intros.
        if(SceneManager.GetActiveScene().name == "Tutorial"){
            SceneManager.LoadScene("Juego 1");
        }
        else if (sceneIndex >= 15 && sceneIndex <= 21){
            SceneManager.LoadScene("GameSelector");
        }
        else{
            SceneManager.LoadScene("Juego 1");
        }
    }

    public void repitScene(){
        SceneManager.LoadScene("Tutorial");
    }

    public void ReturntoFirstSceneofGame(){
       if (sceneIndex >= 15 && sceneIndex <= 21){
           SceneManager.LoadScene("1Game_Intro");
       }
       else{
           SceneManager.LoadScene("Introducción");
       }
    }

    void ActivateChildren()
    {
        for (int i=0; i<totalFruits; i++)
        {
            currentFruitsPanel.transform
                              .GetChild(i)
                              .gameObject
                              .SetActive(true);
        }
    }
}
