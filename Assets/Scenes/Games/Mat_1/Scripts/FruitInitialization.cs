// Script in charge of 

using UnityEngine;
using UnityEngine.UI;

public class FruitInitialization : MonoBehaviour
{
    public int numFruitA, numFruitB, fruitIndexA, fruitIndexB;
    int cycles;
    public GameObject[] fruits;
    Transform fruitAPanel, fruitBPanel;

    void Start()
    {
        fruitAPanel = transform.GetChild(0);
        fruitBPanel = transform.GetChild(1);

        Debug.Log("Panel A: " + fruitAPanel.name + "\nPanel B: " + fruitBPanel.name);


        /* Generate number of fruits to be received from each type.
         * Total number of fruits cannot exceed 10.
         */
        do
        {
            numFruitA = Random.Range(1,10);
            numFruitB = Random.Range(1,10);
        } while ((numFruitA + numFruitB) > 10);


        /* Specify type of fruits to be instanced.
           1. Apple
           2. Pear
           3. Peach
         */
        do
        {
            fruitIndexA = Random.Range(0, 2);
            fruitIndexB = Random.Range(0, 2);
        } while (fruitIndexA == fruitIndexB);


        Debug.Log("Number Fruits A: " + numFruitA + "\nNumber Fruits B: " + numFruitB);
        Debug.Log("Index A: " + fruitIndexA + "\nIndex B: " + fruitIndexB);


        //Instance fruits. Max number of fruits will be instanced in each tree.
        if (numFruitB <= numFruitA)
            cycles = numFruitA;
        else
            cycles = numFruitB;

        
        for (int i=0; i<cycles; i++)
        {
            GameObject newFruitA = Instantiate(fruits[fruitIndexA], randomCoordinates(270, 590), Quaternion.identity);            
            newFruitA.GetComponent<Transform>().SetParent(fruitAPanel);
            newFruitA.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Debug.Log("Fruit A Scale: " + newFruitA.transform.localScale + "\nFruit A Position: " + newFruitA.transform.position);

            GameObject newFruitB = Instantiate(fruits[fruitIndexB], randomCoordinates(413, 1036), Quaternion.identity);
            newFruitB.GetComponent<Transform>().SetParent(fruitBPanel);
            newFruitB.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Debug.Log("Fruit B Scale: " + newFruitB.transform.localScale + "\nFruit B Position: " + newFruitB.transform.position);
        }        
        

        // Pass values to Basket Script
        Basket.fruitA = fruits[fruitIndexA];
        Basket.fruitB = fruits[fruitIndexB];

        Basket.fruitsToBeReceivedA = numFruitA;
        Basket.fruitsToBeReceivedB = numFruitB;        
    }
    
    /* Generate position in which to instantiate a fruit.
     * Missing a new condition, which will only spawn fruits within a certain radius from each Panel's center point.
     */
    Vector3 randomCoordinates(float x1, float x2)
    {
        float x = Random.Range(x1, x2);
        float y = Random.Range(352,586);
        //float z = -2f;

        return new Vector3(x, y, 0f);
    }
}
