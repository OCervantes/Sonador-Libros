using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour {
	public RoundData[] allRoundData; 


	void Start () {
		DontDestroyOnLoad (gameObject); //persistence of the objects
		SceneManager.LoadScene("IntroMenu");
	}//end Start	

	public RoundData GetCurrentRoundData(){
		return allRoundData [0];
	}//end GetCurrentRoundData
	
	void Update () {
		
	}//end Update
}//end DataController
