using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour {

	public string previousScene;

	private Button menuToggle, goBack;
	private Slider musicSlider, soundSlider;
	private Toggle musicToggle, soundToggle, subtitlesToggle;
	private GameObject container, sidebar, sidemenu;
	private Dictionary<string, GameObject> gameObjects;

	// Start is called before the first frame update
	void Awake() {
		container = gameObject;
		gameObjects = new Dictionary<string, GameObject>();

		addGameObjectsIn(transform);
		addGameObjectsIn("Sidebar");
		addGameObjectsIn("Side Menu");
		addGameObjectsIn("Music Controller");
		addGameObjectsIn("Sound Controller");
		addGameObjectsIn("Subtitles Controller");
	}

	void Start() {
		sidebar = gameObjects["Sidebar"];
		sidemenu = gameObjects["Side Menu"];
		menuToggle = gameObjects["Menu Toggle"].GetComponent<Button>();
		goBack = gameObjects["Back Button"].GetComponent<Button>();
		musicSlider = gameObjects["Music Slider"].GetComponent<Slider>();
		musicToggle = gameObjects["Music Toggle"].GetComponent<Toggle>();
		soundSlider = gameObjects["Sound Slider"].GetComponent<Slider>();
		soundToggle = gameObjects["Sound Toggle"].GetComponent<Toggle>();
		subtitlesToggle = gameObjects["Subtitles Toggle"].GetComponent<Toggle>();

		musicSlider.value = GameManager.State.MusicVolume;
		musicToggle.isOn = GameManager.State.MusicEnabled;
		soundSlider.value = GameManager.State.SoundVolume;
		soundToggle.isOn = GameManager.State.SoundEnabled;
		subtitlesToggle.isOn = GameManager.State.SubtitlesEnabled;

		menuToggle.onClick.AddListener(toggleSideBar);
		goBack.onClick.AddListener(loadPreviousScene);
		musicSlider.onValueChanged.AddListener(onMusicVolumeChanged);
		musicToggle.onValueChanged.AddListener(onMusicValueToggled);
		soundSlider.onValueChanged.AddListener(onSoundVolumeChanged);
		soundToggle.onValueChanged.AddListener(onSoundValueToggled);
		subtitlesToggle.onValueChanged.AddListener(onSubtitlesValueToggled);

		toggleSideBar();
	}

	private void addGameObjectsIn(string gameObjectsName) {
		if (gameObjects[gameObjectsName] != null) {
			addGameObjectsIn(gameObjects[gameObjectsName].transform);
		} else {
			Debug.LogError("Unable to find" + gameObjectsName + "in gameObjects");
		}
	}

	private void addGameObjectsIn(Transform objectTransform) {
		foreach (Transform t in objectTransform) {
			gameObjects.Add(t.name, t.gameObject);
		}
	}

	private void toggleSideBar() {
		RectTransform rectTransform = container.transform as RectTransform;
		Text buttonText = menuToggle.GetComponentInChildren<Text>();
		if (sidemenu.activeInHierarchy) {
			rectTransform.sizeDelta = new Vector2(0, 0);
			sidemenu.SetActive(false);
			buttonText.lineSpacing = 0.15f;
			buttonText.text = "-\n-\n-";
		} else {
			rectTransform.sizeDelta = new Vector2(200, 1);
			sidemenu.SetActive(true);
			buttonText.lineSpacing = 1f;
			buttonText.text = "X";
		}
	}

	private void loadPreviousScene() {
		if (previousScene != null && previousScene.Trim().Length > 0)
			SceneManager.LoadScene(previousScene);
		else
			Debug.LogError("Cannot load previous scene when a value was not provided");
	}

	private void onMusicVolumeChanged(float value) {
		GameManager.State.MusicVolume = (int) Math.Round(value);
		if (value > 0 && !musicToggle.isOn) {
			musicToggle.isOn = true;
		} else if (value == 0 && musicToggle.isOn){
			musicToggle.isOn = false;
		}
	}

	private void onMusicValueToggled(bool value) {
		GameManager.State.MusicEnabled = enabled;
	}

	private void onSoundVolumeChanged(float value) {
		GameManager.State.SoundVolume = (int) Math.Round(value);
		if (value > 0 && !soundToggle.isOn) {
			soundToggle.isOn = true;
		} else if (value == 0 && soundToggle.isOn){
			soundToggle.isOn = false;
		}
	}

	private void onSoundValueToggled(bool value) {
		GameManager.State.SoundEnabled = enabled;
	}

	private void onSubtitlesValueToggled(bool value) {
		GameManager.State.SubtitlesEnabled = enabled;
	}

}
