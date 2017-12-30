using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdScript : MonoBehaviour {
	public static BirdScript instance; 

	public Rigidbody2D rb;
	public Animator anim;
	public float forwardSpeed;
	public float bounceSpeed; 
	private bool didFlap;
	public bool isAlive;

	private Button flapButton;

	[SerializeField]
	AudioSource audioSource = null;

	[SerializeField]
	AudioClip pointClip = null, diedClip = null, flapClip = null;


	public int score;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		score = PlayerPrefs.GetInt("Current Score");
		GamePlayController.instance.SetScore (score);
		isAlive = true;

		flapButton = GameObject.FindGameObjectWithTag ("FlapButton").GetComponent<Button>();
		flapButton.onClick.AddListener (() => FlapTheBird ());
		SetCamerasX ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAlive) {
			Vector3 temp = transform.position;
			temp.x += forwardSpeed * Time.deltaTime;
			transform.position = temp;
		

			if (didFlap) {
				didFlap = false;
				rb.velocity = new Vector2 (0, bounceSpeed); 
				audioSource.PlayOneShot (flapClip);
				anim.SetTrigger ("Flap");
			}

			if (rb.velocity.y >= 0) {
				transform.rotation = Quaternion.Euler (0, 0, 0);
			} else {
				float angle = 0;
				angle = Mathf.Lerp (0, -90, -rb.velocity.y / 7);
				transform.rotation = Quaternion.Euler (0, 0, angle);
			}
		}
	}
	void SetCamerasX(){
		CameraScript.offsetX = (Camera.main.transform.position.x - transform.position.x) - 1f;
	}

	public float GetPositionX(){
		return transform.position.x;
	}
	public void FlapTheBird(){
		didFlap = true;
	}

	void OnCollisionEnter2D(Collision2D target){
		if (target.gameObject.tag == "Pipe" || target.gameObject.tag == "Ground") {
			if (isAlive) {
				audioSource.PlayOneShot (diedClip);
				anim.SetTrigger ("Died");
				isAlive = false;
				GamePlayController.instance.PlayerDiedShowScore (score);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D target){
		if (target.tag == "PipeHolder") {
			audioSource.PlayOneShot (pointClip);
			score++;
			GamePlayController.instance.SetScore (score);
		}
	}
}
