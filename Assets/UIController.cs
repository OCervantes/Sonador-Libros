using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    /* static reference -> accessible from anywhere */
    public static UIController instance;
    public GameObject[] UIGameObjects;
    public Transform mainCanvas;

    // Start is called before the first frame update
    void Start()
    {

        /* If an instance already exists */
        if(instance != null)
        {
            /* Destroy said instance's gameObject */
            GameObject.Destroy(this.gameObject);
            //return;
        }

        instance = this;
        
    }

    public Popup CreatePopup()
    {
        GameObject popUpGameObject = UIGameObjects[0];//Instantiate();//GameObject.Instantiate(popup as GameObject);
        return popUpGameObject.GetComponent<Popup>();
    }
}
