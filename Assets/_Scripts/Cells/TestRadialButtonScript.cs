using UnityEngine;
using System.Collections;

public class TestRadialButtonScript : MonoBehaviour {

	public GameObject radialButton;

	//get mouse position for creating buttons
	Vector3 mousePosition;
	bool objectClicked = false;
	//Vector3 objectPosition;
	//Rect heatButton;


	void Awake(){
		radialButton.SetActive (false);
		}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		if (Time.timeScale > 0.0f) {
			mousePosition = Input.mousePosition;
			Debug.Log ("Object Clicked");
			Debug.Log (mousePosition);
			objectClicked = true;
			Debug.Log (objectClicked);
			radialButton.SetActive (true);
		}

	}

}
