using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void ToLogin() {
		SceneManager.LoadScene ("Login");
	}

	public void ToNewAccount() {
		SceneManager.LoadScene ("NewAccount");
	}
}
