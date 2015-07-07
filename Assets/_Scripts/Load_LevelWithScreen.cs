using UnityEngine;
using System.Collections;

public class Load_LevelWithScreen : MonoBehaviour {

    public GameObject backGround;
    public GameObject loadingBar;

    private int loadProgress = 0;

	// Use this for initialization
	void Start () {
        backGround.SetActive(false);
        loadingBar.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
