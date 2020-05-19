using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {
	public GameObject image;
	public TriviaController triviaController;
	public Text answerText;

	private AnswerData answerData;

	void Start () {
		triviaController = FindObjectOfType<TriviaController> ();
	}

	public void Setup(AnswerData data){
		answerData = data;
		answerText.text = answerData.answerText;
		Image imageSprite =
			image.transform.GetChild(0).gameObject.GetComponent<Image>();
		if (data.image != null) {
			imageSprite.sprite = data.image;
			image.SetActive(true);
		} else {
			imageSprite.sprite = null;
			image.SetActive(false);
		}

		if (data.background != null) {
			image.GetComponent<Image>().color = data.background;
		} else if (data.image == null) {
			image.GetComponent<Image>().color = Color.clear;
		} else {
			image.GetComponent<Image>().color = Color.white;
		}
	}

	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	public void HandleClick(){
		triviaController.AnswerButtonClicked (answerData.isCorrect);
	}

}//end AnswerButton
