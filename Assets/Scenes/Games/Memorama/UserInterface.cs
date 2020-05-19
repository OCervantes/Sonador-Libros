using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour {

	public GameObject buttonPause;
	public GameObject menuPause;
	public GameObject menuStart;
	public GameObject menuWinner;
	public Text textTime;
	public Slider sliderDifficulty;
	public Text textDifficulty;
	public Chronometer chronometer;

	private int difficulty = 2;
	public int Difficulty {
		get {
			return difficulty;
		}
	}

	// ---------------------------------------------------------------------
	// ---- functions
	// ---------------------------------------------------------------------

	void Start() {
		ShowMenuStart ();
		HideMenuWinner ();
		ChangeDifficulty (3);

		sliderDifficulty.onValueChanged.AddListener(ChangeDifficulty);

		menuPause.SetActive (false);
	}//end Start

	public void ShowMenuStart(){
		menuStart.SetActive (true);
		buttonPause.SetActive(false);
	}

	public void HideMenuStart(){
		menuStart.SetActive (false);
		buttonPause.SetActive(true);
	}

	public void Pause(){
		if (menuPause.activeSelf) {
			menuPause.SetActive (false);
			chronometer.Resume ();
		} else {
			menuPause.SetActive (true);
			chronometer.Pause ();
		}
	}

	public void ShowMenuWinner(){
		menuWinner.SetActive (true);
		chronometer.Pause ();
		textTime.text = chronometer.Time.ToString ();
		buttonPause.SetActive(false);
		FirebaseManager.SaveMissionStats(
			new MissionStats(chronometer.Time, CompletionType.Completed, 10));
	}

	public void HideMenuWinner(){
		menuWinner.SetActive (false);
	}

	public void ChangeDifficulty (float value){
		difficulty = (int) value;
		textDifficulty.text = "Dificultad: " + difficulty + "x" + difficulty;
	}

	public void ActivateChronometer(){
		chronometer.Activate();
	}

	public void RestartChronometer(){
		chronometer.Restart();
	}

	public void PauseChronometer(){
		chronometer.Pause();
	}

	public void ExitMemorama(){
		SceneManager.LoadScene (10); // map
	}

}//end UserInterface
