using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAnimation : MonoBehaviour
{
    public Dialog dialogo;
    //public GameObject dialogBackground;
    //public GameObject handanimation;

    //public GameObject continueButton;
    void enableDialogBackground()
    {
        dialogo.dialogBackground.SetActive(true);
        dialogo.handanimation.SetActive(false);
        dialogo.continueButton.SetActive(true);
    }

}
