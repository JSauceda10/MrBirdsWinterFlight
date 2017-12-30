using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCollector : MonoBehaviour {
	[SerializeField]
	private GameObject[] background;
	[SerializeField]
	private GameObject[] grounds;
	private float lastBGX;
	private float lastGroundX;

	// Use this for initialization
	void Awake () {
		background = GameObject.FindGameObjectsWithTag ("Background");
		grounds = GameObject.FindGameObjectsWithTag ("Ground");

		//Here we are just getting the x position of the first element in the array.
		//We'll use it to compare it against every other to make sure we get the 
		//last background in the scene setup
		lastBGX = background [0].transform.position.x;
		lastGroundX = grounds [0].transform.position.x;

		//Here, we actually mkae sure if we have the last background by comparing
		//element 0 to every other element and overwriting it if the there is 
		//another background ahead of the current one.
		for (int i = 1; i < background.Length; i++) {
			if (lastBGX < background [i].transform.position.x) {
				lastBGX = background [i].transform.position.x;
			}
		}

		for (int i = 1; i < grounds.Length; i++) {
			if (lastGroundX < grounds [i].transform.position.x) {
				lastGroundX = grounds [i].transform.position.x;
			}
		}
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == "Background") {
			Vector3 temp = target.transform.position;
			float width = ((BoxCollider2D)target).size.x;

			temp.x = lastBGX + width;

			target.transform.position = temp;

			lastBGX = temp.x;
		}else if (target.tag == "Ground") {
			Vector3 temp = target.transform.position;
			float width = ((BoxCollider2D)target).size.x;

			temp.x = lastGroundX + width;

			target.transform.position = temp;

			lastGroundX = temp.x;
		}
		
	}
}
