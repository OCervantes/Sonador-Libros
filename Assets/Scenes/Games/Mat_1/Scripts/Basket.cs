/* Script in charge of 
 * Basket/Slot attached to Panel. The smallest one; within the Panel which in such is within the greatest Panel.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour, IDropHandler
{        
    public Text fruitsInBasketLabel;
    public FruitInitialization initializer;
    [SerializeField] AudioClip[] audios;
    [SerializeField] static int[] receivedFruits;

    AudioSource source;
    /*public static */GameObject[] fruits;

    // Static due to their reference to the Monologue and FruitInitialization Scripts, respectively.
    /*public*/ static int totalReceivedFruits;    
    /*public */static int[] fruitsToBeReceived;

    void Start()
    {
        receivedFruits = new int[3];        
        fruits = new GameObject[3];
        fruitsToBeReceived = new int[3];

        totalReceivedFruits = 0;
        receivedFruits[0] = 0;
        receivedFruits[1] = 0;
        receivedFruits[2] = 0;
        
        fruits[0] = initializer.fruits[FruitInitialization.fruitIndexes[0]];
        fruits[1] = initializer.fruits[FruitInitialization.fruitIndexes[1]];
        fruits[2] = initializer.fruits[FruitInitialization.fruitIndexes[2]];

        fruitsToBeReceived[0] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]];
        fruitsToBeReceived[1] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]];
        fruitsToBeReceived[2] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]];

        Debug.Log("Basket fruits[0], [1], [2]: " + fruits[0] + ", " + fruits[1] + ", " + fruits[2]);
        Debug.Log("Basket fruitsToBeReceived[0], [1], [2]: " + fruitsToBeReceived[0] + ", " + fruitsToBeReceived[1] + ", " + fruitsToBeReceived[2]);

        source = GetComponent<AudioSource>();
    }    

    /*
    public GameObject item
    {
        /* Properties:
         *  - Allow definition of custom methods of the (called)(?)
         *  - Treated as a variable outside of the script
         
        get
        {
            // If the item has a Child:
            if (transform.childCount > 0)
            {
                /* It will return the first child's Game Object
                 * GetChild() returns Transform
                 
                return transform.GetChild(0).gameObject;
            }
            // If it doesn't have one, it returns null.
            return null;
        }
    }
    */

    /* Interface used here is the IDropHandler
     * Drag Handlers on object being dragged
     * Drop Handlers go on to the receiving object.
     */
    #region IDropHandler implementation

    // Each time a Fruit is dragged to the basket:
    public void OnDrop(PointerEventData eventData)
    {                
        // Print the fruit's name.
        Debug.Log("Name: " + MatDragHandeler.itemBeingDragged.name);
        
        // Set the Basket as its parent.
        MatDragHandeler.itemBeingDragged.transform.SetParent(transform);
        //MatDragHandeler.itemBeingDragged.GetComponent<MatDragHandeler>().flag = true;


        /* Increment the counter of total fruits collected, print it in console, adjust the label of fruits collected,
           and play corresponding audio.
         */
        totalReceivedFruits++;
        Debug.Log("totalReceivedFruits = " + totalReceivedFruits);
        
        if (totalReceivedFruits == 1)
            fruitsInBasketLabel.text = totalReceivedFruits.ToString() + " fruta.";
        else
            fruitsInBasketLabel.text = totalReceivedFruits.ToString() + " frutas.";
                
        source.PlayOneShot(audios[(totalReceivedFruits)-1]);
        

        // Depending on the specific fruit that was dragged to the basket, increment its specific counter.
        if (MatDragHandeler.itemBeingDragged.name == fruits[0].name + "(Clone)")
        {
            receivedFruits[0]++;
            Debug.Log("receivedFruits[0] = " + receivedFruits[0]);
        }
        else if (MatDragHandeler.itemBeingDragged.name == fruits[1].name + "(Clone)")
        {
            receivedFruits[1]++;
            Debug.Log("receivedFruits[1] = " + receivedFruits[1]);
        }   
        else
        {
            receivedFruits[2]++;
            Debug.Log("receivedFruits[2] = " + receivedFruits[2]);    
        }
        
        /* Once all the requested AMOUNT of fruits has been collected:

         * Pass value to Monologue Script (in charge of "New" Scene).
         * Reset counter of total collected fruits.
         * Switch Scenes.
         */
        if (totalReceivedFruits == (fruitsToBeReceived[0] + fruitsToBeReceived[1] + fruitsToBeReceived[2]))
        { 
            // Static variables/functions from other classes can have their values assigned/be called as following:
            Monologue.fruits = totalReceivedFruits;

            totalReceivedFruits = 0;

            Invoke("SwitchScene", 1.2f);
        }

        /* ExecuteHierarchy calls EVERY Game Object above the one called along, until it finds something that can actually handle.
            * Pass "IHasChanged" interface.
            * Pass current GameObject to start with
            * Pass a "null" for data
            * Lambda function that will call method.
            */
        //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
    }
    #endregion

    void SwitchScene()
    {
        /* If the fruits that were collected are the same that were requested:
        
         * The FruitInitialization Script will generate new fruits to collect, once the Player comes back to the "Juego 1"
           Scene.
         * Player will be taken to the "New" Scene, in order to verify that they understand the abstract concept of num-
           bers.
         */
        if (receivedFruits[0] == fruitsToBeReceived[0] && receivedFruits[1] == fruitsToBeReceived[1] && receivedFruits[2] == fruitsToBeReceived[2])
        {
            /* Non-static variables/functions, on the other hand, must have an instance of their class declared in order
               to access them.
             */
            //initializer.SetFruitInitializationFlag(true);
            FruitInitialization.playerCollectedCorrectAmountOfFruits = true;
            SceneManager.LoadScene("New");
        }
        /* If not:

         * The FruitInitialization Script will generate the same fruits that were requested this time, in order to have
           the player practice with the fruits they did not collect correctly.
         * Player will be taken to the "New Corrección" Scene, in order to learn from their mistakes, and have another go
           at the game.           
         */
        else
        {
            //initializer.SetFruitInitializationFlag(false);
            FruitInitialization.playerCollectedCorrectAmountOfFruits = false;
            SceneManager.LoadScene("New Corrección");
        }
    }
}