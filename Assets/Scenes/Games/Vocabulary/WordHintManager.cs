using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHintManager : MonoBehaviour
{
    public GameObject Recibidor, BancoDeLetras, checkChild;
    public GameObject[] slots;
    public GameObject[] movableSlots;
    public int numberOfHints = 3;
    public int slotIncrementer = 0;
    public int countHints = 0;

    // Start is called before the first frame update
    public void StartHint()
    {
        /*Guarda una lista de los objetos movableandslot, 
        y comparar cada una con los slots (también los podemos guardar en una lista), y
        si son iguales sus texts, entonces poner la posición de movableandSlot en la posición slot*/
        slots = GetChilds(Recibidor);
        movableSlots = GetChilds(BancoDeLetras);

        if(countHints != numberOfHints)
        {
            //Si un slot ya tiene un hijo entoncces incrementar el indice para evitar palabras repetidas esten en el mismos slot.
            if (slots[slotIncrementer].transform.childCount >= 1)
            {
                slotIncrementer++;
            }
            Debug.Log("SlotIncrementer: " + slotIncrementer + "/n" + "countHints: " + countHints);
            slots[slotIncrementer].GetComponent<Slot>().MoveCardToCorrectSlot(movableSlots);
            countHints++;
            slotIncrementer++;
            Debug.Log("SlotIncrementer: " + slotIncrementer + "/n" + "countHints: " + countHints);
            
        } 
    }

    public GameObject[] GetChilds(GameObject parent){

        GameObject[] Childs = new GameObject[parent.transform.childCount];
        for (int i=0; i< parent.transform.childCount; i++)
        {
            Childs[i] = parent.transform.GetChild(i).gameObject;
        }

        return Childs;
    }

}
