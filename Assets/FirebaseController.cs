
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using GVR.Entity;

// Handler for UI buttons on the scene.  Also performs some
// necessary setup (initializing the firebase app, etc) on
// startup.
public class FirebaseController : MonoBehaviour {

	ArrayList leaderBoard;
	Vector2 scrollPosition = Vector2.zero;
	private Vector2 controlsScrollViewVector = Vector2.zero;

	const int kMaxLogSize = 16382;
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

	public CollisionSoundPlayer key_one;
	public CollisionSoundPlayer key_two;

	// When the app starts, check to make sure that we have
	// the required dependencies to use Firebase, and if not,
	// add them if possible.
	void Start() {
		dependencyStatus = FirebaseApp.CheckDependencies();
		if (dependencyStatus != DependencyStatus.Available) {
			FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
				dependencyStatus = FirebaseApp.CheckDependencies();
				if (dependencyStatus == DependencyStatus.Available) {
					InitializeFirebase();
				} else {
					Debug.LogError(
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase();
		}
	}

	// Initialize the Firebase database:
	void InitializeFirebase() {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl("https://unitytest-72cdd.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);


		/*leaderBoard = new ArrayList();
		leaderBoard.Add("Firebase Top " + MaxScores.ToString() + " Scores");
		FirebaseDatabase.DefaultInstance
			.GetReference("Leaders").OrderByChild("score")
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError(e2.DatabaseError.Message);
				return;
			}
			string title = leaderBoard[0].ToString();
			leaderBoard.Clear();
			leaderBoard.Add(title);
			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
				foreach (var childSnapshot in e2.Snapshot.Children) {
					DebugLog("update");

					if (childSnapshot.Child("score") == null
						|| childSnapshot.Child("score").Value == null) {
						Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
						break;
					} else {
						leaderBoard.Insert(1, childSnapshot.Child("score").Value.ToString()
							+ "  " + childSnapshot.Child("email").Value.ToString());
						DebugLog("update " + childSnapshot.Child("email").Value.ToString());

					}
				}
			}
		};*/

		FirebaseDatabase.DefaultInstance
			.GetReference ("led")
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}


			if (e2.Snapshot != null) {

				Debug.Log("Joao " + e2.Snapshot.GetValue(false).ToString() );

				switch (e2.Snapshot.GetValue(false).ToString())
				{
				case "red":
					key_one.PlaySound();
					break;
				case "blue":
					key_two.PlaySound();
					break;
				default:
					Console.WriteLine("Default case");
					break;
				}

			}
		};
	}


	public void keyPressed(String key) {
		FirebaseDatabase.DefaultInstance
			.GetReference ("key")
			.SetValueAsync (key);
		
	}


}
