using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    //bool for if the game is paused or not
	bool isPaused = false;
    //get the pause menu
    public GameObject pauseMenu;


	void Awake() {
        isPaused = false;
	}

	// Use this for initialization
	void Start () {
        isPaused = false;	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
            isPaused = !isPaused;
			//Debug.Log ("game paused");
		}

        //check if game is paused or not
		if (isPaused) {
			//Debug.Log ("game paused");
            //set the pause menu to true
            pauseMenu.SetActive(true);
			Time.timeScale = 0.0f;
		} else {
			//Debug.Log ("game resumed");
            //turn the pause menu off
            pauseMenu.SetActive(false);
			Time.timeScale = 1.0f;
		}
	
	}

    public void ContinuePlaying(){
        //set the bool to continue the game
        isPaused = false;
    }

    public void RestartLevel()
    {
        //restarts the level
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ExitToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    void OnLevelWasLoaded()
    {
        //Any variables that need to be reset should be reset in this function
        isPaused = false;
    }
}
