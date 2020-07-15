//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class Animations: MonoBehaviour
{
    public GameObject dialogAndAudioMan;
    public GameObject[] personajes;

    /* CurrentDialogIndex, as its name implies, refers to the integer 'index' from script Dialog.
     * Given that it is not static (it updates continuously), it cannot be initialized at its declaration.
     * Instead, it must be assigned within Update(), due to its constant execution, once per frame.
     */
    string scene;
    int index;//InitialDialogIndex, CurrentDialogIndex;
    string DialogText;
    string[] DialogSentences;    

    void Start() 
    {
        scene = SceneManager.GetActiveScene().name;
        /*InitialDialogIndex = dialogAndAudioMan.GetComponent<Dialog>().index;
        CurrentDialogIndex = InitialDialogIndex;
        DialogText = dialogAndAudioMan.GetComponent<Dialog>().UIText.text;
        DialogSentences = dialogAndAudioMan.GetComponent<Dialog>().sentences;        
        */
    }

    // Update is called once per frame
    void Update()
    {
        index = dialogAndAudioMan.GetComponent<Dialog>().index;
        //Debug.Log("Index: " + Dialog.index + "\nSentence: " + Dialog.sentences[Dialog.index]);        
        //CurrentDialogIndex = dialogAndAudioMan.GetComponent<Dialog>().index;

        //DialogText = dialogAndAudioMan.GetComponent<Dialog>().UIText.text;
        /*if (CurrentDialogIndex != InitialDialogIndex)
        {
            InitialDialogIndex = CurrentDialogIndex;
            DialogText = dialogAndAudioMan.GetComponent<Dialog>().UIText.text;
            DialogSentences = dialogAndAudioMan.GetComponent<Dialog>().sentences;
        }*/
        //CurrentDialogIndex = dialogAndAudioMan.GetComponent<Dialog>().index;  
        /*DialogText = dialogAndAudioMan.GetComponent<Dialog>().UIText.text;
        DialogSentences = dialogAndAudioMan.GetComponent<Dialog>().sentences;*/

        switch(scene)
        {
            // Escena: "Introducción"
            case "Introducción":

                // Same condition as in Dialog. Checks once the sentence has finished "typing".                                   
                if (/*DialogText == /*Dialog.sentences[Dialog.index])DialogSentences[CurrentDialogIndex])*/dialogAndAudioMan.GetComponent<Dialog>().UIText.text == dialogAndAudioMan.GetComponent<Dialog>().sentences[index])
                {
                    /* However, instead of displaying a button, it executes a switch().
                     * Depending on the index, the switch() will disable the animation of the character who has finished
                       talking.
                     */
                    switch (/*Dialog.index)CurrentDialogIndex*/index)
                    {
                        case 0:
                            // Feru stops talking.
                            personajes[0].SetActive(false);
                            break;
                        case 1:
                            // Mati stops talking.
                            personajes[1].SetActive(false);
                            break;
                        case 2:
                            // Feru stops talking.
                            personajes[0].SetActive(false); 
                            break;
                    }
                    break;
                }

                /* Enables and disables the animations between all characters who speak in the scene, according to the sen-
                   tence that is currently being displayed in Dialogue Text.
                 */
                switch(/*Dialog.index)CurrentDialogIndex*/index)
                {                                         
                    case 1:
                        // 
                        personajes[0].SetActive(false);
                        personajes[1].SetActive(true);
                        break;
                    case 2:
                        // 
                        personajes[1].SetActive(false);
                        personajes[0].SetActive(true);
                        break;                        
                    case 3:
                        //
                        personajes[0].SetActive(false);
                        personajes[2].SetActive(false);
                        personajes[3].SetActive(true);
                        break;
                }
                break;            
            

            // Scene "Selección de Nivel"
            case "Selección de Nivel":

                // Same condition as in Dialog. Checks once the sentence has finished "typing".                                   
                if (/*DialogText == Dialog.sentences[Dialog.index])DialogSentences[CurrentDialogIndex])*/dialogAndAudioMan.GetComponent<Dialog>().UIText.text == dialogAndAudioMan.GetComponent<Dialog>().sentences[index])
                {
                    /* However, instead of displaying a button, it executes a switch().
                     * Depending on the index, the switch() will disable the animation of the character who has finished
                       talking.
                     */
                    switch (/*Dialog.index)CurrentDialogIndex*/index)
                    {
                        case 0:
                            // 
                            personajes[0].SetActive(false);
                            personajes[1].SetActive(true);
                            break;

                        case 1:
                            // Mati stops talking.
                            personajes[0].SetActive(false);
                            personajes[1].SetActive(true);
                            break; 
                    }
                    break;
                }

                /* Enables and disables the animations between all characters who speak in the scene, according to the sen-
                   tence that is currently being displayed in Dialogue Text.
                 */
                switch(/*Dialog.index)CurrentDialogIndex*/index)
                {  
                    case 1:
                        // 
                        personajes[0].SetActive(true);
                        personajes[1].SetActive(false);
                        break;  
                }
                break;                
            
            // Scene "Agradecimiento"
            case "Agradecimiento":

                // Same condition as in Dialog. Checks once the sentence has finished "typing".                                   
                if (/*DialogText == Dialog.sentences[Dialog.index])DialogSentences[CurrentDialogIndex])*/dialogAndAudioMan.GetComponent<Dialog>().UIText.text == dialogAndAudioMan.GetComponent<Dialog>().sentences[index])
                {
                    /* However, instead of displaying a button, it executes a switch().
                     * Depending on the index, the switch() will disable the animation of the character who has finished
                       talking.
                     */
                    switch (/*Dialog.index)CurrentDialogIndex*/index)
                    {
                        case 0:
                            // Tepo stops talking and stops raising his hand.
                            personajes[0].SetActive(false);
                            personajes[1].SetActive(true);
                            break; 
                    }
                    break;
                }

                /* Enables and disables the animations between all characters who speak in the scene, according to the sen-
                   tence that is currently being displayed in Dialogue Text.
                 */
                switch(/*Dialog.index)CurrentDialogIndex*/index)
                {
                    case 1:
                        // 
                        personajes[0].SetActive(false);
                        personajes[1].SetActive(true);
                        break;
                }
                break;   

            // Scene "New Corrección"
            case "New Corrección":
                bool finished = false;

                // Same condition as in Dialog. Checks once the sentence has finished "typing".                                   
                if (/*DialogText == Dialog.sentences[Dialog.index])DialogSentences[CurrentDialogIndex])*/dialogAndAudioMan.GetComponent<Dialog>().UIText.text.EndsWith(".")) // == dialogAndAudioMan.GetComponent<Dialog>().sentences[index])
                {
                    /* However, instead of displaying a button, it executes a switch().
                     * Depending on the index, the switch() will disable the animation of the character who has finished
                       talking.
                     */
                    switch (/*Dialog.index)CurrentDialogIndex*/index)
                    {
                        case 0:
                            // 
                            personajes[0].SetActive(false);
                            //personajes[1].SetActive(true);
                            break;

                        case 1:
                            // Mati stops talking.
                            //personajes[0].SetActive(false);
                            personajes[1].SetActive(false);
                            break; 

                        case 2:
                            // Feru stops talking.
                            personajes[0].SetActive(false);
                            finished = true;
                            //personajes[1].SetActive(true);
                            break;

                        case 3:
                            // Mati stops talking.
                            personajes[1].SetActive(false);
                            finished = true;
                            break;
                    }
                    break;
                }

                /* Enables and disables the animations between all characters who speak in the scene, according to the sen-
                   tence that is currently being displayed in Dialogue Text.
                 */
                switch(/*Dialog.index)CurrentDialogIndex*/index)
                {
                    case 0:
                        personajes[0].SetActive(true);
                        break;

                    case 1:
                        // 
                        //personajes[0].SetActive(true);
                        personajes[1].SetActive(true);
                        break;  

                    case 2:
                        //                         
                        personajes[0].SetActive(true);
                        break;

                    case 3:
                        if (finished == false)
                            personajes[1].SetActive(true);
                        else
                            personajes[1].SetActive(false);
                        break;
                }
                break;
        }
    }
}
