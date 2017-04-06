
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


	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

	public CollisionSoundPlayer key_one;
	public CollisionSoundPlayer key_two;
	public CollisionSoundPlayer key_three;
	public CollisionSoundPlayer key_four;


	private SoundPlayerBase pipePlayer;

	public AudioClip[] soundFiles;

	public AudioSource audioSource;

	private long counter = 0;

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
		app.SetEditorDatabaseUrl("https://simonsaysfirebase.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);

		FirebaseDatabase.DefaultInstance
			.GetReference ("led")
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}


			if (e2.Snapshot != null) {

				switch (e2.Snapshot.Child("color").GetValue(false).ToString())
				{
				case "0":
					key_one.PlaySound();
					break;
				case "1":
					key_two.PlaySound();
					break;
				case "2":
					key_three.PlaySound();
					break;
				case "3":
					key_four.PlaySound();
					break;
				default:
					Console.WriteLine("Default case");
					break;
				}

			}
		};


		FirebaseDatabase.DefaultInstance
			.GetReference ("status")
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}


			if (e2.Snapshot != null) {


				switch (e2.Snapshot.GetValue(false).ToString())
				{
				case "lose":
					audioSource.clip = soundFiles[0];
					audioSource.Play();
					break;
				case "win":
					audioSource.clip = soundFiles[1];
					audioSource.Play();					
					break;
				default:
					Console.WriteLine("Default case");
					break;
				}

			}
		};
	}


	public void keyPressed(String key) {
		counter++;
		SimonEvent keyPressedEvent = new SimonEvent(key, counter);
		string json = JsonUtility.ToJson(keyPressedEvent);

		FirebaseDatabase.DefaultInstance
			.GetReference ("key").SetRawJsonValueAsync(json);

	}


}
