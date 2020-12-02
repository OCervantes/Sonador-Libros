using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

using DocumentStore = System.Collections.Generic.Dictionary<string, object>;

public class TriviaController : MonoBehaviour,
	FirebaseManager.OnMissionDataRecievedCallback
{
	public Text questionDisplayText;
	public Text scoreDisplayText;
	public Text timeRemainingDisplayText;
	public SimpleObjectPool answerButtonObjectPool;
	public Transform answerButtonParent;
	public String missionName = "trivia";

	public GameObject questionPanel;
	public GameObject roundEndPanel;

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
		FirebaseManager.GetMissionData(this, missionName);
	}//end Start

	 // Called when firebase manager has a response to the API method called
    public void MissionDataRecieved(FirebaseManager.CallbackResult result,
       MissionData? data, string message) {
        // Try to handle all cases
        switch(result) {
            // If the result is not success, just print to console as error
            case FirebaseManager.CallbackResult.Canceled:
            case FirebaseManager.CallbackResult.Faulted:
            case FirebaseManager.CallbackResult.Invalid:
                Debug.LogError(message);
                break;
            // If successful, use recieved mission data to populate scene
            case FirebaseManager.CallbackResult.Success:
            default:
                Debug.Log(message);
                UnityMainThreadDispatcher
                    .Instance ()
                    .Enqueue(RoundDataLoaderWrapper(data));
                break;
        }
    }

    // Wrapper function to call RoundDataLoader as an IEnumerator
    // to work with UnityMainThreadDispatcher.
    IEnumerator RoundDataLoaderWrapper(MissionData? data) {
        RoundDataLoader(data.Value.Data);
        yield return null;
    }

    // Parsed data from Firebase into data that this scene can use.
    private void RoundDataLoader(DocumentStore data)
	{
		List<object> roundData = data["roundData"] as List<object>;
		DocumentStore singleRoundData = roundData[1] as DocumentStore;
		currentRoundData = new RoundData();
		currentRoundData.name = singleRoundData["name"] as string;
		int pointsAdedForCorrectAnswer =
			Convert.ToInt32(singleRoundData["pointsAdedForCorrectAnswer"]);
		int timeLimitInSeconds =
			Convert.ToInt32(singleRoundData["timeLimitInSeconds"]);
		currentRoundData.timeLimitInSeconds = timeLimitInSeconds;
		currentRoundData.pointsAdedForCorrectAnswer = pointsAdedForCorrectAnswer;
		List<object> questions = singleRoundData["questions"] as List<object>;
		QuestionData[] questionsData = new QuestionData[questions.Count - 1];

		for(int i = 1; i < questions.Count; i++) {
			DocumentStore question =  questions[i] as DocumentStore;
			QuestionData questionData = new QuestionData();
			questionData.questionText = question["question"] as string;
			List<object> answers = question["answers"] as List<object>;
			List<object> images = null;
			List<object> imagesBackground = null;
			if (question.ContainsKey("images")) {
				images = question["images"] as List<object>;
			}
			if (question.ContainsKey("imagesBackground")) {
				imagesBackground = question["imagesBackground"] as List<object>;
			}
			int correct = Convert.ToInt32(question["correct"]);
			AnswerData[] answersData = new AnswerData[answers.Count];
			for (int j = 0; j < answers.Count; j++) {
				AnswerData answer = new AnswerData();
				answer.answerText = answers[j] as string;
				answer.isCorrect = correct == j;
				if (imagesBackground != null && imagesBackground.Count > j) {
					string imageBackground = imagesBackground[j] as string;
					Color color;
					if (imageBackground != null &&
						ColorUtility.TryParseHtmlString(imageBackground, out color)) {
						answer.background = color;
					}
				}
				if (images != null  && images.Count > j) {
					string imageName = images[j] as string;
					if (imageName != null) {
						string directory = Application.dataPath;
						string localpath = "/Scenes/Games/Trivia/Images/";
						if (File.Exists(directory + localpath + imageName)) {
							Debug.Log("The file exists!!!");
							byte[] bytes = File.ReadAllBytes(directory + localpath + imageName);
							Texture2D texture = new Texture2D(1,1);
							texture.LoadImage(bytes);
							Sprite sprite = Sprite.Create(
								texture,
								new Rect(0, 0, texture.width, texture.height),
								new Vector2(0.5f, 0.5f)
							);
							answer.image = sprite;
						}
					}
				}
				answersData[j] = answer;
			}
			questionData.answers = answersData;
			questionsData[i - 1] = questionData;
        }
        currentRoundData.questions = questionsData;

		questionPool = currentRoundData.questions;

		timeRemaining = currentRoundData.timeLimitInSeconds;
		initialTime = currentRoundData.timeLimitInSeconds;
		UpdateTimeRemainingDisplay ();
		playerScore = 0;
		questionIndex = 0;

		ShowQuestion();
		isRoundActive = true;
	}

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
		//DocumentStore userData = new DocumentStore();
		//userData["current_level"] = 1;
		//userData["has_seen_intro"] = true;
		FirebaseManager.SaveMissionStats(
			new MissionStats(totalTime, CompletionType.Completed, 9));
		//FirebaseManager.SaveProgressStats(new ProgressData(userData));
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
