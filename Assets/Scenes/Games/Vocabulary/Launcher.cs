using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(LaunchGame(5));
	}

	private IEnumerator LaunchGame(int index) {
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene (index);
	}

}