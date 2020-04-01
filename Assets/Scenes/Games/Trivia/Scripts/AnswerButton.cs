using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {
	public Text answerText;
	private AnswerData answerData;

	public TriviaController triviaController;

	void Start () {
		triviaController = FindObjectOfType<TriviaController> ();
	}

	public void Setup(AnswerData data){
		answerData = data;
		answerText.text = answerData.answerText;
	}

	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	public void HandleClick(){
		triviaController.AnswerButtonClicked (answerData.isCorrect);
	}

}//end AnswerButton
