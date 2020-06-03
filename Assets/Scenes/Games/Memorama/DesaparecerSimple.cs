using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesaparecerSimple : MonoBehaviour
{
    public GameObject Object;
    public float sec = 14f;
    // Start is called before the first frame update
    void Start()
    {
        desaparecerSimple();
    }

    void desaparecerSimple(){
        StartCoroutine(LateCall());
    }

    IEnumerator LateCall()
    {
        Object.SetActive(true);
        
        yield return new WaitForSeconds(sec);

        Object.SetActive(false);

        
    }
}
