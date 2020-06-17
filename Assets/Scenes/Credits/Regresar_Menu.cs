using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Regresar_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void RegrsarMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
