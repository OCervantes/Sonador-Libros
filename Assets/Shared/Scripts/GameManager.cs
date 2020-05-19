using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * A singleton object initalized at the beggining of the lifecycle
 * of the application (Preloader).
 * It holds common functions related to the game,
 * as well as the game state.
 */
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

	// When first initialized set the instance for GameManager to this object.
	protected virtual void Awake () {
		instance = this;
		DontDestroyOnLoad (gameObject);

		state = new GameState();
	}

	// Monitor the application at all times for an exit instruction.
	protected virtual void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

}

// The GameState class is a data holder for current game state needed
// by other pieces of code.
public class GameState {

	// Monitor whether music should be enabled.
	private bool musicEnabled = true;
	public bool MusicEnabled {
		get {
			return musicEnabled;
		}
		set {
			musicEnabled = value;
		}
	}

	// Monitor music volume.
	private int musicVolume = 10;
	public int MusicVolume {
		get {
			// If music is disabled, return 0 for volume.
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

	// Monitor whether sound should be enabled.
	private bool soundEnabled = true;
	public bool SoundEnabled {
		get {
			return soundEnabled;
		}
		set {
			soundEnabled = value;
		}
	}

	// Monitor sound volume.
	private int soundVolume = 10;
	public int SoundVolume {
		get {
			// If sound is disabled, return 0 for volume.
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

	// Monitor whether subtiltes should be enabled.
	private bool subtitlesEnabled = false;
	public bool SubtitlesEnabled {
		get {
			return subtitlesEnabled;
		}
		set {
			subtitlesEnabled = value;
		}
	}

	// Monitor wheter subtitles should be shown.
	// Note that enabled subtitles always shows them,
	// but they can also be shown if sound volume is reduced to 0.
	public bool ShowSubtitles {
		get {
			return subtitlesEnabled ||
				soundVolume == 0;
		}
	}

	public GameState() {}

}