using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	protected static GameState state;
	public static GameState State {
		get {
			return state;
		}
	}

	protected static GameManager instance;
	public static GameManager Instance {
		get {
			return instance;
		}
	}

	protected virtual void Awake () {
		instance = this;
		DontDestroyOnLoad (gameObject);

		state = new GameState();
	}

	protected virtual void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

}

public class GameState {

	private bool musicEnabled = true;
	public bool MusicEnabled {
		get {
			return musicEnabled;
		}
		set {
			musicEnabled = value;
		}
	}

	private int musicVolume = 10;
	public int MusicVolume {
		get {
			if (musicEnabled) {
				return musicVolume;
			} else {
				return 0;
			}
		}
		set {
			musicVolume = value;
		}
	}

	private bool soundEnabled = true;
	public bool SoundEnabled {
		get {
			return soundEnabled;
		}
		set {
			soundEnabled = value;
		}
	}

	private int soundVolume = 10;
	public int SoundVolume {
		get {
			if (soundEnabled) {
				return soundVolume;
			} else {
				return 0;
			}
		}
		set {
			soundVolume = value;
		}
	}

	private bool subtitlesEnabled = false;
	public bool SubtitlesEnabled {
		get {
			return subtitlesEnabled;
		}
		set {
			subtitlesEnabled = value;
		}
	}

	public bool ShowSubtitles {
		get {
			return subtitlesEnabled ||
				soundVolume == 0;
		}
	}

	public GameState() {}
}