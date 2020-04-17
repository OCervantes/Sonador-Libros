// Script in charge of 
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FruitInitialization : MonoBehaviour
{
    public static int[] numFruits, fruitIndexes;
    int cycles;
    //Can't be static. 
    public GameObject[] fruits;
    Transform [] fruitPanels;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Juego 1")
        numFruits = new int [3];
        fruitIndexes = new int [3];
        fruitPanels = new Transform[3];

        fruitPanels[0] = transform.GetChild(0);
        fruitPanels[1] = transform.GetChild(1);
        fruitPanels[2] = transform.GetChild(2);        

        /* Generate number of fruits to be received from each type.
         * Total number of fruits cannot exceed 10.
         */
        do
        {
            /* When its arguments are integers, Random.Range has an exclusive max limit.
             * That is to say, it well never generate its second argument.
             */
            numFruits[0] = UnityEngine.Random.Range(1,11);
            numFruits[1] = UnityEngine.Random.Range(1,11);
            numFruits[2] = UnityEngine.Random.Range(1,11);
        } while ((numFruits[0] + numFruits[1] + numFruits[2]) > 10);


        /* Specify type of fruits to be instanced.
           1. Apple
           2. Pear
           3. Peach
         */
        do
        {
            fruitIndexes[0] = UnityEngine.Random.Range(0, 3);
            fruitIndexes[1] = UnityEngine.Random.Range(0, 3);
            fruitIndexes[2] = UnityEngine.Random.Range(0, 3);
        } while ((fruitIndexes[0] == fruitIndexes[1]) || (fruitIndexes[1] == fruitIndexes[2])  || (fruitIndexes[0] == fruitIndexes[2]));


        Debug.Log("Number Fruits A: " + numFruits[0] + "\nNumber Fruits B: " + numFruits[1]);
        Debug.Log("Number Fruits C: " + numFruits[2]);
        Debug.Log("Index A: " + fruitIndexes[0] + "\nIndex B: " + fruitIndexes[1]);
        Debug.Log("Index C: " + fruitIndexes[2]);


        //Instance fruits. Max number of fruits will be instanced in each tree.
        if (Math.Max(Math.Max(numFruits[0], numFruits[1]), numFruits[2]) == numFruits[0]) //== numFruits[0]] && Math.Max(numFruits[0]], numFruitC) == numFruits[0])//(numFruitB <= numFruits[0] && numFruits[0] >= numFruitC)
            cycles = numFruits[0];
        else if ((Math.Max(Math.Max(numFruits[0], numFruits[1]), numFruits[2]) == numFruits[1]))
            cycles = numFruits[1];
        else
            cycles = numFruits[2];

        
        for (int i=0; i<cycles; i++)
        {
            GameObject newFruitA = Instantiate(fruits[fruitIndexes[0]], randomCoordinates(227, 433), Quaternion.identity, fruitPanels[0]);                        
            newFruitA.transform.SetParent(fruitPanels[0].transform, false);            

            GameObject newFruitB = Instantiate(fruits[fruitIndexes[1]], randomCoordinates(1053, 435), Quaternion.identity, fruitPanels[1]);            
            newFruitB.transform.SetParent(fruitPanels[1].transform, false);            

            GameObject newFruitC = Instantiate(fruits[fruitIndexes[2]], randomCoordinates(640, 513), Quaternion.identity, fruitPanels[2]);            
            newFruitC.transform.SetParent(fruitPanels[2].transform, false);            
        }        
        
        Debug.Log("Fruits[0]: " + fruits[fruitIndexes[0]] + "\nFruits[1]: " + fruits[fruitIndexes[1]]);    
    }
    
    /* Generate position in which to instantiate a fruit.
     * Missing a new condition, which will only spawn fruits within a certain radius from each Panel's center point.
     */
    Vector2 randomCoordinates(float x, float y)
    {
        float resX = UnityEngine.Random.Range(x-190/*203*/, x+190/*204*/);
        float resY = UnityEngine.Random.Range(y-110/*7*/, y+110/*8*/);        

        return new Vector3(resX, resY);
    }
}
