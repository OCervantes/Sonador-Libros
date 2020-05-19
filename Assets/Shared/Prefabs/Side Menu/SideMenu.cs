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

	/*
	 * From the tree of objects present in the scene,
	 * create a dictionary of game object name and game object
	 */
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

	/*
	 * For convenience, assign certain game object from the generated map
	 * to concrete components.
	 * This helps when accessing methods from a component,
	 * since the type is already set and no casting is needed.
	 * In addition, initializa values of compenents with values
	 * provided by the State prsent in GameManager.
	 * Finally, add listeners to components to execute functions when
	 * condition is met.
	 */
	void Start() {
		// Cast general game object into concetre component
		sidebar = gameObjects["Sidebar"];
		sidemenu = gameObjects["Side Menu"];
		menuToggle = gameObjects["Menu Toggle"].GetComponent<Button>();
		goBack = gameObjects["Back Button"].GetComponent<Button>();
		musicSlider = gameObjects["Music Slider"].GetComponent<Slider>();
		musicToggle = gameObjects["Music Toggle"].GetComponent<Toggle>();
		soundSlider = gameObjects["Sound Slider"].GetComponent<Slider>();
		soundToggle = gameObjects["Sound Toggle"].GetComponent<Toggle>();
		subtitlesToggle = gameObjects["Subtitles Toggle"].GetComponent<Toggle>();

		// Initialize values from game state.
		musicSlider.value = GameManager.State.MusicVolume;
		musicToggle.isOn = GameManager.State.MusicEnabled;
		soundSlider.value = GameManager.State.SoundVolume;
		soundToggle.isOn = GameManager.State.SoundEnabled;
		subtitlesToggle.isOn = GameManager.State.SubtitlesEnabled;

		// Add listeners to components
		menuToggle.onClick.AddListener(toggleSideBar);
		goBack.onClick.AddListener(loadPreviousScene);
		musicSlider.onValueChanged.AddListener(onMusicVolumeChanged);
		musicToggle.onValueChanged.AddListener(onMusicValueToggled);
		soundSlider.onValueChanged.AddListener(onSoundVolumeChanged);
		soundToggle.onValueChanged.AddListener(onSoundValueToggled);
		subtitlesToggle.onValueChanged.AddListener(onSubtitlesValueToggled);

		// Toggle side bar to hide it.
		toggleSideBar();
	}

	// Helper method to unpack game object by name from generated map
	private void addGameObjectsIn(string gameObjectsName) {
		// Only add if game object exists by name in map, utilizing transform.
		if (gameObjects[gameObjectsName] != null) {
			addGameObjectsIn(gameObjects[gameObjectsName].transform);
		} else {
			Debug.LogError("Unable to find" + gameObjectsName + "in gameObjects");
		}
	}

	// Helper method to unpack game object by transform,
	// which contains in itself an array of child game objects
	private void addGameObjectsIn(Transform objectTransform) {
		foreach (Transform t in objectTransform) {
			gameObjects.Add(t.name, t.gameObject);
		}
	}

	// Toggle wheter the sidebar is shown on screen
	private void toggleSideBar() {
		RectTransform rectTransform = container.transform as RectTransform;
		Text buttonText = menuToggle.GetComponentInChildren<Text>();
		// Find if side bar is active on screen to toggle between states.
		if (sidemenu.activeInHierarchy) {
			// The side bar is hidden by setting the container size to 0
			rectTransform.sizeDelta = new Vector2(0, 0);
			// as well as setting the side bar to an inactive state
			sidemenu.SetActive(false);
			// and changing what the button to hide it displays.
			buttonText.lineSpacing = 0.15f;
			buttonText.text = "-\n-\n-";
		} else {
			// The side bar is shown by setting the container size
			rectTransform.sizeDelta = new Vector2(200, 1);
			// as well as by setting the side bar to an active state
			sidemenu.SetActive(true);
			// and changing what the button to show it displays.
			buttonText.lineSpacing = 1f;
			buttonText.text = "X";
		}
	}

	// Helper method to load a previous scene that was saved in memory
	private void loadPreviousScene() {
		if (previousScene != null && previousScene.Trim().Length > 0)
			SceneManager.LoadScene(previousScene);
		else
			Debug.LogError("Cannot load previous scene when a value was not provided");
	}

	// Helper method to update music volumne
	private void onMusicVolumeChanged(float value) {
		// The volume is synced to game state
		GameManager.State.MusicVolume = (int) Math.Round(value);
		// Update the isOn value depending on whether the value
		// is greater than zero & musicToggle state.
		if (value > 0 && !musicToggle.isOn) {
			musicToggle.isOn = true;
		} else if (value == 0 && musicToggle.isOn){
			musicToggle.isOn = false;
		}
	}

	// Helper method to update music enabled toggle
	private void onMusicValueToggled(bool value) {
		GameManager.State.MusicEnabled = enabled;
	}

	// Helper method to update sound volumne
	private void onSoundVolumeChanged(float value) {
		GameManager.State.SoundVolume = (int) Math.Round(value);
		// Update the isOn value depending on whether the value
		// is greater than zero & soundToggle state.
		if (value > 0 && !soundToggle.isOn) {
			soundToggle.isOn = true;
		} else if (value == 0 && soundToggle.isOn){
			soundToggle.isOn = false;
		}
	}

	// Helper method to update sound enabled toggle
	private void onSoundValueToggled(bool value) {
		GameManager.State.SoundEnabled = enabled;
	}

	// Helper method to update subititles enabled toggle
	private void onSubtitlesValueToggled(bool value) {
		GameManager.State.SubtitlesEnabled = enabled;
	}

}
