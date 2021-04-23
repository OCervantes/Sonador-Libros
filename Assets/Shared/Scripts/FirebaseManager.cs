using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DocumentStore = System.Collections.Generic.Dictionary<string, object>;

public class FirebaseManager : MonoBehaviour {

	/* Firebase Authentication object: manage user accounts and credentials.
	   Type: Firebase.Auth.FirebaseAuth

	   Given its static type, retrieved as an attribute, not as a method.
	 */
	protected static FirebaseAuth auth;
	public static FirebaseAuth Auth {
		get {
			return auth;
		}
	}

	/* Firebase reference: a particular location in your Database; the starting point for all Database operations.
	   After initialized it with a URL, can be used for reading or writing data to that Database location.
	   Type: Firebase.Database.DatabaseReference

	   Retrieved as an attribute, not as a method.
	 */
	protected static DatabaseReference reference;
	public static DatabaseReference Reference {
		get {
			return reference;
		}
	}

	/* An instance of this class.
	   (Custom) Type: FirebaseManager

	   Retrieved as an attribute, not as a method.
	 */
	protected static FirebaseManager instance;
	public static FirebaseManager Instance {
		get {
			return instance;
		}
	}

	/* Firebase user account object: manipulate the profile of a user, link to and unlink from authentication providers, and refresh 
	   authentication tokens.	   
	   Type: Firebase.Auth.FirebaseUser

	   Retrieved as an attribute, not as a method.
	 */
	protected static FirebaseUser user;
	public static FirebaseUser User {
		get {
			return user;
		}
	}

	/* Overridable method that initializes an instance of this class to 'instance'.
	   Also preserves the GameObject it is attached to when loading different scenes. 
	 */
	protected virtual void Awake () {
		instance = this;
		DontDestroyOnLoad (gameObject);
	}

	protected virtual void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(
			"https://sonadores-eec21.firebaseio.com");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		//auth.StateChanged += AuthStateChanged;
		//AuthStateChanged(this, null);
	}

	/* Create a new player.
	
	   Requires the use of the OnFinishConnectionCallback, which must define the implementation
	   of the ConnectionFinished method, which receives a CallbackResult and a message as parameters.
	 */
	public static void CreateNewPlayer(string email, string username,
		string password, OnFinishConnectionCallback callback) {
		auth
			.CreateUserWithEmailAndPasswordAsync(email, password)
			.ContinueWith(
				task => {
					if (task.IsCanceled) {
						callback.ConnectionFinished(CallbackResult.Canceled,
							"CreateNewPlayer was canceled.");
						return;
					}
					if (task.IsFaulted) {
						callback.ConnectionFinished(CallbackResult.Faulted,
							"CreateNewPlayer encountered an error: " +
								task.Exception);
						return;
					}
					if (!task.IsCompleted) {
						callback.ConnectionFinished(CallbackResult.Invalid,
							"CreateNewPlayer failed to complete.");
						return;
					}

					// Firebase user has been created.
					user = task.Result;
					DocumentStore userData = new DocumentStore();
					userData["last_login"] = DateTime.Now.ToShortDateString();
					userData["username"] = username;

					/* Save the user's last login date and username in the database.
					 */
					reference
						.Child("users")
						.Child(user.UserId)
						.SetValueAsync(userData);

					/* Initialize at 0:
					   - Number of rejected missions
					   - Number of accepted missions
					   - Number of abandoned missions
					   - Number of failed missions
					   - Number of completed missions
					   - Total play time
					 */
					reference
						.Child("statistics")
						.Child(user.UserId)
						.SetValueAsync(initializeStatistics());

					/*
					 */
					reference
						.Child("progress")
						.Child(user.UserId)
						.SetValueAsync(initializeProgress());

					callback.ConnectionFinished(CallbackResult.Success,
						"User succesfully created.");
				}
			);
	}

	public static void LoginPlayer(string email, string password,
		OnFinishConnectionCallback callback) {
		auth
			.SignInWithEmailAndPasswordAsync(email, password)
			.ContinueWith(
				task => {
					if (task.IsCanceled) {
						callback.ConnectionFinished(CallbackResult.Canceled,
							"LoginPlayer was canceled.");
						return;
					}
					if (task.IsFaulted) {
						callback.ConnectionFinished(CallbackResult.Faulted,
							"LoginPlayer encountered an error: " +
								task.Exception);
						return;
					}
					if (!task.IsCompleted) {
						callback.ConnectionFinished(CallbackResult.Invalid,
							"LoginPlayer failed to complete.");
						return;
					}

					user = task.Result;
					reference
						.Child("users")
						.Child(user.UserId)
						.Child("last_login")
						.SetValueAsync(DateTime.Now.ToShortDateString());

					callback.ConnectionFinished(CallbackResult.Success,
						"User succesfully logged in.");
				}
			);
	}

	//This Function is call when the Logout of the current user. But is doing somethings really strange so don use it
	/*public void Logout(){
		if (FirebaseAuth.DefaultInstance.CurrentUser != null){
			Debug.Log("Successfully logged Out");
			SceneManager.LoadScene("MainMenu");
			FirebaseAuth.DefaultInstance.SignOut();
		}
	}*/

	public static void SaveMissionStats(MissionStats newStats) {
		if (user == null) {
			Debug.LogError("A logged in user is required to save mission data.");
			return;
		}

		reference
			.Child("statistics")
			.Child(user.UserId)
			.RunTransaction(
				savedStats => {
					Debug.Log("Running transaction on statistics.");
					savedStats.Value =
						CommitMissionStats(savedStats.Value as DocumentStore,
							newStats);

					return TransactionResult.Success(savedStats);
				}
			).ContinueWith(
				task => {
					if (task.IsCanceled) {
						Debug.Log("Transaction was canceled.");
						return;
					}
					if (task.IsFaulted) {
						Debug.LogError(
							"Transaction fault : " +
							task.Exception.Message
						);
						Debug.LogError(task.Exception);
						return;
					}
					if (!task.IsCompleted) {
						Debug.LogError("Transaction failed to complete.");
						return;
					}
					Debug.Log("Transaction complete.");
				}
			);
	}		

	public static void GetUsername(OnResultRecievedCallback callback) {
		reference
			.Child("users")
			.Child(user.UserId)
			.GetValueAsync()
			.ContinueWith(
				task => {
					if (task.IsCanceled) {
						callback.ResultRecieved(
							CallbackResult.Canceled,
							null,
							"GetUsername was canceled."
						);
						return;
					}
					if (task.IsFaulted) {
						callback.ResultRecieved(
							CallbackResult.Faulted,
							null,
							"GetUsername encountered an error: " +
								task.Exception
						);
						return;
					}
					if (!task.IsCompleted) {
						callback.ResultRecieved(
							CallbackResult.Invalid,
							null,
							"GetUsername failed to complete."
						);
						return;
					}
					
					callback.ResultRecieved(
						CallbackResult.Success,
						task.Result.Value as DocumentStore,
						"Successfully retrieved username."
					);
				}
			);
	}

	public static void SaveProgressStats(ProgressData newdata) {
		if (user == null) {
			Debug.LogError("A logged in user is required to save progress data.");
			return;
		}

		reference
			.Child("progress")
			.Child(user.UserId)
			.RunTransaction(
				savedStats => {
					Debug.Log("Running transaction on progress.");
					//DocumentStore userData = new DocumentStore();
					savedStats.Value = CommitProgressData(savedStats.Value as DocumentStore, newdata);
					

					return TransactionResult.Success(savedStats);
				}
			).ContinueWith(
				task => {
					if (task.IsCanceled) {
						Debug.Log("Transaction was canceled.");
						return;
					}
					if (task.IsFaulted) {
						Debug.LogError(
							"Transaction fault : " +
							task.Exception.Message
						);
						Debug.LogError(task.Exception);
						return;
					}
					if (!task.IsCompleted) {
						Debug.LogError("Transaction failed to complete.");
						return;
					}
					Debug.Log("Transaction complete.");
				}
			);
	}

	private static DocumentStore CommitProgressData(DocumentStore userData,
		ProgressData newdata) {
		if (userData == null) {
			Debug.Log("Initializing statistics");
			userData = initializeProgress();
		}
			userData["current_level"] = newdata.currentLevel;
			userData["has_seen_intro"] = newdata.hasSeenIntro;

		return userData;
	}

	public static void GetMissionData(OnMissionDataRecievedCallback callback,
		string missionID) {
		reference
			.Child("missions")
			.Child(missionID)
			.GetValueAsync()
			.ContinueWith(
				task => {
					if (task.IsCanceled) {
						callback.MissionDataRecieved(
							CallbackResult.Canceled,
							null,
							"GetMissionData was canceled."
						);
						return;
					}
					if (task.IsFaulted) {
						callback.MissionDataRecieved(
							CallbackResult.Faulted,
							null,
							"GetMissionData encountered an error: " +
								task.Exception
						);
						return;
					}
					if (!task.IsCompleted) {
						callback.MissionDataRecieved(
							CallbackResult.Invalid,
							null,
							"GetMissionData failed to complete."
						);
						return;
					}
					MissionData data = new MissionData(
						task.Result.Value as DocumentStore);
					callback.MissionDataRecieved(
						CallbackResult.Success,
						data,
						"Successfully retrieved mission data."
					);
				}
			);
	}

	private static DocumentStore CommitMissionStats(DocumentStore savedStats,
		MissionStats newStats) {
		if (savedStats == null) {
			Debug.Log("Initializing statistics");
			savedStats = initializeStatistics();
		}

		IncrementCompletionFor(savedStats, newStats.Type);
		UpdateTime(savedStats, newStats.Time);

		DocumentStore levelStats = null;
		if (savedStats.ContainsKey(newStats.LevelID)) {
			levelStats = savedStats[newStats.LevelID] as DocumentStore;
		}
		savedStats[newStats.LevelID] = CommitLevelStats(levelStats, newStats);

		return savedStats;
	}

	private static DocumentStore CommitLevelStats(DocumentStore savedStats,
		MissionStats newStats) {
		if (savedStats == null) {
			Debug.Log("Initializing Level statistics");
			savedStats = initializeStatistics();
			savedStats["avg_time"] = "0";
		}

		IncrementCompletionFor(savedStats, newStats.Type);
		UpdateTime(savedStats, newStats.Time);

		return savedStats;
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

	private static DocumentStore IncrementCompletionFor(DocumentStore data,
		CompletionType type) {
		string completionName = "";
		switch(type) {
			case CompletionType.Rejected:
				completionName = "num_m_rejected";
				break;

			case CompletionType.Began:
				completionName = "num_m_accepted";
				break;

			case CompletionType.Abandoned:
				completionName = "num_m_abandoned";
				break;

			case CompletionType.Failed:
				completionName = "num_m_failed";
				break;

			case CompletionType.Completed:
				completionName = "num_m_completed";;
				break;

			default:
				Debug.LogError("Unsupported state");
				return data;
		}

		data[completionName] = ((long) data[completionName]) + 1L;

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

	private static DocumentStore initializeProgress() {
		DocumentStore data = new DocumentStore();

		data["current_level"] = 0;
		data["has_seen_intro"] = false;		

		return data;
	}

	public enum CallbackResult {
		Canceled,
		Faulted,
		Invalid,
		Success
	}

	public interface OnFinishConnectionCallback {
		void ConnectionFinished(CallbackResult result, string message);
	}

	public interface OnMissionDataRecievedCallback {
		void MissionDataRecieved(CallbackResult result, MissionData? data,
			string message);
	}

	public interface OnResultRecievedCallback {
		void ResultRecieved(CallbackResult resul, DocumentStore data,
			string message);
	}

}

public enum CompletionType {
	Rejected,
	Began,
	Abandoned,
	Failed,
	Completed
}

public struct MissionStats {

	public readonly TimeSpan Time;
	public readonly CompletionType Type;
	public readonly string LevelID;

	public MissionStats(TimeSpan time, CompletionType type, int levelID) {
		Type = type;
		Time = time;
		LevelID = levelID.ToString();
	}

}

public struct MissionData {

	public readonly string Name;
	public readonly bool IsMath;
	public readonly bool IsLangague;
	public readonly string SceneId;
	public int SceneIndex {
		get {
			try {
				return Int32.Parse(SceneId);
			} catch (FormatException e) {
				return -1;
			}
		}
	}
	public readonly DocumentStore Data;

	public MissionData(DocumentStore data) {
		Name = (string) data["name"];
		IsMath = (bool) data["is_math"];
		IsLangague = (bool) data["is_language"];
		object sceneId = data["scene_id"];
		try {
			SceneId = (string) sceneId;
		} catch(Exception e) {
			SceneId = Convert.ToInt32(sceneId) + "";
		}
		Data = data["data"] as DocumentStore;
	}	
}

public struct ProgressData{
	public readonly int currentLevel;
	public readonly bool hasSeenIntro;
	
	public ProgressData(DocumentStore data){
		hasSeenIntro = (bool) data["has_seen_intro"];
		currentLevel = (int) data["current_level"];
	}
}