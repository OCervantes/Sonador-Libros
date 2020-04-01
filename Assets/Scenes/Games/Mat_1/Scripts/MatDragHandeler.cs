// Script que se encarga del inicio, traslado (frame por frame), y llegada a Slot de cada Movable.
// ATTACHED TO IMAGE. Surely due to the Canvas Group Component attached (Blocks Raycasts property).

using UnityEngine;
//using System.Collections;
/* "Really powerful"
 * Needed in order to make use of the EventSystems
 */
using UnityEngine.EventSystems;
//using UnityEngine.UI;

/* 3 interfaces implemented (from EventSystems):
 * Usados cuando se necesite arrastrar un Objeto(?) Como se intuye por sus nombres:
 * - IBeginDragHandler es justo cuando se empieza el arrastre (primer frame al arrastrar?)
 * - IDragHandler está presente durante todos los frames del arrastre.
 * - IEndDragHandler debe ser justo al acabar el arrastre.
 */
public class MatDragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /* 'flag' es declarado como público para ser accedido por el Script "Slot"
     *
     */
    public bool flag;
   // bool check = itemBeingDragged.GetComponentInParent<O>.tag;

    /* Game object that's being dragged
     * 'static' because the user is only allowed to drag one object at the time.
     */
    public static GameObject itemBeingDragged;
    /* Vector to store the start position of the Game Object
     * If the user drags it to an invalid location it'll pop back to where it started from.
     */
    Vector3 startPosition;
    Transform startParent;

    /* Where the position is at the start(?)
     * Called at the Start of the Drag
     */
    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("DRAG BEGUN");
        if (flag) return;
        // Said object that's being dragged is assigned to current Game Object
        itemBeingDragged = gameObject;
        // The initial position of the Game Object.
        startPosition = transform.position;
        // To determine if the object is being dropped into a new slot
        startParent = transform.parent;
        /* Grab the canvas group added, and set it so it no longer blocks Raycast while dragging
         * -> Allows to pass events through the item that's being dragged and back to whatever items are behind it
         */
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    #endregion

    /* Where we acutally change the Transform position every frame
     * Called every frame of the Drag.
     */
    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("DRAGGING");

        if (flag) return;

        transform.position = Input.mousePosition;
    }

    #endregion

    /* Where the Transform position stays once the object is dropped(?)
     * Called when we finish the Drag.
     */
    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("DRAG CONCLUDED");

        if (flag) return;
        //ALGO NO FUNCIONA
        /*if (itemBeingDragged.transform.parent.tag.Equals("Recibidor"))
        {

            itemBeingDragged = null;

            GetComponent<CanvasGroup>().blocksRaycasts = true;

            if (transform.parent == startParent)
            {
                transform.position = startPosition;
            }
        }*/
        //if (itemBeingDragged.GetComponentInParent<Object>.CompareTag("Recibidor"))

        // OG
         itemBeingDragged = null;

        /* However, once the object has been dragged, Raycast is blocked once again.
         * -> Events can't pass through the item that was dragged (?)
         * Permite colisiones
         */
        // OG
          GetComponent<CanvasGroup>().blocksRaycasts = true;

        /* If the parent has changed, -> we don't want the object to step back to its original position.
         * But instead to go to its new position
         */
        //OG
         if (transform.parent == startParent)
         {
            transform.position = startPosition;
         }

    }

    #endregion
}

