using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenuController : MonoBehaviour {
	public void StartGame(){
		SceneManager.LoadScene("Trivia");
	}

	public void QuitGame(){
		SceneManager.LoadScene("GameSelector");
	}

}//end IntroMenuController
