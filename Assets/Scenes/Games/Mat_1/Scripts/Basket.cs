// Basket/Slot attached to Panel. The smallest one; within the Panel which in such is within the greatest Panel.
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour, IDropHandler
{
    public static Basket sceneSclot;

    /* DATOS MIEMBRO
     * Variable de tipo 'Text' (UI) llamado "letter".
       Es público, ya que tiene referencia directa al Text del Movable (que a su vez es del Slot).
     * Contador entero inicializado en 0.
       Es estático, dado que si no lo fuese, 'totalReceivedFruits' no aumentaría de 1 por cada Slot que haya recibido un Movable exitosamente. 
     * Entero: Número de Slots del Recibidor.
     */ 
    public Text fruitsInBasketLabel;
    public static int totalReceivedFruits;// = 0;
    public int totalFruitsToBeReceived;
    //public bool lvlComplete;// = false;
    public FruitInitialization fruits;
    //public GameObject[] fruits;
    public AudioClip[] audios;
    public AudioSource source;
    //public GameObject endgame;

    public GameObject fruitA, fruitB;
    public int fruitsToBeReceivedA, fruitsToBeReceivedB;
    public static int receivedFruitsA, receivedFruitsB;

    Stack stack;

    void Start()
    {
        // Didn't allow it other way than an Object reference.
        totalReceivedFruits = 0;
        receivedFruitsA = 0;
        receivedFruitsB = 0;
        //totalFruitsToBeReceived = 3;//fruits.totalFruitsToBeReceived;           //SÓLO Para preuba        
        //lvlComplete = false;
        stack = new Stack();
    }

    /*void Update()
    {
        Debug.Log(transform.childCount);
    }*/

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
        /* If we don't have an item already:
         * (If we do, we don't want to accept it/them)
         * Otherwise, check if the letter of the item matches with the one accepted by this slot.
         */
        /* Evita que un Movable sea trasladado a un Slot del Banco de Letras SI ES QUE NO TIENE UNA REFERENCIA TIPO 'LETTER'
         * Sin embargo, esto termina causando errores a Unity.
         * Chance y sí sea necesario incluirle alguna letra aleatoria a los Slots del Banco de Letras, e incluir una condición donde nada más se permita hacer Drop de un 'itemB
           eingDragged' si el Slot tiene un Tag de tipo "Recibidor"
         * U, otra opción es simplemente quitarle esa referencia a los Slots del Banco de Letras en primer lugar.
           El problema es que comparte el mismo Script con los Slots del Recibidor, que NECESITAN esa referencia tipo Text.
         * ¿Habrá alguna manera de desabilitar una referencia de acuerdo al Objeto, incluso si se comparte el mismo Script?
         */

        /*if (item == null) //&& MatDragHandeler.itemBeingDragged.GetComponentInChildren<Text>().text == slotLetter)//letter.text) //&& MatDragHandeler.itemBeingDragged.GetComponentInParent<Object>.tag == "Recibidor")
        {*/
            //Debug.Log("Tag: " + MatDragHandeler.itemBeingDragged.tag);
            Debug.Log("Name: " + MatDragHandeler.itemBeingDragged.name);
            //stack.Push(MatDragHandeler.itemBeingDragged.tag);
            /* Grab the static var item being dragged.
             * -> Grab its Transform, and set its Parent to the current Transfrom.
             * = Each slot, if an item's dragged over it, and it doesn't have an item already, it will grab the item it's dropped on.
             */
            MatDragHandeler.itemBeingDragged.transform.SetParent(transform);
            MatDragHandeler.itemBeingDragged.GetComponent<MatDragHandeler>().flag = true;
            //MatDragHandeler.itemBeingDragged.GetComponent<Image>().color = Color.green;    //CAMBIO DE COLOR EXITOSO

            /*Basket.*/totalReceivedFruits++;
            fruitsInBasketLabel.text = totalReceivedFruits.ToString();
            source.PlayOneShot(audios[(totalReceivedFruits)-1]);
            
            if (MatDragHandeler.itemBeingDragged.name == fruitA.name)
                receivedFruitsA++;
            else
                receivedFruitsB++;
            //FindObjectOfType<GameManager>().Whatevs();    FUNCIONA. PROYECTA EL TEXTO DEL OBJETO <Text>

            /* Sólo permite el cambio de color para el primer "Recibidor" de todos los que hay.
             * No permite seleccionar varios objetos con el mismo tag a la vez; ni varios componentes, para hacer el cambio de color.
             * Además, más que el Slot, sería mejor que el "Movable" sea quien cambie de color.
             * Después habrá que ver si no se puede cambiar la saturación.
             */

            /* La condición funciona. Sin embargo, en lugar de igualar a 'totalReceivedFruits' a un número fijo, debería ser al número de recibidores que haya.
             * Ahí es donde entra <totalFruitsToBeReceived>
             * Ahora hace falta cambiar el color de los Slots del Recibidor a verde una vez que haya embonado (la condición larga arriba se haya cumplido)
             */

            // Hace falta reiniciarlo después de cada nivel --> Static? --> No es válido.
            //totalFruitsToBeReceived = GetComponent<Transform>().childCount; //EUREKA. FORMA SEGURA DE CONSEGUIR childCount.

            if (totalReceivedFruits == totalFruitsToBeReceived)
            {
                //GetComponent<TestScript>().endWord();
                //lvlComplete = true;
                //SceneManager.LoadScene(1);
                totalReceivedFruits = 0;

                //Debug.Log(stack.contents());

                Invoke("ChangeOfScene", 1.2f);
                //endgame.SetActive(true);          NOTA
            }

            /* ExecuteHierarchy calls EVERY Game Object above the one called along, until it finds something that can actually handle.
             * Pass "IHasChanged" interface.
             * Pass current GameObject to start with
             * Pass a "null" for data
             * Lambda function that will call method.
             */
            //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
            // Está atorado en 1...
            /*totalReceivedFruits = totalReceivedFruits+1;
            Debug.Log(totalReceivedFruits);*/
            //check = true;
        //} //return amount of children dropped
    }
    #endregion

    void ChangeOfScene()
    {
        /*for (int i=0; i<stack.Count; i++)
        {

        }
        string[] collectedFruits = stack.ToString();

        if (collectedFruits[0] == "Apple" && collectedFruits[1] =="Apple" && collectedFruits[2] == "Apple")*/
        if (receivedFruitsA == fruitsToBeReceivedA && receivedFruitsB == fruitsToBeReceivedB)
            SceneManager.LoadScene("Agradecimiento");//SceneManager.GetActiveScene().buildIndex+1);
        else
            SceneManager.LoadScene("Corrección");//10);                        
    }
    /*public void TestCall()
    {
        Debug.Log("<Slot> llamado exitosamente.");
    }*/
}
