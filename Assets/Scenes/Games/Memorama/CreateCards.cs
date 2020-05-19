using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class CreateCards : MonoBehaviour {

	public GameObject prefabCard;
	public int width;
	public Transform ParentCards;
	//public Material[] materials;

	public int counterClicks;
	public Text textAttempsCounter;

	public Card showedCard;
	public bool itCanBeShown = true;

	public UserInterface userInterface;

	public int foundedCouples;

	private List<GameObject> cards = new List<GameObject> ();

	private Texture2D[] textures;

	// ---------------------------------------------------------------------
	// ---- functions
	// ---------------------------------------------------------------------

	public void Start(){
		 Addressables.LoadAssetsAsync<Texture2D>("memorama", null).Completed += OnSpritesLoaded;
	}

	private void OnSpritesLoaded(AsyncOperationHandle<IList<Texture2D>> result)
	{
		if (result.Status == AsyncOperationStatus.Succeeded)
		{
			if (result.Result != null || result.Result.Count > 0)
			{
				IList<Texture2D> list = result.Result;
				textures = new Texture2D[list.Count];
				list.CopyTo(textures, 0);
			}
			Addressables.Release<IList<Texture2D>>(result);
		}
		else if (result.Status == AsyncOperationStatus.Failed)
		{
			Debug.Log("List of Texture2D failed to load.");
		}
	}

	public void Restart(){
		width = 0;
		cards.Clear ();

		GameObject[] actualCards = GameObject.FindGameObjectsWithTag ("Card");
		for (int i = 0; i < actualCards.Length; i++) {
			Destroy(actualCards[i]);
		}
		counterClicks = 0;
		textAttempsCounter.text = "Intentos";
		showedCard = null;
		itCanBeShown = true;
		foundedCouples = 0;

		userInterface.chronometer.Restart ();
		userInterface.chronometer.Activate ();

		Create ();
	}//end Restart

	public void Create(){
		width = userInterface.Difficulty;

		int cont = 0;
		for(int i=0; i<width; i++){
			for(int x=0; x<width; x++){
				//float factor = 9.0f / ancho;
				//mantener distribución de las cartas
				Vector3 tempPosition = new Vector3(x,0,i /**factor*/);
				//proporcionar cartas

				GameObject tempCard = Instantiate(prefabCard, tempPosition,
					Quaternion.Euler(new Vector3(0,180,0)));

				//escalado
				//cartaTemp.transform.localScale *= factor;

				cards.Add (tempCard);

				tempCard.GetComponent<Card> ().originalPosition = tempPosition;
				//cartaTemp.GetComponent<Carta>().numCarta = cont;

				tempCard.transform.parent = ParentCards;

				cont++;
			}//end for
		}//end for

		AssignTextures();
		ShuffleCards ();
	}//end Create

	void AssignTextures(){
		//textures = Resources.Load("Resources", typeof(Texture2D)) as Texture2D;

		//variando las texturas de las cartas
		int[] tempArray = new int[textures.Length];

		for (int i = 0; i <= textures.Length-1; i++) {
			tempArray [i] = i;
		}

		for (int t = 0; t < tempArray.Length; t++) {
			int tmp = tempArray [t];
			int r = Random.Range (t, tempArray.Length);
			tempArray [t] = tempArray [r];
			tempArray [r] = tmp;
		}

		//int[] arrayDefinitivo = new int[(ancho * ancho)/2];
		int[] finalArray = new int[width * width];

		for (int i = 0; i < finalArray.Length; i++) {
			finalArray [i] = tempArray [i];
		}

		for (int i = 0; i < finalArray.Length; i++) {
			cards[i].GetComponent<Card>().AssignTexture(textures[(finalArray[i/2])]);
			cards [i].GetComponent<Card> ().numCard = i / 2;
			print (i / 2); //para obtener id's por parejas
		}
	}//end AssignTextures

	void ShuffleCards(){
		int randomNum;

		for (int i = 0; i < cards.Count; i++) {
			randomNum = Random.Range (0, cards.Count);

			cards [i].transform.position = cards [randomNum].transform.position;
			cards [randomNum].transform.position = cards [i].GetComponent<Card> ().originalPosition;

			cards [i].GetComponent<Card> ().originalPosition = cards [i].transform.position;
			cards [randomNum].GetComponent<Card> ().originalPosition = cards [randomNum].transform.position;
		}//end for
	}//end ShuffleCards

	public bool CompareCards(GameObject card1, GameObject card2){
		//if (carta1.GetComponent<MeshRenderer> ().material.mainTexture ==
		//    carta2.GetComponent<MeshRenderer> ().material.mainTexture) {
		return card1.GetComponent<Card> ().numCard == card2.GetComponent<Card> ().numCard;
	}//end CompareCards

	public void ActualizeUI(){
		textAttempsCounter.text = "Intentos: " + counterClicks;
	}//end ActualizeUI

	// ---------------------------------------------------------------------
	// ---- buttons
	// ---------------------------------------------------------------------

	public void OnClickCard(Card _card){
		if (showedCard == null) {
			showedCard = _card;
		} else {
			counterClicks++;
			ActualizeUI ();

			if (CompareCards (_card.gameObject, showedCard.gameObject)) {
				print ("Cartas iguales");
				foundedCouples++;
				print ("Parejas Encontradas " + foundedCouples);
				if (foundedCouples == cards.Count / 2) {
					print ("Se encontraron todas las parejas");
					userInterface.ShowMenuWinner ();
				}
			} else {
				print ("NO son iguales");
				_card.HideCard ();
				showedCard.HideCard ();
			}
			showedCard = null;
		}//end else
	}//end OnClickCard

}//end CreateCards
