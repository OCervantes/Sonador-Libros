using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Animations: MonoBehaviour
{
    public GameObject dialogAndAudioMan;
    public GameObject[] personajes;
    int scene;

    /* DialogIndex, as its name implies, refers to the integer 'index' from script Dialog.
     * Given that it is not static (it updates continuously), it cannot be initialized at its declaration.
     * Instead, it must be assigned within Update(), due to its constant execution, once per frame.
     */
    private int DialogIndex;// = dialogAndAudioMan.GetComponent<Dialog>().index;

    void Start() 
    {        
        scene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {        
        DialogIndex = dialogAndAudioMan.GetComponent<Dialog>().index;

        switch(scene)
        {
            // Escena: "Introducción"
            case 6:

                // Same condition as in Dialog. Checks once the sentence has finished "typing".                                   
                if (dialogAndAudioMan.GetComponent<Dialog>().UIText.text == dialogAndAudioMan.GetComponent<Dialog>().sentences[DialogIndex])
                {
                    /* However, instead of displaying a button, it executes a switch().
                     * Depending on the index, the switch() will disable the animation of the character who has finished
                       talking.
                     */
                    switch (DialogIndex)
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
                switch(/*dialogAndAudioMan.GetComponent<Dialog>().index*/DialogIndex)
                { 
                    /*case 0:
                        personajes[0].SetActive(true);
                        break;*/
                    case 1:
                        personajes[0].SetActive(false);
                        personajes[1].SetActive(true);
                        break;
                    case 2:
                        personajes[1].SetActive(false);
                        personajes[0].SetActive(true);
                        break;
                    case 3:
                        personajes[0].SetActive(false);
                        personajes[2].SetActive(false);
                        personajes[3].SetActive(true);
                        break;
                }
                break;            
            

            // Scene "Selección de Nivel"
            case 7:

                // Same condition as in Dialog. Checks once the sentence has finished "typing".                                   
                if (dialogAndAudioMan.GetComponent<Dialog>().UIText.text == dialogAndAudioMan.GetComponent<Dialog>().sentences[DialogIndex])
                {
                    /* However, instead of displaying a button, it executes a switch().
                     * Depending on the index, the switch() will disable the animation of the character who has finished
                       talking.
                     */
                    switch (DialogIndex)
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

                        /*case 2:
                            // Feru stops talking.
                            personajes[0].SetActive(false); 
                            break;
                            */
                    }
                    break;
                }

                /* Enables and disables the animations between all characters who speak in the scene, according to the sen-
                   tence that is currently being displayed in Dialogue Text.
                 */
                switch(/*dialogAndAudioMan.GetComponent<Dialog>().index*/DialogIndex)
                { 
                    /*case 0:
                        personajes[0].SetActive(true);
                        break;*/
                    case 1:
                        personajes[0].SetActive(true);
                        personajes[1].SetActive(false);
                        break;
                    /*case 2:
                        personajes[1].SetActive(false);
                        personajes[0].SetActive(true);
                        break;
                    case 3:
                        personajes[0].SetActive(false);
                        personajes[2].SetActive(false);
                        personajes[3].SetActive(true);
                        break;
                    */
                }
                break;    

            // Scene "Agradecimiento"
            case 9:
                switch(/*dialogAndAudioMan.GetComponent<Dialog>().index*/DialogIndex)
                {
                    /*case 0:
                        personajes[0].SetActive(true);
                        break;*/
                    case 1:
                        personajes[0].SetActive(false);
                        personajes[1].SetActive(true);
                        break;
                }
                break;            
        }
    }
}
