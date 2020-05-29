using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desaparecer : MonoBehaviour
{
    // Start is called before the first frame update
    public float sec = 14f;
    public GameObject hand;
    public GameObject letrero;

    public int contadorclicks = 0;

    void Start(){
           desaparecerAnimacion(); 
    }

    private void RemoverAyuda()
    {
        hand.SetActive(false);
        letrero.SetActive(false);
        contadorclicks = 0;
    }

    //Activa el gameobject y luego lo hace desaparecer iniciando corutina 
    public void desaparecerAnimacion()
    {
         if (contadorclicks == 0){
            contadorclicks = 1;
            StartCoroutine(LateCall());
        }
        else{
            RemoverAyuda(); //Si vuelve a hacer click durante la animación, se detiene para remover los assets de ayuda.
            StopAllCoroutines(); //Aunque se remueven los assets la corutina sigue, por lo tanto también se para la corutina.
        }
    }

    // Update is called once per frame
    IEnumerator LateCall()
    {
        Debug.Log("Si comencé la animación");
        letrero.SetActive(true);
        hand.SetActive(true);
        
        yield return new WaitForSeconds(sec);

        hand.SetActive(false);
        letrero.SetActive(false);

        contadorclicks = 0;
        
    }
}
