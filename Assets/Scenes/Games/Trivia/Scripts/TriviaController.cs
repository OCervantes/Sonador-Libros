using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TriviaController : MonoBehaviour {
	public Text questionDisplayText;
	public Text scoreDisplayText;
	public Text timeRemainingDisplayText;
	public SimpleObjectPool answerButtonObjectPool;
	public Transform answerButtonParent;

	public GameObject questionPanel;
	public GameObject roundEndPanel;

	private DataController dataController;
	private RoundData currentRoundData;
	private QuestionData[] questionPool;

	private bool isRoundActive;
	private float timeRemaining;
	private float initialTime;
	private int questionIndex;
	private int playerScore;
	// buttons that will be displayed
	public List<GameObject> answerButtonGameObjects = new List<GameObject>();

	// ---------------------------------------------------------------------
	// ---- functions
	// ---------------------------------------------------------------------

	// Use this for initialization
	void Start () {
		dataController = FindObjectOfType<DataController> (); //always begin with persistent scene
		currentRoundData = dataController.GetCurrentRoundData();
		questionPool = currentRoundData.questions;

		timeRemaining = currentRoundData.timeLimitInSeconds;
		initialTime = currentRoundData.timeLimitInSeconds;
		UpdateTimeRemainingDisplay ();
		playerScore = 0;
		questionIndex = 0;

		ShowQuestion();
		isRoundActive = true;
	}//end Start

	private void RemoveAnswerButtons(){
		while (answerButtonGameObjects.Count > 0) {
			answerButtonObjectPool.ReturnObject (answerButtonGameObjects [0]);
			answerButtonGameObjects.RemoveAt (0);
		}
	}//end RemoveAnswerButtons

	private void ShowQuestion(){
		RemoveAnswerButtons ();
		QuestionData questionData = questionPool [questionIndex];
		//reach in pool, get question and display in UI
		questionDisplayText.text = questionData.questionText;

		for (int i = 0; i < questionData.answers.Length; i++) {
			GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
			answerButtonGameObjects.Add (answerButtonGameObject);
			answerButtonGameObject.transform.SetParent (answerButtonParent);

			AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton> ();
			answerButton.Setup (questionData.answers [i]);
		}//end for

	}//end ShowQuestion

	public void EndRound(){
		isRoundActive = false;
		questionPanel.SetActive (false);
		roundEndPanel.SetActive (true);
		TimeSpan totalTime = TimeSpan.FromSeconds(initialTime - timeRemaining);
		FirebaseManager.SaveMissionStats(
			new MissionStats(totalTime, CompletionType.Completed, 9));
	}//end EndRound

	public void UpdateTimeRemainingDisplay(){
		timeRemainingDisplayText.text = "Tiempo restante: " + Mathf.Round (timeRemaining).ToString ();
	}//end UpdateTimeRemainingDisplay

	// Update is called once per frame
	void Update () {
		if (isRoundActive) {
			timeRemaining -= Time.deltaTime;
			UpdateTimeRemainingDisplay ();
			if (timeRemaining <= 0f) {
				EndRound ();
			}
		}//end if
	}//end Update

	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	public void AnswerButtonClicked(bool isCorrect){
		if (isCorrect) {
			playerScore += currentRoundData.pointsAdedForCorrectAnswer;
			scoreDisplayText.text = "Respuestas correctas: " + playerScore.ToString ();
		}
		if (questionPool.Length > questionIndex + 1) {
			questionIndex++;
			ShowQuestion ();
		} else {
			EndRound ();
		}
	}//end AnswerButtonClicked

}//end TriviaController
