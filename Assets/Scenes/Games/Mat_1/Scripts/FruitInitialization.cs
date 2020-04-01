using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInitialization : MonoBehaviour
{
    public int num_fruitA, num_fruitB, fruit_indexA, fruit_indexB;
    public GameObject[] fruits;

    // Start is called before the first frame update
    void Start()
    {
        num_fruitA = Random.Range(1,10);
        num_fruitB = Random.Range(1,10);
        fruit_indexA = Random.Range(1, 3);
        fruit_indexB = Random.Range(1, 3);

        Debug.Log("Number A: " + num_fruitA + "\nNumber B: " + num_fruitB);
        Debug.Log("Index A: " + fruit_indexA + "\nIndex B: " + fruit_indexB);
        /*numberOfFruits = fruits.Length;

        for (int i = 0; i < numberOfFruits; i++)
        {
            GameObject newFruit = /*Basket.Instantiate(fruits[i], randomCoordinates(), Quaternion.identity, transform);
            newFruit.transform.localScale = new Vector3(0.10f, 0.10f, 0.0f);
        }

        Debug.Log("Number of Fruits: " + numberOfFruits);*/
    }

    /* Variable de tipo GameObject llamado "item"
     * Avisa si cada Slot contiene una letra o no (null) 
     */

    /*Vector3 randomCoordinates()
    {
        float x = Random.Range(-3, 3);
        float y = Random.Range(1, 4);
        float z = -2f;

        return new Vector3(x, y, z);
    }*/
}
