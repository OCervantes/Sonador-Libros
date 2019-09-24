using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour, FirebaseManager.OnFinishConnectionCallback {

	public InputField email, pwd;

	public void ConnectionFinished(FirebaseManager.CallbackResult result, string message) {
		switch(result) {
			case FirebaseManager.CallbackResult.Canceled:
			case FirebaseManager.CallbackResult.Faulted:
				Debug.LogError(message);
				break;
			default:
				SceneManager.LoadScene ("LivingRoom");
				break;
		}
	}

	public void OnLogin() {
		FirebaseManager.LoginPlayer(email.text, pwd.text, this);
	}
}
