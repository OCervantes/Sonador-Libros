/* Script in charge of 
 * Basket/Slot attached to Panel. The smallest one; within the Panel which in such is within the greatest Panel.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour, IDropHandler
{        
    //public Text fruitsInBasketLabel;
    public FruitInitialization initializer;
    public GameObject[] fruitPanels;
    public GameObject tableclothPanel;
    //[SerializeField] AudioClip[] audios;
    [SerializeField] static int[] receivedFruits;

    //AudioSource //source;
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
        // Print the fruit's name in console.
        string fruitType = MatDragHandeler.itemBeingDragged.name;
        Debug.Log("Name: " + fruitType);        

        // Depending on the specific fruit that was dragged to the basket, increment its specific counter.        
        if (fruitType == fruits[0].name + "(Clone)")
        {
            ProcessFruit(0);            
        }
        else if (fruitType == fruits[1].name + "(Clone)")
        {
            ProcessFruit(1);            
        }   
        else
        {
            ProcessFruit(2);
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

    void ProcessFruit(int fruitIndex)
    {
        //If there are still fruits to be received, for this specific type of fruit:
        if (receivedFruits[fruitIndex] < fruitsToBeReceived[fruitIndex])
        {
            // Increment counter of received fruits of said kind
            receivedFruits[fruitIndex]++;
            Debug.Log("receivedFruits[" + fruitIndex + "] = " + receivedFruits[fruitIndex]);

            // Instantiate Fruit Panel within corresponding Panel
            GameObject newFruitPanel = Instantiate(fruitPanels[FruitInitialization.fruitIndexes[fruitIndex]], tableclothPanel.transform);
            newFruitPanel.transform.SetParent(tableclothPanel.transform, false);

            // Eliminate fruit being dragged to the tablecloth
            GameObject draggedFruit = MatDragHandeler.itemBeingDragged;
            Destroy(draggedFruit);

            // Increment the counter of total fruits collected, and print it in console
            totalReceivedFruits++;
            Debug.Log("totalReceivedFruits = " + totalReceivedFruits);            
        }

        // Otherwise...
        else
        {
            // Display "Uh Oh" via Popup as a warning message for the user
            Debug.Log("Uh Oh!");
            Popup popup = UIController.instance.CreatePopup();

            popup.ShowPopup(UIController.instance.mainCanvas, "Uh Oh!");
        }
    }

    // Public in order to be referenced by the Inspector.
    public void LoadNextScene()
    {
        // If every requested fruit was collected:
        if (receivedFruits[0] == fruitsToBeReceived[0] && 
            receivedFruits[1] == fruitsToBeReceived[1] && 
            receivedFruits[2] == fruitsToBeReceived[2])
        {
            // The FruitInitialization Script will generate new fruits to collect, once the Player comes back to the "Juego 1"Scene.
            FruitInitialization.playerCollectedCorrectAmountOfFruits = true;

            // Monologue, script working at "New" Scene, is given the value of the number of received fruits.
            Monologue.fruits = totalReceivedFruits;

            // Player will be taken to the "New" Scene, in order to verify that they understand the abstract concept of numbers.            
            SceneManager.LoadScene("New");
        }

        // Otherwise...         
        else
        {
            /* The FruitInitialization Script will generate the same fruits that were requested this time, in order to have the player 
               practice with the fruits they did not collect correctly.
             */  
            FruitInitialization.playerCollectedCorrectAmountOfFruits = false;

            // Player will be taken to the "New Corrección" Scene, in order to learn from their mistakes, and have another go at the game.
            SceneManager.LoadScene("New Corrección");
        }
    }
}