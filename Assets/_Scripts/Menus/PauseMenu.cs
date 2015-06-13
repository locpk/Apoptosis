using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	bool isPaused = false;

	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			isPaused = !isPaused;
			//Debug.Log ("game paused");
		}

		if (isPaused) {
			//Debug.Log ("game paused");
			Time.timeScale = 0.0f;
		} else {
			//Debug.Log ("game resumed");
			Time.timeScale = 1.0f;
		}
	
	}
	
	void FixedUpdate() {
		
	}
	
	//LateUpdate is called after all Update functions have been called
	void LateUpdate() {
		
	}
}
