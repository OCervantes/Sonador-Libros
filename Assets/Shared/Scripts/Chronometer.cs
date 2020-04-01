using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Chronometer : MonoBehaviour {

	private TimeSpan time;

	public TimeSpan Time {
		get {
			return time;
		}
	}

	private int seconds;
	public Text text;

	private bool isActive;
	public bool Active {
		get {
			return isActive;
		}
	}

	private void Cancel() {
		CancelInvoke ("Increment");
		isActive = false;
	}

	private void Begin() {
		Invoke ("Increment", 1.0f);
		isActive = true;
	}

	public void Activate() {
		Increment ();
	}

	public void Increment() {
		seconds++;

		time = new TimeSpan (0, 0, seconds);
		if (text != null) {
			text.text = time.ToString ();	
		}
		Begin ();
	}

	public void Pause() {
		Cancel ();
	}

	public void Resume() {
		Begin ();
	}

	public void Restart() {
		seconds = 0;
		Cancel ();
	}

}