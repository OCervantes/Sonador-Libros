using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] Button closePopupButton;
    [SerializeField] Text popupText;

    //Transform popupCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
        //4popupCanvas = gameObject.GetComponentInParent<Transform>();
        //popupCanvas.gameObject.SetActive(true);
        //popupCanvas

        //gameObject.GetComponentInParent<GameObject>().SetActive(true);
        //showPopup(/*gameObject.GetComponentInParent<Transform>()*/UIController.instance.mainCanvas, "This is a message.");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * canvas = The canvas in which to initialize this Popup.
     * message = The popup's message.
     * action = 
     */
    public void ShowPopup(Transform canvas, string message/*, Action action*/)
    {
        /* Set the popup's message */
        popupText.text = message;

        /* Place Popup in Screen. 
         * Instantiating it won't make it appear on scren.
         * => Set it to a canvas.
         */
        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        //transform.localPosition = Vector3.zero;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        closePopupButton.onClick.AddListener(() => 
        {
            GameObject.Destroy(this.gameObject);            
        });
    }
}
