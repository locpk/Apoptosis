﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    public GameObject menu;

	void Awake() {
        
    }

	// Use this for initialization
	void Start () {
	
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
        Application.Quit();
    }
}