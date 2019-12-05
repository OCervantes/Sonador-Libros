using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewAccount : MonoBehaviour,
	FirebaseManager.OnFinishConnectionCallback {

	public InputField email, username, pwd;

	public void ConnectionFinished(FirebaseManager.CallbackResult result,
		string message) {
		switch(result) {
			case FirebaseManager.CallbackResult.Canceled:
			case FirebaseManager.CallbackResult.Faulted:
			case FirebaseManager.CallbackResult.Invalid:
				Debug.LogError(message);
				break;
			case FirebaseManager.CallbackResult.Success:
			default:
				Debug.Log(message);
				UnityMainThreadDispatcher
					.Instance ()
					.EnqueueNextScene (10);
				break;
		}
	}

	public void OnNewAccount() {
		FirebaseManager.CreateNewPlayer(
			email.text,
			username.text,
			pwd.text,
			this
		);
	}

}
