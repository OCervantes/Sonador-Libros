// Script in charge of 
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FruitInitialization : MonoBehaviour
{
    public static int[] numFruits, fruitIndexes;
    public static bool playerCollectedCorrectAmountOfFruits = true;
    
    //Can't be static. 
    public GameObject[] fruits;    
    public Transform[] fruitPanels;
    public Vector3 goal;

    Transform[] treePanels;
    int cycles;


    void Awake()//Start()
    {        
        if (SceneManager.GetActiveScene().name == "Juego 1")
        {
            treePanels = new Transform[3];

            if (playerCollectedCorrectAmountOfFruits)
                InitializeFruits();

            treePanels[0] = transform.GetChild(0);
            treePanels[1] = transform.GetChild(1);
            treePanels[2] = transform.GetChild(2);

            goal =  InstantiateFruits();                       
        }

        else if (SceneManager.GetActiveScene().name != "Tutorial")        
        {
            for (int i=0; i<numFruits[fruitIndexes[0]]; i++)
            {
                /*GameObject newFruitPanelA = */Instantiate(fruitPanels[fruitIndexes[0]], gameObject.transform);

            }

            for (int i=0; i<numFruits[fruitIndexes[1]]; i++)
            {
                /*GameObject newFruitPanelB = */Instantiate(fruitPanels[fruitIndexes[1]], gameObject.transform);

            }

            for (int i=0; i<numFruits[fruitIndexes[2]]; i++)
            {
                /*GameObject newFruitPanelA = */Instantiate(fruitPanels[fruitIndexes[2]], gameObject.transform);

            }  
            Debug.Log("Number Fruits A: " + numFruits[0] + "\nNumber Fruits B: " + numFruits[1]);
        Debug.Log("Number Fruits C: " + numFruits[2]);
        Debug.Log("Index A: " + fruitIndexes[0] + "\nIndex B: " + fruitIndexes[1]);
        Debug.Log("Index C: " + fruitIndexes[2]);                      
        }      
        
    }
    
    /* Setter method which determines if new fruits shall be initialized in the game (the Player collected the correct a-
       mount of fruits), or not.
     */
    public void SetFruitInitializationFlag(bool value)
    {
        playerCollectedCorrectAmountOfFruits = value;
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

    void InitializeFruits()
    {
        // Number of fruits to be instantiated for each fruit type.
        numFruits = new int [3];
        // Indexes corresponding to the type of fruit to be instantiated.
        fruitIndexes = new int [3];
        //treePanels = new Transform[3];

        /* Generate number of fruits to be received from each type.
         * Total number of fruits cannot exceed 10.
         */
        do
        {
            /* When its arguments are integers, Random.Range has an exclusive max limit.
             * That is to say, it will never generate its second argument.
             */
            numFruits[0] = UnityEngine.Random.Range(1,11);
            numFruits[1] = UnityEngine.Random.Range(1,11);
            numFruits[2] = UnityEngine.Random.Range(1,11);
        } while ((numFruits[0] + numFruits[1] + numFruits[2]) > 10);

        /* Specify type of fruits to be instanced.
           0. Apple
           1. Pear
           2. Peach
         */
        do
        {
            fruitIndexes[0] = UnityEngine.Random.Range(0, 3);
            fruitIndexes[1] = UnityEngine.Random.Range(0, 3);
            fruitIndexes[2] = UnityEngine.Random.Range(0, 3);
        } while ((fruitIndexes[0] == fruitIndexes[1]) || (fruitIndexes[1] == fruitIndexes[2])  || (fruitIndexes[0] == fruitIndexes[2]));
    }

    Vector3 InstantiateFruits()
    {
        Vector3 goal = new Vector3(10,10,10);

        //Instance fruits. Max number of fruits will be instanced in each tree.
        if (Math.Max(Math.Max(numFruits[0], numFruits[1]), numFruits[2]) == numFruits[0]) //== numFruits[0]] && Math.Max(numFruits[0]], numFruitC) == numFruits[0])//(numFruitB <= numFruits[0] && numFruits[0] >= numFruitC)
            cycles = numFruits[0];
        else if ((Math.Max(Math.Max(numFruits[0], numFruits[1]), numFruits[2]) == numFruits[1]))
            cycles = numFruits[1];
        else
            cycles = numFruits[2];

        
        for (int i=0; i<cycles; i++)
        {
            GameObject newFruitA = Instantiate(fruits[fruitIndexes[0]], randomCoordinates(227, 433), Quaternion.identity, treePanels[0]);                        
            newFruitA.transform.SetParent(treePanels[0].transform, false); 

            goal =  newFruitA.transform.position;        

            GameObject newFruitB = Instantiate(fruits[fruitIndexes[1]], randomCoordinates(1053, 435), Quaternion.identity, treePanels[1]);            
            newFruitB.transform.SetParent(treePanels[1].transform, false);            

            GameObject newFruitC = Instantiate(fruits[fruitIndexes[2]], randomCoordinates(640, 513), Quaternion.identity, treePanels[2]);            
            newFruitC.transform.SetParent(treePanels[2].transform, false);            
        }
        return goal;
    }
}
