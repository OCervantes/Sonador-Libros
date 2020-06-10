using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
	public int numCard = 0;

	public Sprite sprite;
	public Sprite defaultSprite;

	public float timeDelay;
	public bool showing;

	public GameObject createCards;
	public GameObject userInterface;
	private Image image;

	// ---------------------------------------------------------------------
	// ---- functions
	// ---------------------------------------------------------------------

	void Awake(){
		createCards = GameObject.Find ("Scripts");
		userInterface = GameObject.Find ("Scripts");
		image = GetComponent<Image>();
	}

	void Start(){
		HideCard ();
	}

	public void AssignSprite(Sprite _sprite){
		sprite = _sprite;
	}

	public void ShowCard(){
		if (!showing && createCards.GetComponent<CreateCards>().itCanBeShown) {
			showing = true;
			image.sprite = sprite;
			//Invoke ("EsconderCarta", tiempoDelay);
			createCards.GetComponent<CreateCards>().OnClickCard (this);
		}//end if
	}//end ShowCard

	public void HideCard(){
		Invoke ("Hide", timeDelay);
		createCards.GetComponent<CreateCards> ().itCanBeShown = false;
	}//end HideCard

	void Hide(){
		image.sprite = defaultSprite;
		showing = false;
		createCards.GetComponent<CreateCards> ().itCanBeShown = true;
	}//end Hide


	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	public void OnMouseDown(){
		print (numCard.ToString ());
		if (!userInterface.GetComponent<UserInterface>().menuStart.activeSelf) {
			ShowCard ();
		}
	}//end OnMouseDown

}//end Card
