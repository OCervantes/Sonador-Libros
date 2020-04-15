using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {
	public GameObject imageLeft, imageRight;
	public TriviaController triviaController;
	public Text answerText;

	private AnswerData answerData;

	void Start () {
		triviaController = FindObjectOfType<TriviaController> ();
	}

	public void Setup(AnswerData data){
		answerData = data;
		answerText.text = answerData.answerText;
		Image imageSpriteLeft =
			imageLeft.transform.GetChild(0).gameObject.GetComponent<Image>();
		Image imageSpriteRight =
			imageRight.transform.GetChild(0).gameObject.GetComponent<Image>();
		if (data.image != null) {
			imageSpriteLeft.sprite = data.image;
			imageSpriteRight.sprite = data.image;
			imageLeft.SetActive(true);
			imageRight.SetActive(true);
		} else {
			imageSpriteLeft.sprite = null;
			imageSpriteRight.sprite = null;
			imageLeft.SetActive(false);
			imageRight.SetActive(false);
		}

		if (data.background != null) {
			imageLeft.GetComponent<Image>().color = data.background;
			imageRight.GetComponent<Image>().color = data.background;
		} else if (data.image == null) {
			imageLeft.GetComponent<Image>().color = Color.clear;
			imageRight.GetComponent<Image>().color = Color.clear;
		} else {
			imageLeft.GetComponent<Image>().color = Color.white;
			imageRight.GetComponent<Image>().color = Color.white;
		}
	}

	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	public void HandleClick(){
		triviaController.AnswerButtonClicked (answerData.isCorrect);
	}

}//end AnswerButton
