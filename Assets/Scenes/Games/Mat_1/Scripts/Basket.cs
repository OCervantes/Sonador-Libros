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

    public GameObject item
    {
        /* Properties:
         *  - Allow definition of custom methods of the (called)(?)
         *  - Treated as a variable outside of the script
         */
        get
        {
            // If the item has a Child:
            if (transform.childCount > 0)
            {
                /* It will return the first child's Game Object
                 * GetChild() returns Transform
                 */
                return transform.GetChild(0).gameObject;
            }
            // If it doesn't have one, it returns null.
            return null;
        }
    }

    /* Interface used here is the IDropHandler
     * Drag Handlers on object being dragged
     * Drop Handlers go on to the receiving object.
     */
    #region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {        
        /*if (item == null) //&& MatDragHandeler.itemBeingDragged.GetComponentInChildren<Text>().text == slotLetter)//letter.text) //&& MatDragHandeler.itemBeingDragged.GetComponentInParent<Object>.tag == "Recibidor")
        {*/
            Debug.Log("Name: " + MatDragHandeler.itemBeingDragged.name);
            /* Grab the static var item being dragged.
             * -> Grab its Transform, and set its Parent to the current Transfrom.
             * = Each slot, if an item's dragged over it, and it doesn't have an item already, it will grab the item it's dropped on.
             */
            MatDragHandeler.itemBeingDragged.transform.SetParent(transform);
            //MatDragHandeler.itemBeingDragged.GetComponent<MatDragHandeler>().flag = true;

            totalReceivedFruits++;

            if (totalReceivedFruits == 1)
                fruitsInBasketLabel.text = totalReceivedFruits.ToString() + " fruta.";
            else
                fruitsInBasketLabel.text = totalReceivedFruits.ToString() + " frutas.";
                
            source.PlayOneShot(audios[(totalReceivedFruits)-1]);
            
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

            Debug.Log("totalReceivedFruits = " + totalReceivedFruits);
            if (totalReceivedFruits == (fruitsToBeReceived[0] + fruitsToBeReceived[1] + fruitsToBeReceived[2]))
            { 
                // ÉXITO
                // Passes value
                Monologue.fruits = totalReceivedFruits;//totalFruitsToBeReceived;

                totalReceivedFruits = 0;

                Invoke("ChangeOfScene", 1.2f);
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

    void ChangeOfScene()
    {        
        if (receivedFruits[0] == fruitsToBeReceived[0] && receivedFruits[1] == fruitsToBeReceived[1] && receivedFruits[2] == fruitsToBeReceived[2])
            SceneManager.LoadScene("New");
        else
            SceneManager.LoadScene("New Corrección");
    }
}
