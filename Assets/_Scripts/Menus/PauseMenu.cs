using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    //bool for if the game is paused or not
	bool isPaused = false;
    bool OnPause = false;
    bool OptionsOn = false;
    bool InstructOn = false;
    //get the pause menu
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    //public GameObject instructionsMenu;


	void Awake() {
        isPaused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        //instructionsMenu.SetActive(false);
	}

	// Use this for initialization
	void Start () {
        isPaused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        //instructionsMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
            isPaused = !isPaused;
            OnPause = !OnPause;
			//Debug.Log ("game paused");
		}

        //check if game is paused or not
		if (isPaused && OnPause && !OptionsOn && !InstructOn) {
			//Debug.Log ("game paused");
            //set the pause menu to true
            pauseMenu.SetActive(true);
			Time.timeScale = 0.0f;
		}
        else if (isPaused && !OnPause && OptionsOn && !InstructOn)
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(true);
            //instructionsMenu.SetActive(false);
        }
        else if (isPaused && !OnPause && !OptionsOn && InstructOn)
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            //instructionsMenu.SetActive(true);
        }
        else if (!isPaused /*&& !OnPause && !OptionsOn && !InstructOn*/)
        {
			//Debug.Log ("game resumed");
            //turn the pause menu off
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            //instructionsMenu.SetActive(false);
            InstructOn = false;
            OptionsOn = false;
            OnPause = false;
			Time.timeScale = 1.0f;
		}
	
	}

    public void ContinuePlaying(){
        //set the bool to continue the game
        isPaused = false;
        InstructOn = false;
        OptionsOn = false;
        OnPause = false;

    }

    public void RestartLevel()
    {
        //restarts the level
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoadOptions()
    {
        OnPause = false;
        OptionsOn = true;
        InstructOn = false;
    }

    public void LoadInstructions()
    {
        OnPause = false;
        OptionsOn = false;
        InstructOn = true;
    }

    public void LoadPauseMenu()
    {
        OnPause = true;
        OptionsOn = false;
        InstructOn = false;
    }

    public void ExitToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    void OnLevelWasLoaded()
    {
        //Any variables that need to be reset should be reset in this function
        isPaused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        //instructionsMenu.SetActive(false);
    }
}
