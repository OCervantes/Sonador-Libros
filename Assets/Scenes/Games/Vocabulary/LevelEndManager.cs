using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndManager : MonoBehaviour
{
    public GameObject LevelEndPanel, CompletedGamePanel, TextInstructionsGameObject;
    public string TextInstructionsString;
    public int LevelNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        //LevelEndPanel = this.gameObject;
        CompletedGamePanel.SetActive(false);
        LevelEndPanel.SetActive(false);
        //This copies the text does not reference it.
        TextInstructionsString = TextInstructionsGameObject.GetComponent<Text>().text;
    }

    public void TransitionOfLevel()
    {
        //if last level show "Juego Completado"
        if(LevelNumber == 7) { CompletedGamePanel.SetActive(true); }
        //Congratulate the player for completing the level
        TextInstructionsString = "¡Lo lograste, haz pasado el nivel " + LevelNumber + "! ¡Gracias!";
        //Assigns the string to the gameobject.
        TextInstructionsGameObject.GetComponent<Text>().text = TextInstructionsString;
        LevelEndPanel.SetActive(true);
    }

    public void NextLevel()
    {
        LevelEndPanel.SetActive(false);
        LevelNumber += 1;
    }

    public void ReturnToGameSelector()
    {
        CompletedGamePanel.SetActive(false);
        SceneManager.LoadScene("GameSelector");
    }

    public void RepeatGame()
    {
        CompletedGamePanel.SetActive(false);
        SceneManager.LoadScene("Game");
    }

}
