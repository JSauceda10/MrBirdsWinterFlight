using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class LeaderBoards : MonoBehaviour {
	public static LeaderBoards instance;

	private const string LEADERBOARDS_SCORE = "Cgkl_djb_P4OEAIQAQ";

	// Use this for initialization
	void Awake () {
		MakeSingleton ();
	}

	void Start(){
		PlayGamesPlatform.Activate ();
	}
	
	void OnLevelWasLoaded(){
		ReportScore (GameController.instance.GetHighScore ());
	}

	void MakeSingleton () {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	public void ConnectOrDisconnectOnGooglePlayGames(){
		if (Social.localUser.authenticated) {
			PlayGamesPlatform.Instance.SignOut ();
		} else {
			Social.localUser.Authenticate ((bool success) => {
				
			});
		}
	}

	public void OpenLeaderboardsScore(){
		if (Social.localUser.authenticated) {
			PlayGamesPlatform.Instance.ShowLeaderboardUI (LEADERBOARDS_SCORE);
		}
	}

	void ReportScore(int score){
		if (Social.localUser.authenticated) {
			Social.ReportScore (score, LEADERBOARDS_SCORE, (bool success) => {
					
			});
		}
	}
}
