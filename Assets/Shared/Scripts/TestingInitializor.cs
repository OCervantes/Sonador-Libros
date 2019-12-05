using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingInitializor : MonoBehaviour,
	FirebaseManager.OnFinishConnectionCallback {

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

	protected virtual void Start() {
		FirebaseManager.LoginPlayer(
			"mau.graci@gmail.com",
			"Mau1214*#",
			this
		);
	}

}
