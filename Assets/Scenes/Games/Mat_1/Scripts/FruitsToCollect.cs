using UnityEngine;
using UnityEngine.UI;

public class FruitsToCollect : MonoBehaviour
{
    public FruitInitialization initializer;
    static int[] fruitNumbers;
    static string[] fruitNames;    
    Text instructions;

    // Start is called before the first frame update
    void Start()
    {
        fruitNumbers = new int [3];
        fruitNames = new string [3];        

        fruitNames[0] = initializer.fruits[FruitInitialization.fruitIndexes[0]].name;
        fruitNames[1] = initializer.fruits[FruitInitialization.fruitIndexes[1]].name;
        fruitNames[2] = initializer.fruits[FruitInitialization.fruitIndexes[2]].name;

        fruitNumbers[0] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[0]];
        fruitNumbers[1] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[1]];
        fruitNumbers[2] = FruitInitialization.numFruits[FruitInitialization.fruitIndexes[2]];

        instructions = GetComponent<Text>();
                
        for (int i=0; i<3; i++)
        {
            if (fruitNumbers[i] > 1)
            {
                if (fruitNames[i] == "Peach")
                    fruitNames[i] += "es";
                else
                    fruitNames[i] += "s";
            }                
        }

        printInstructions();        
    }

    void printInstructions()
    {
        instructions.text = fruitNumbers[0] + " " + fruitNames[0] + ", " + fruitNumbers[1] + " " + fruitNames[1] + ", " + fruitNumbers[2] + " " + fruitNames[2];
    }

}
