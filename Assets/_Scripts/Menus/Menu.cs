using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    //public GameObject menu;

    public GameObject backGround;
    public GameObject loadingBar;
    public GameObject brackets;
    private float n_loadProgress = 0.0f;

	void Awake() 
    {
        brackets.SetActive(false);
        backGround.SetActive(false);
        loadingBar.SetActive(false);
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
        //loading screen goes here, fade and put it in 
        // does the progress bar over the corutine.
        StartCoroutine(DisplayLoadingScreen(SceneName));

       // Application.LoadLevel(SceneName);

    }

    public void ExitApplication()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
      
    }

    // this does the loading bar and returns when done
    IEnumerator DisplayLoadingScreen(string new_SceneName)
    {
        loadingBar.transform.localScale = new Vector3( n_loadProgress, loadingBar.transform.localScale.y, loadingBar.transform.localScale.z);
   
        backGround.SetActive(true);
        loadingBar.SetActive(true);
        brackets.SetActive(true);
        
        AsyncOperation async = Application.LoadLevelAsync(new_SceneName);
        while (!async.isDone) // carry on updating
        {
            n_loadProgress = async.progress; // returns from 0.0 - 1.0
            n_loadProgress *= 0.2f;
            loadingBar.transform.localScale = new Vector3(n_loadProgress, loadingBar.transform.localScale.y, loadingBar.transform.localScale.z);
          
            yield return null; 
        }
    }
}