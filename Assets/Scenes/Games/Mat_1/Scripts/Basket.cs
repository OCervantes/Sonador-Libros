// Basket/Slot attached to Panel. The smallest one; within the Panel which in such is within the greatest Panel.
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour, IDropHandler
{    
    /* DATOS MIEMBRO
     * Variable de tipo 'Text' (UI) llamado "letter".
       Es público, ya que tiene referencia directa al Text del Movable (que a su vez es del Slot).
     * Contador entero inicializado en 0.
       Es estático, dado que si no lo fuese, 'totalReceivedFruits' no aumentaría de 1 por cada Slot que haya recibido un Movable exitosamente. 
     * Entero: Número de Slots del Recibidor.
     */ 
    public Text fruitsInBasketLabel;
    public static int totalReceivedFruits;
    //int totalFruitsToBeReceived;    
    [SerializeField] AudioClip[] audios;
    AudioSource source;
    public GameObject fruitA, fruitB;
    [SerializeField] int fruitsToBeReceivedA, fruitsToBeReceivedB;
    [SerializeField] static int receivedFruitsA, receivedFruitsB;

    void Start()
    {
        totalReceivedFruits = 0;
        //totalFruitsToBeReceived = fruitsToBeReceivedA + fruitsToBeReceivedB;
        receivedFruitsA = 0;
        receivedFruitsB = 0;        
        source = GetComponent<AudioSource>();
    }

    public int getFruits()
    {
        return totalReceivedFruits;
    }

    /* Moved to FruitInitialization
     * void Start()
    {
        totalFruitsToBeReceived = fruits.Length;

        for (int i=0; i<totalFruitsToBeReceived; i++)
        {
            GameObject newFruit = Basket.Instantiate(fruits[i], randomCoordinates(), Quaternion.identity, transform);
            newFruit.transform.localScale = new Vector3(0.05f, 0.05f, 0.0f);
        }

        Debug.Log("Number of Fruits: " + totalFruitsToBeReceived);
    }
 
    /* Variable de tipo GameObject llamado "item"
     * Avisa si cada Slot contiene una letra o no (null) 
     */

    /*Vector3 randomCoordinates()
    {
        float x = Random.Range(-3, 3);
        float y = Random.Range(1, 4);
        float z = -2f;

        return new Vector3(x, y, z);
    }*/

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
            MatDragHandeler.itemBeingDragged.GetComponent<MatDragHandeler>().flag = true;

            totalReceivedFruits++;
            fruitsInBasketLabel.text = totalReceivedFruits.ToString();
            source.PlayOneShot(audios[(totalReceivedFruits)-1]);
            
            if (MatDragHandeler.itemBeingDragged.name == fruitA.name)
                receivedFruitsA++;
            else
                receivedFruitsB++;            

            if (totalReceivedFruits == /*totalFruitsToBeReceived*/(fruitsToBeReceivedA + fruitsToBeReceivedB))
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
        if (receivedFruitsA == fruitsToBeReceivedA && receivedFruitsB == fruitsToBeReceivedB)
            SceneManager.LoadScene("New");//"Agradecimiento");//SceneManager.GetActiveScene().buildIndex+1);
        else
            SceneManager.LoadScene("New Corrección");//10);                        
    }
}
