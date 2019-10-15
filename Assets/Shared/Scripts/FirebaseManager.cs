using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using DocumentStore = System.Collections.Generic.Dictionary<string, object>;

public class FirebaseManager : MonoBehaviour {

	protected static FirebaseAuth auth;
	public static FirebaseAuth Auth {
		get {
			return auth;
		}
	}

	protected static DatabaseReference reference;
	public static DatabaseReference Reference {
		get {
			return reference;
		}
	}

	protected static FirebaseManager instance;
	public static FirebaseManager Instance {
		get {
			return instance;
		}
	}

	protected static FirebaseUser user;
	public static FirebaseUser User {
		get {
			return user;
		}
	}

	protected virtual void Awake () {
		instance = this;
		DontDestroyOnLoad (gameObject);
	}

	protected virtual void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonadores-eec21.firebaseio.com");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		//auth.StateChanged += AuthStateChanged;
		//AuthStateChanged(this, null);
	}

	/*
	public static void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				DebugLog("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {
				DebugLog("Signed in " + user.UserId);
				displayName = user.DisplayName ?? "";
				emailAddress = user.Email ?? "";
				photoUrl = user.PhotoUrl ?? "";
			}
		}
	}
	*/

	public static void CreateNewPlayer(string email, string username,
								string password, OnFinishConnectionCallback callback)
	{
		auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
			task => {
				if (task.IsCanceled) {
					callback.ConnectionFinished(CallbackResult.Canceled,
						"CreateNewPlayer was canceled.");
					return;
				}
				if (task.IsFaulted) {
					callback.ConnectionFinished(CallbackResult.Faulted,
						"CreateNewPlayer encountered an error: " + task.Exception);
					return;
				}

				// Firebase user has been created.
				user = task.Result;
				DocumentStore userData = new DocumentStore();
				userData["last_login"] = DateTime.Now.ToShortDateString();
				userData["username"] = username;

				reference.Child("users").Child(user.UserId).SetValueAsync(userData);

				DocumentStore statsData = initializeStatistics();

				reference.Child("statistics").Child(user.UserId).SetValueAsync(statsData);

				callback.ConnectionFinished(CallbackResult.Success, null);
			}
		);
	}

	public static void LoginPlayer(string email, string password, OnFinishConnectionCallback callback) {
		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
			task => {
				if (task.IsCanceled) {
					callback.ConnectionFinished(CallbackResult.Canceled,
						"LoginPlayer was canceled.");
					return;
				}
				if (task.IsFaulted) {
					callback.ConnectionFinished(CallbackResult.Faulted,
						"LoginPlayer encountered an error: " + task.Exception);
					return;
				}

				user = task.Result;
				reference.Child("users").Child(user.UserId).Child("last_login").SetValueAsync(DateTime.Now.ToShortDateString());

				callback.ConnectionFinished(CallbackResult.Success, null);
			}
		);
	}

	public static void SaveMissionData(MissionData newData) {
		if (user == null) {
			Debug.LogError("A logged in user is required to save mission data");
			return;
		}

		reference.Child("statistics").Child(user.UserId).RunTransaction(
			savedData => {
				Debug.Log("Running transaction on statistics");
				savedData.Value = CommitMissions(savedData.Value as DocumentStore, newData);
				return TransactionResult.Success(savedData);
			}
		).ContinueWith(
			task => {
				if (	task.IsCompleted &&
						!task.IsCanceled &&
						!task.IsFaulted) {
					//Success
					Debug.Log("Completed transaction");
				} else if (task.IsCanceled) {
					Debug.Log("Transaction was canceled");
				} else {
					Debug.LogError(task.IsFaulted + " : " + task.Exception.Message);
					Debug.LogError(task.Exception);
				}
			}
		);
	}

	public static void GetUsername(OnFinishConnectionCallback callback) {
		reference.Child("users").Child(user.UserId).GetValueAsync().ContinueWith(
			task => {
				if (task.IsCanceled) {
					callback.ConnectionFinished(CallbackResult.Canceled,
						"GetUsername was canceled.");
					return;
				}
				if (task.IsFaulted) {
					callback.ConnectionFinished(CallbackResult.Faulted,
						"GetUsername encountered an error: " + task.Exception);
					return;
				}
				if (task.IsCompleted) {
					DocumentStore userData = task.Result.Value as DocumentStore;
					string username = (string) userData["username"];
					Debug.Log("Retrieved username: " + username);
					callback.ConnectionFinished(CallbackResult.Success, username);
					return;
				}

				callback.ConnectionFinished(CallbackResult.Faulted, "Invalid task error");
			}
		);
	}

	private static DocumentStore CommitMissions(DocumentStore savedData, MissionData newData) {
		if (savedData == null) {
			Debug.Log("Initializing statistics");
			savedData = initializeStatistics();
		}

		UpdateMissionType(savedData, newData.Type);
		UpdateTime(savedData, newData.Time);

		DocumentStore levelData = null;
		if (savedData.ContainsKey(newData.LevelName)) {
			levelData =  savedData[newData.LevelName] as DocumentStore;
		}
		savedData[newData.LevelName] = CommitLevel(levelData, newData);

		return savedData;
	}

	private static DocumentStore CommitLevel(DocumentStore savedData, MissionData newData) {
		if (savedData == null) {
			Debug.Log("Initializing Level statistics");
			savedData = initializeStatistics();
			savedData["avg_time"] = "0";
		}

		UpdateMissionType(savedData, newData.Type);
		UpdateTime(savedData, newData.Time);

		return savedData;
	}

	private static DocumentStore UpdateTime(DocumentStore data, TimeSpan time) {
		TimeSpan lastTime, totalTime, avgTime;
		if (data.ContainsKey("total_time")) {
			lastTime = TimeSpan.Parse(data["total_time"] as string);
			totalTime = lastTime + time;

			if (	data.ContainsKey("avg_time") &&
					data.ContainsKey("num_m_completed")) {
				long completedMissions = ((long) data["num_m_completed"]);
				avgTime =
					TimeSpan.FromTicks(totalTime.Ticks / completedMissions);

				data["avg_time"] = avgTime.ToString();
			}

			data["total_time"] = totalTime.ToString();
		}

		return data;
	}

	private static DocumentStore UpdateMissionType(DocumentStore data,
		MissionType type) {
		string missionType = "";
		switch(type) {
			case MissionType.Rejected:
				missionType = "num_m_rejected";
				break;

			case MissionType.Began:
				missionType = "num_m_accepted";
				break;

			case MissionType.Abandoned:
				missionType = "num_m_abandoned";
				break;

			case MissionType.Failed:
				missionType = "num_m_failed";
				break;

			case MissionType.Completed:
				missionType = "num_m_completed";;
				break;

			default:
				Debug.LogError("Unsupported state");
				return data;
		}

		data[missionType] = ((long) data[missionType]) + 1L;

		return data;
	}

	private static DocumentStore initializeStatistics() {
		DocumentStore data = new DocumentStore();
		data["num_m_rejected"] = 0L;
		data["num_m_accepted"] = 0L;
		data["num_m_abandoned"] = 0L;
		data["num_m_failed"] = 0L;
		data["num_m_completed"] = 0L;
		data["total_time"] = "0";

		return data;
	}

		return data;
	}

	public enum CallbackResult {
		Canceled,
		Faulted,
		Success
	}

	public interface OnFinishConnectionCallback {
		void ConnectionFinished(CallbackResult result, string message);
	}

}


public enum MissionType {
	Rejected,
	Began,
	Abandoned,
	Failed,
	Completed
}

public struct MissionData {

	public readonly TimeSpan Time;
	public readonly MissionType Type;
	public readonly string LevelName;

	public MissionData(TimeSpan time, MissionType type, string levelName) {
		Type = type;
		Time = time;
		LevelName = levelName;
	}

}