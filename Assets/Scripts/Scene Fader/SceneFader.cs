using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour {
	public static SceneFader instance;

	[SerializeField]
	private GameObject fadeCanvas = null;

	[SerializeField]
	Animator fadeAnim = null;
	// Use this for initialization
	void Awake () {
		MakeSingleton ();
	}
	

	void MakeSingleton () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}


	public void FadeIn(string levelName){
		StartCoroutine (FadeInAnimation(levelName));
	}

	public void FadeOut(){
		StartCoroutine (FadeOutAnimation());
	}

	IEnumerator FadeInAnimation(string levelName) {
		fadeCanvas.SetActive (true);
		fadeAnim.Play ("FadeIn");
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (.7f));
		SceneManager.LoadScene (levelName);
		FadeOut ();
	}

	IEnumerator FadeOutAnimation() {
		fadeAnim.Play ("FadeOut");
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (.7f));
		fadeCanvas.SetActive (false);
	}
}
