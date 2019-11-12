using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInitialization : MonoBehaviour
{
    public int numberOfFruits=0;
    public GameObject[] fruits;

    // Start is called before the first frame update
    void Start()
    {
        numberOfFruits = fruits.Length;

        for (int i = 0; i < numberOfFruits; i++)
        {
            GameObject newFruit = /*Basket.*/Instantiate(fruits[i], randomCoordinates(), Quaternion.identity, transform);
            newFruit.transform.localScale = new Vector3(0.10f, 0.10f, 0.0f);
        }

        Debug.Log("Number of Fruits: " + numberOfFruits);
    }

    /* Variable de tipo GameObject llamado "item"
     * Avisa si cada Slot contiene una letra o no (null) 
     */

    Vector3 randomCoordinates()
    {
        float x = Random.Range(-3, 3);
        float y = Random.Range(1, 4);
        float z = -2f;

        return new Vector3(x, y, z);
    }
}
