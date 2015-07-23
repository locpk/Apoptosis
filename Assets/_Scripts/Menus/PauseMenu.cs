using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

    //bool for if the game is paused or not
    bool PauseGame = false;
    //bool for pause menu canvas
    bool OnPause = false;
    //bool for options menu canvas
    bool OptionsOn = false;
    //bool for instructions menu canvas
    bool InstructOn = false;
    //get the pause menu
    public GameObject pauseMenu;
    //get the options menu
    public GameObject optionsMenu;
    //get the instructions menu
    public GameObject instructionsMenu;
    //Capture the pause menu button
    public GameObject pauseMenuButton;

    //Sound_Manager manager; 

    //private GameObject playerController;
    void Awake()
    {
        //set everything to false
        PauseGame = false;
        OptionsOn = false;
        OnPause = false;
        InstructOn = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        instructionsMenu.SetActive(false);


        //manager = Sound_Manager.Instance;
    }

    // Use this for initialization
    void Start()
    {
        //set everything to false
        PauseGame = false;
        OptionsOn = false;
        OnPause = false;
        InstructOn = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        instructionsMenu.SetActive(false);

        //playerController = GameObject.FindGameObjectWithTag("PlayerController");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //pause and unpause the game when the user hits escape
            PauseGame = !PauseGame;
            //turn on and off the pause menu based on user input
            OnPause = !OnPause;
        }

        //check if game is paused or not
        if (PauseGame)
        {
            pauseMenuButton.SetActive(false);
            //check if only pause menu is on
            if (OnPause && !OptionsOn && !InstructOn)
            {
                //set the pause menu to true
                pauseMenu.SetActive(true);

                //turn the options menu off
                optionsMenu.SetActive(false);

                //turn the instructions menu off
                instructionsMenu.SetActive(false);

            }
            //check if only options menu is on
            else if (!OnPause && OptionsOn && !InstructOn)
            {
                //turn off the pause menu
                pauseMenu.SetActive(false);

                //turn on the options menu
                optionsMenu.SetActive(true);

                //turn off the instructions menu
                instructionsMenu.SetActive(false);
            }
            //check if only instructions menu is on
            else if (!OnPause && !OptionsOn && InstructOn)
            {
                //turn off the pause menu
                pauseMenu.SetActive(false);

                //turn off the options menu
                optionsMenu.SetActive(false);

                //turn on the instructions menu
                instructionsMenu.SetActive(true);
            }

            //set the timescale to 0 to pause the game
            Time.timeScale = 0.0f;
        } 
        else
        {
            //turn the all menus off
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            instructionsMenu.SetActive(false);

            //turn all menu bools off
            InstructOn = false;
            OptionsOn = false;
            OnPause = false;

            //turn the timescale back to 1 to continue playing
            Time.timeScale = 1.0f;

            //turn pause menu button back on
            pauseMenuButton.SetActive(true);
        }

    }

    //call this function to continue the game
    public void ContinuePlaying()
    {
        //set the PauseGame to false to continue the game
        PauseGame = false;
    }

    //call this function to restart the level
    public void RestartLevel()
    {
        //restarts the level
        Application.LoadLevel(Application.loadedLevel);
    }

    //call this function to load the pause menu
    public void LoadPauseMenu()
    {
        //Pause the game
        PauseGame = true;

        //turn the pause menu bool on
        OnPause = true;

        //turn the options menu bool off
        OptionsOn = false;

        //turn the instructions menu bool off
        InstructOn = false;
    }

    //call this function to load the options menu
    public void LoadOptions()
    {
        //turn the pause menu bool off
        OnPause = false;

        //turn the options menu bool on
        OptionsOn = true;

        //turn the instructions menu off
        InstructOn = false;
    }

    //call this function to load the instructions menu
    public void LoadInstructions()
    {
        //turn the pause menu bool off
        OnPause = false;

        //turn the options menu bool off
        OptionsOn = false;

        //turn the instructions menu bool on
        InstructOn = true;
    }

    //call this function to exit single player and go back to the main menu
    public void ExitToMainMenu()
    {
        //load the main menu scene
        Application.LoadLevel("MainMenu");
    }

    //Any variables that need to be reset should be reset in this function
    void OnLevelWasLoaded()
    {
        //set everything to false
        PauseGame = false;
        OptionsOn = false;
        OnPause = false;
        InstructOn = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        instructionsMenu.SetActive(false);

    }
}
