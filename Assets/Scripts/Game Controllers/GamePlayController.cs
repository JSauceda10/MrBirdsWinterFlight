using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GamePlayController : MonoBehaviour {
	public static GamePlayController instance;

	[SerializeField]
	Text scoreText = null, endScore= null, bestScore= null, gameOverText= null;

	[SerializeField]
	Button restartGameButton= null, instructionsButton= null;

	[SerializeField]
	GameObject pausePanel= null,AdOfferPanel = null;

	[SerializeField]
	GameObject[] birds= null;

	[SerializeField]
	Sprite[] medals= null;

	[SerializeField]
	Image medalImage= null;

	[SerializeField]
	AudioSource buttonSounds = null;

	[SerializeField]
	AudioClip buttonPress = null;
	// Use this for initialization
	void Awake () {
		//Advertisement.Initialize ("1649648",true);
		MakeInstance ();
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void MakeInstance () {
		if (instance == null) {
			instance = this;
		} 
	}

	public void PauseGame(){
		buttonSounds.PlayOneShot (buttonPress);
		if (BirdScript.instance != null) {
			if (BirdScript.instance.isAlive) {
				pausePanel.SetActive (true);
				gameOverText.gameObject.SetActive (false);
				endScore.text = "" + BirdScript.instance.score;
				bestScore.text = "" + GameController.instance.GetHighScore ();
				Time.timeScale = 0;
				restartGameButton.onClick.RemoveAllListeners ();
				restartGameButton.onClick.AddListener (() => ResumeGame ());
			}
		}
	}

	public void GoToMenuButton(){
		buttonSounds.PlayOneShot (buttonPress);
		SceneFader.instance.FadeIn ("MainMenu");

	}

	public void ResumeGame(){
		buttonSounds.PlayOneShot (buttonPress);
		pausePanel.SetActive (false);
		Time.timeScale = 1f;
	}

	public void RestartGame(){
		buttonSounds.PlayOneShot (buttonPress);
		SceneFader.instance.FadeIn (SceneManager.GetActiveScene().name);

	}

	public void PlayGame(){
		buttonSounds.PlayOneShot (buttonPress);
		scoreText.gameObject.SetActive (true);
		birds [GameController.instance.GetSelectedBird ()].SetActive (true);
		instructionsButton.gameObject.SetActive (false);
		Time.timeScale = 1f;

	}

	public void SetScore(int score){
		scoreText.text = "" + score;
	}

	public void PlayerDiedShowScore(int score){
		
		pausePanel.SetActive (true);

		AdOfferPanel.SetActive (true);

		gameOverText.gameObject.SetActive (true);
		scoreText.gameObject.SetActive (false);

		endScore.text = "" + score;

		if (score > GameController.instance.GetHighScore ()) {
			GameController.instance.SetHighScore (score);
		}

		bestScore.text = "" + GameController.instance.GetHighScore ();

		if (score <= 20) {
			medalImage.sprite = medals [0];
		} else if (score > 20 && score < 40) {
			medalImage.sprite = medals [1];

			if (GameController.instance.IsGreenBirdUnlocked () == 0) {
				GameController.instance.UnlockGreenBird ();
			} 
		} else {
			medalImage.sprite = medals [2];

			if (GameController.instance.IsGreenBirdUnlocked () == 0) {
				GameController.instance.UnlockGreenBird ();
			}

			if (GameController.instance.IsRedBirdUnlocked () == 0) {
				GameController.instance.UnlockRedBird ();
			}
		}

		restartGameButton.onClick.RemoveAllListeners ();
		restartGameButton.onClick.AddListener (() => RestartGame ());


	}

	public void WatchVideo(){
		AdOfferPanel.SetActive (false);
		if (Advertisement.IsReady ()) {
			Advertisement.Show ("rewardedVideo",new ShowOptions(){resultCallback = HandleResult});
		}

	}

	public void TurnOffAdOffer(){
		PlayerPrefs.SetInt ("Current Score",0);
		AdOfferPanel.SetActive (false);
	}

	private void HandleResult(ShowResult result){
		switch(result)
		{
		case ShowResult.Finished:
			PlayerPrefs.SetInt ("Current Score", BirdScript.instance.score);
			break;
		case ShowResult.Skipped:
			PlayerPrefs.SetInt ("Current Score",0);
			break;
		case ShowResult.Failed:
			PlayerPrefs.SetInt ("Current Score", 0);
			break;
		}
	}
}
