using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    //public GameObject menu;

	void Awake() {
    }

	// Use this for initialization
	void Start () {
        Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
       
    }

	//LateUpdate is called after all Update functions have been called
	void LateUpdate() {
        
    }

    public void LoadScene(string SceneName)
    {
        Application.LoadLevel(SceneName);
    }

    public void ExitApplication()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        //Application.Quit();
    }
}