using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	protected static GameData data;
	public static GameData Data {
		get {
			return data;
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
	}

	protected virtual void Start() {
		data = new GameData();
	}

	protected virtual void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

}

public class GameData {

	private Vector2 mapCharPos;
	public Vector2 MapCharPos {
		get {
			return mapCharPos;
		}
		set {
			mapCharPos = value;
		}
	}

	private float mapCharRot;
	public float MapCharRot {
		get {
			return mapCharRot;
		}
		set {
			mapCharRot = value;
		}
	}

	public GameData() {
		mapCharPos = new Vector2(-3.07f, -64.22f);
		mapCharRot = 0f;
	}
}