using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
	public int numCard = 0;
	public Vector3 originalPosition;
	
	public Texture2D imageTexture;
	public Texture2D backTexture;

	public float timeDelay;
	public GameObject createCards;
	public bool showing;

	public GameObject userInterface;

	// ---------------------------------------------------------------------
	// ---- functions
	// ---------------------------------------------------------------------

	void Awake(){
		createCards = GameObject.Find ("Scripts");
		userInterface = GameObject.Find ("Scripts");
	}

	void Start(){
		HideCard ();
	}

	public void AssignTexture(Texture2D _texture){
		imageTexture = _texture;
	}

	public void ShowCard(){
		if (!showing && createCards.GetComponent<CreateCards>().itCanBeShown) {
			showing = true;
			GetComponent<MeshRenderer>().material.mainTexture = imageTexture;
			//Invoke ("EsconderCarta", tiempoDelay);
			createCards.GetComponent<CreateCards>().OnClickCard (this);	
		}//end if
	}//end ShowCard

	public void HideCard(){
		Invoke ("Hide", timeDelay);
		createCards.GetComponent<CreateCards> ().itCanBeShown = false;
	}//end HideCard

	void Hide(){
		GetComponent<MeshRenderer> ().material.mainTexture = backTexture;
		showing = false;
		createCards.GetComponent<CreateCards> ().itCanBeShown = true;
	}//end Hide


	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	void OnMouseDown(){
		print (numCard.ToString ());
		if (!userInterface.GetComponent<UserInterface>().menuStart.activeSelf) {
			ShowCard ();
		}
	}//end OnMouseDown
		
}//end Card
