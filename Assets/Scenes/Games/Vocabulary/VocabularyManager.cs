using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DocumentStore = System.Collections.Generic.Dictionary<string, object>;

public class VocabularyManager: MonoBehaviour,
    FirebaseManager.OnMissionDataRecievedCallback
{
    //Datos Miembro
    public AudioSource bounceSource, lockSource;
    public GameObject recibidor, banco, definition, prefabMovableAndSlot, prefabSlot;    
    /*public*/ string missionName;

    string recoveredVocabWord;
    int correctChildCount;

    /* Lo tuve que volver estático porque no me permitía de otra manera.
     * Tendré que revisar que no interfiera más tarde.
     */
    static int lowerRNGLimit, upperRNGLimit;    

    IDictionary<int, string> indexedDictionary;
    IDictionary<string, string> dictionary;
    Stack wordsAlreadyDisplayed;

    //Métodos
    public void Start()
    {        
        missionName = "hangman_1";        
        lowerRNGLimit = 1;
        upperRNGLimit = lowerRNGLimit + 2;

        indexedDictionary = new Dictionary<int, string>();
        dictionary = new Dictionary<string, string>();
        wordsAlreadyDisplayed = new Stack();

        FirebaseManager.GetMissionData(this, missionName);
    }    

    // Function called once data from Firebase has been received.
    public void InitializeGame()
    {        
        int i = 0;

        // indexedDictionary initialized with data: pairs of <index, word>
        foreach(KeyValuePair<string, string> pair in dictionary)
        {
            i++;

            indexedDictionary.Add(i, pair.Key);            
        }

        // Once both Dictionaries have been initialized, build the game.
        BuildGame();        
    }


    void BuildGame()
    {    
        // If the game hasn't yet displayed the words within the range defined by [lowerRNGLimit, upperRNGLimit], do so.
        if (!(wordsAlreadyDisplayed.Contains(lowerRNGLimit) && wordsAlreadyDisplayed.Contains(lowerRNGLimit+1) && wordsAlreadyDisplayed.Contains(lowerRNGLimit+2)))
        {            
            InstanceGameObjects();
        }

        // If the game HAS displayed the words within said range, adjust it so that it may consider the next 3 words.
        else
        {
            lowerRNGLimit = lowerRNGLimit + 3;
            upperRNGLimit = lowerRNGLimit + 2;
            
            InstanceGameObjects();
        }
    }

    /* Fetch a random word from the dictionaries, instantiate all Slot and MovAndSlot Objects necessary for said word,
       and display said word's definition.     
     */
    void InstanceGameObjects()
    {
        int randomlyGeneratedIndex;

        // Generate a random number in range of lowerRNGLimit (initially 1) and upperRNGLimit (initially 3)
        do
        {
            randomlyGeneratedIndex = Random.Range(lowerRNGLimit, upperRNGLimit+1);
        }
        while (wordsAlreadyDisplayed.Contains(randomlyGeneratedIndex));
        

        // Pick word from dictionary corresponding to said random number.        
        recoveredVocabWord = indexedDictionary[randomlyGeneratedIndex];
        Debug.Log("Index: " + randomlyGeneratedIndex + "\nRecovered Vocab Word: " + recoveredVocabWord);

        /* Add randomly generated number to Stack, to know which index --> word has already been displayed, in order to
           not display it again.
         */
        wordsAlreadyDisplayed.Push(randomlyGeneratedIndex);

        // Instantiate all necessary MovAndSlot and Slot objects.
        for (int n=0; n<(recoveredVocabWord.Length); n++)
        {
            GameObject MovAndSlotObjRef = Instantiate(prefabMovableAndSlot, banco.transform);
            MovAndSlotObjRef.GetComponentInChildren<Text>().text = System.Convert.ToString(recoveredVocabWord[n]).ToUpper();

            GameObject SlotObjRef = Instantiate(prefabSlot, recibidor.transform);
            Slot SlotObj = SlotObjRef.GetComponent<Slot>();            
            SlotObj.slotLetter = System.Convert.ToString(recoveredVocabWord[n]).ToUpper();
            SlotObj.vocabManager = this;
        }

        Stack letterIndexes = new Stack();

        /* Displays the number of children 
         * Curiously, from the second word onwards, it returns the current number of children + the previous iteration's
           number of children.
         */
        Debug.Log("Number of children: " + banco.transform.childCount);        

        // Rearrange position of MovAndSlot objects.
        for (int m=0; m<(recoveredVocabWord.Length); m++)
        {            
            int indexOfLetter;

            /* Generate another random number. This time, to represent each MovAndSlot's new position in banco.
             * It is important that said random number is different from the current MovAndSlot's origiaal position.
             */
            do
            {
                indexOfLetter = Random.Range(0, recoveredVocabWord.Length);                
            }
            while (letterIndexes.Contains(indexOfLetter) && indexOfLetter != m);

            Debug.Log("Random Letter Index: " + indexOfLetter);

            /* If this is the first word in the game, get each of banco's children, and assign it's new position to the
               randomly generated number.
             */
            if (wordsAlreadyDisplayed.Count == 1)
            {
                if (banco.transform
                         .GetChild(m)
                         .GetComponentInChildren<Text>() != null)
                    Debug.Log("Child: " + m + "\nLetter: " + banco.transform
                                                                  .GetChild(m)
                                                                  .GetChild(0)
                                                                  .GetComponentInChildren<Text>()
                                                                  .text);
                
                banco.transform
                     .GetChild(m)
                     .GetComponent<Slot>()
                     .transform
                     .SetSiblingIndex(indexOfLetter);
            }
            /* If this is the 2nd+ word in the game, 
             */
            else
            {
                if (banco.transform
                         .GetChild(m+correctChildCount)
                         .GetComponentInChildren<Text>() != null)
                    Debug.Log("Child: " + (m+correctChildCount) + "\nLetter: " + banco.transform
                                                                                      .GetChild(m+correctChildCount)
                                                                                      .GetChild(0)
                                                                                      .GetComponentInChildren<Text>()
                                                                                      .text);

                banco.transform
                     .GetChild(m+correctChildCount)
                     .GetComponent<Slot>()
                     .transform
                     .SetSiblingIndex(indexOfLetter);                    
            }     
            
            letterIndexes.Push(indexOfLetter);             
        }                

        // Display corresponding word's definition.
        definition.GetComponent<Text>().text = dictionary[recoveredVocabWord];
    }    

    // Called when firebase manager has a response to the API method called
    public void MissionDataRecieved(FirebaseManager.CallbackResult result,
       MissionData? data, string message) 
    {
        // Try to handle all cases
        switch(result) 
        {
            // If the result is not success, just print to console as error
            case FirebaseManager.CallbackResult.Canceled:
            case FirebaseManager.CallbackResult.Faulted:
            case FirebaseManager.CallbackResult.Invalid:
                Debug.LogError(message);
                break;
            // If successful, use recieved mission data to populate scene
            case FirebaseManager.CallbackResult.Success:
            default:
                Debug.Log(message);
                UnityMainThreadDispatcher
                    .Instance ()
                    .Enqueue(WordLoaderWrapper(data));
                break;
        }
    }

    // Wrapper function to call WorldLoader as an IEnumerator
    // to work with UnityMainThreadDispatcher.
    IEnumerator WordLoaderWrapper(MissionData? data) 
    {
        WordLoader(data.Value.Data);
        yield return null;
    }

    // Parsed data from Firebase into data that this scene can use.
    private void WordLoader(DocumentStore data) 
    {        

        List<object> definitions = data["definitions"] as List<object>;

        // Iterate over all objects found under definitions
        for (int i = 1; i < definitions.Count; i++) 
        {
            // Attempt to parse the object as another subpage
            DocumentStore levels = definitions[i] as DocumentStore;
            // and iterate over all its children
            foreach (KeyValuePair<string, object> level in levels) 
            {
                string[] pair = new string[2];
                pair[0] = level.Key;
                pair[1] = (string) level.Value;

                // dictionary initialized with data: pairs of <word, definition>                 
                dictionary.Add(pair[0], pair[1]);
            }
        }        

        // Now that we have the data & its properly parsed, initalize game.
        InitializeGame();
    }
    
    public void EndWord()
    {
        foreach (Transform child in banco.transform)
        {
            GameObject.Destroy(child.gameObject);
            Transform.Destroy(child);
        }

        foreach (Transform child in recibidor.transform)
        {
            GameObject.Destroy(child.gameObject);
            Transform.Destroy(child);
        }

        definition.GetComponent<Text>().text = "";

        correctChildCount = banco.transform.childCount;
        Debug.Log("Number of children AFTER EndWord(): " + banco.transform.childCount);
        //correctChildCount = banco.transform.childCount;
        
        BuildGame();
    }    
}