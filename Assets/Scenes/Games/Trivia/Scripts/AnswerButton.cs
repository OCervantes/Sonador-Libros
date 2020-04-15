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
		if (data.image != null) {
			Image imageSpriteLeft =
				imageLeft.transform.GetChild(0).gameObject.GetComponent<Image>();
			imageSpriteLeft.sprite = data.image;
			Image imageSpriteRight =
				imageRight.transform.GetChild(0).gameObject.GetComponent<Image>();
			imageSpriteRight.sprite = data.image;
			imageLeft.SetActive(true);
			imageRight.SetActive(true);
		}
		if (data.background != null) {
			imageLeft.GetComponent<Image>().color = data.background;
			imageRight.GetComponent<Image>().color = data.background;
		}
	}

	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	public void HandleClick(){
		triviaController.AnswerButtonClicked (answerData.isCorrect);
	}

}//end AnswerButton
