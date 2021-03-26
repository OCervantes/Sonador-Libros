using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    //public static Slot sceneSclot;

    /* DATOS MIEMBRO
     * Variable de tipo 'Text' (UI) llamado "letter".
       Es público, ya que tiene referencia directa al Text del Movable (que a su vez es del Slot).
     * Contador entero inicializado en 0.
       Es estático, dado que si no lo fuese, 'embeddedMovCounter' no aumentaría de 1 por cada Slot que haya recibido un Movable exitosamente.
     * Entero: Número de Slots del Recibidor.
     */
    //public Text letter;
    public string slotLetter;
    public static int embeddedMovCounter = 0;
    int numberOfChildren;
    //public bool lvlComplete = false;
    public VocabularyManager vocabManager;    

    void Start() 
    {
        embeddedMovCounter = 0;
    }

    /* Variable de tipo GameObject llamado "item"
     * Avisa si cada Slot contiene una letra o no (null)
     */
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
         *
         * Evita que un Movable sea trasladado a un Slot del Banco de Letras SI ES QUE NO TIENE UNA REFERENCIA TIPO 'LETTER'
         * Sin embargo, esto termina causando errores a Unity.
         * Chance y sí sea necesario incluirle alguna letra aleatoria a los Slots del Banco de Letras, e incluir una condición donde nada más se permita hacer Drop de un 'itemB
           eingDragged' si el Slot tiene un Tag de tipo "Recibidor"
         * U, otra opción es simplemente quitarle esa referencia a los Slots del Banco de Letras en primer lugar.
           El problema es que comparte el mismo Script con los Slots del Recibidor, que NECESITAN esa referencia tipo Text.
         * ¿Habrá alguna manera de desabilitar una referencia de acuerdo al Objeto, incluso si se comparte el mismo Script?
         */

        Text textComponent = DragHandeler
            .itemBeingDragged
            .GetComponentInChildren<Text>();

        if (item == null &&
           textComponent.text == slotLetter)
           //letter.text) //&& DragHandeler.itemBeingDragged.GetComponentInParent<Object>.tag == "Recibidor")
        {
            /* Grab the static var item being dragged.
             * -> Grab its Transform, and set its Parent to the current Transfrom.
             * = Each slot, if an item's dragged over it, and it doesn't have an item already, it will grab the item it's dropped on.
             */
            DragHandeler
                .itemBeingDragged
                .transform
                .SetParent(transform);
            DragHandeler
                .itemBeingDragged
                .GetComponent<DragHandeler>()
                .flag = true;
            DragHandeler
                .itemBeingDragged
                .GetComponent<Image>()
                .color = Color.green;   //CAMBIO DE COLOR EXITOSO

            /*Slot.*/embeddedMovCounter++;

            vocabManager.lockSource.Play();

            //FindObjectOfType<VocabularyManager>().Whatevs();    FUNCIONA. PROYECTA EL TEXTO DEL OBJETO <Text>

            /* Sólo permite el cambio de color para el primer "Recibidor" de todos los que hay.
             * No permite seleccionar varios objetos con el mismo tag a la vez; ni varios componentes, para hacer el cambio de color.
             * Además, más que el Slot, sería mejor que el "Movable" sea quien cambie de color.
             * Después habrá que ver si no se puede cambiar la saturación.
             *
             * La condición funciona. Sin embargo, en lugar de igualar a 'embeddedMovCounter' a un número fijo, debería ser al número de recibidores que haya.
             * Ahí es donde entra <numberOfChildren>
             * Ahora hace falta cambiar el color de los Slots del Recibidor a verde una vez que haya embonado (la condición larga arriba se haya cumplido)
             */

            // Hace falta reiniciarlo después de cada nivel --> Static? --> No es válido.
            numberOfChildren = GameObject
                .Find("Recibidor")
                .GetComponent<Transform>()
                .childCount; //EUREKA. FORMA SEGURA DE CONSEGUIR childCount.

            //Debug.Log("Number of children in Recibidor: " + numberOfChildren);

            if (embeddedMovCounter == numberOfChildren)
            {
                vocabManager.EndWord();
                embeddedMovCounter = 0;
            }

            /* ExecuteHierarchy calls EVERY Game Object above the one called along, until it finds something that can actually handle.
             * Pass "IHasChanged" interface.
             * Pass current GameObject to start with
             * Pass a "null" for data
             * Lambda function that will call method.
             */
            //ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
            // Está atorado en 1...
            /*embeddedMovCounter = embeddedMovCounter+1;
            Debug.Log(embeddedMovCounter);*/
            //check = true;
        } //return amount of children dropped
        else if (item == null)
        {
            Image imageComponent = DragHandeler
                .itemBeingDragged
                .GetComponent<Image>();
            imageComponent.color = Color.yellow;
            StartCoroutine(IncorrectSlotCoroutine(imageComponent));
        }
    }
    #endregion

    private IEnumerator IncorrectSlotCoroutine(Image component) 
    {
        vocabManager.bounceSource.Play();
        yield return new WaitForSeconds(1);
        component.color = new Color(1f, 1f, 1f, 0.39f);
    }    
    //Cuando el usuario quiere un Hint
    //Hacer metodo que reciva los gameobject the moveandSlot y compare el texto de esto con el texto del slot.
    public void MoveCardToCorrectSlot(GameObject[] movableSlots)
    {
        GameObject Card, CardText;
        
        foreach (GameObject cardSlot in movableSlots)
        {
            Card = cardSlot.transform.GetChild(0).gameObject;
            CardText = Card.transform.GetChild(0).gameObject;
            Text textComponent = CardText.GetComponent<Text>();

            if (textComponent.text == slotLetter)
            {
               
                //Card.transform.position = this.transform.position;
                Card
                .transform
                .SetParent(transform);
                Card.GetComponent<Image>()
                    .color = Color.green;   //CAMBIO DE COLOR EXITOSO

                /*Slot.*/embeddedMovCounter++;

                vocabManager.lockSource.Play();

                
                numberOfChildren = GameObject
                    .Find("Recibidor")
                    .GetComponent<Transform>()
                    .childCount; //EUREKA. FORMA SEGURA DE CONSEGUIR childCount.

                //Debug.Log("Number of children in Recibidor: " + numberOfChildren);

                if (embeddedMovCounter == numberOfChildren)
                {
                    vocabManager.EndWord();
                    embeddedMovCounter = 0;
                }

            } 
        }
    }
}