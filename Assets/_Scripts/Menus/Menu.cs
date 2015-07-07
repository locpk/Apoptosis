using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    //public GameObject menu;

    public GameObject backGround;
    public GameObject loadingBar;

    private int loadProgress = 0;

	void Awake() 
    {

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

        StartCoroutine(DisplayLoadingScreen(SceneName));

        Application.LoadLevel(SceneName);

    }

    public void ExitApplication()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
      
    }

    // this does the loading bar and returns when done
    IEnumerator DisplayLoadingScreen(string levelName)
    {
        loadingBar.transform.localScale = new Vector3(loadProgress, loadingBar.transform.position.x, loadingBar.transform.position.z);
   
        backGround.SetActive(true);
        loadingBar.SetActive(true);

        yield return null; 
    }

}