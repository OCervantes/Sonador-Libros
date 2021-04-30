using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] Button closePopupButton;
    [SerializeField] Text popupText;
    [SerializeField] Font popupFont;

    /*
     * canvas = The canvas in which to initialize this Popup.
     * message = The popup's message.
     * action = 
     */
    public void ShowPopup(Transform canvas, string message/*, Action action*/)
    {
        // Set the popup's message and font
        popupText.text = message;
        popupText.font = popupFont;        

        /* Place Popup in Screen. 
         * Instantiating it won't make it appear on scren.
         * => Set it to a canvas.
         */
        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
                
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        gameObject.SetActive(true);

        closePopupButton.onClick.AddListener(() => 
        {
            gameObject.SetActive(false);            
            //GameObject.Destroy(this.gameObject);            
        });
    }
}
