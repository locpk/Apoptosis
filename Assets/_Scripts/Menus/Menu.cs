using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour {

    // for loading screen
    public GameObject backGround;
    public GameObject loadingBar;
    public GameObject brackets;
    private float n_loadProgress = 0.0f;

    // for the audio effects 
    public AudioMixerSnapshot audio_muted;
    public AudioMixerSnapshot audio_unmuted;
    public AudioMixer master_mixer;
    public AudioSource click_Sound;
    public bool muted;
    private float volume_master_stored;
    

    // for file IO 
    bool changed;
    byte[] configuration;
    System.IO.FileStream configFile;

    GameObject MasterVolSlider;
    GameObject MusicVolSlider;
    GameObject SFXVolSlider;
    public bool b_fullscreen; 
  //  Slider mas_slider;
  //  Slider mus_slider;
  //  Slider sfx_slider;

    Rect mas_slider_bounds;
	
    // screen stuff 

   public void LoadMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
    void Awake() 
    {

     //   testAudio = GameObject.FindGameObjectWithTag("Menu_Canvas").GetComponent<AudioSource>();

        // hides the loaging screen.
        brackets.SetActive(false);
        backGround.SetActive(false);
        loadingBar.SetActive(false);

      //  MasterVolSlider = GameObject.FindGameObjectWithTag("Master_Slider");
      //  mas_slider = MasterVolSlider.GetComponent<Slider>();
      //  mas_slider_bounds = mas_slider.GetComponent<RectTransform>().rect;
      //  mas_slider_bounds.xMin += mas_slider.GetComponent<RectTransform>().position.x;
      //  mas_slider_bounds.xMax += mas_slider.GetComponent<RectTransform>().position.x;
      //  mas_slider_bounds.yMin += mas_slider.GetComponent<RectTransform>().position.y;
      //  mas_slider_bounds.yMax += mas_slider.GetComponent<RectTransform>().position.y;

       // MusicVolSlider = GameObject.FindGameObjectWithTag("Music_Slider");
       // SFXVolSlider = GameObject.FindGameObjectWithTag("SFX_Slider");

     // if (!System.IO.File.Exists("OptionsMenu.cfg"))
     // {
     //     configFile = new System.IO.FileStream("OptionsMenu.cfg", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);
     //     byte[] emptyparams = new byte[8];
     //     configFile.Write(emptyparams, 0, 8);
     //     configFile.Close();
     // }
     // else
     // {
     //     configFile = new System.IO.FileStream("OptionsMenu.cfg", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
     //     configuration = new byte[9];
     //     configFile.Read(configuration, 0, 9);
     //     MasterVolSlider.GetComponent<UnityEngine.UI.Slider>().value = System.BitConverter.ToSingle(configuration, 0);
     //     MusicVolSlider. GetComponent<UnityEngine.UI.Slider>().value = System.BitConverter.ToSingle(configuration, sizeof(float));
     //     SFXVolSlider.   GetComponent<UnityEngine.UI.Slider>().value = System.BitConverter.ToSingle(configuration, sizeof(float));
     //     b_fullscreen = System.BitConverter.ToBoolean(configuration,sizeof(float)); //ToSingle(configuration, sizeof(float));
     //     configFile.Close();
     // }

          
        // make fullscreen
          if (Application.platform == RuntimePlatform.Android)
          {
              Screen.fullScreen = true;
          }
          else
          {
              Screen.fullScreen = b_fullscreen;
             
          }
          changed = false; // must not forget to reset 
    }

    public void ToggleMakeFullScreen(bool full)
    {
        Screen.fullScreen = full;
        b_fullscreen = full;
        changed = true;
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

    public void SetBackgroundMusicVol(float mus_vol)
    {
      master_mixer.SetFloat("Music_Volume", mus_vol);
    }
    public void SetSFXVoume(float SFX_vol)
    {
        master_mixer.SetFloat("SFX_Volume", SFX_vol);
     
    }
    public void SetMasterVolume(float mas_vol)
    {
        master_mixer.SetFloat("MasterVolume", mas_vol);
        volume_master_stored = mas_vol;
   }
    public void ClearVolume()
    {
        master_mixer.ClearFloat("Master_Volume");
 
    }

	//LateUpdate is called after all Update functions have been called
	void LateUpdate() {
        
    }

    public void LoadScene(string SceneName)
    {
         //loading screen goes here, fade and put it in 
        // does the progress bar over the corutine.
        StartCoroutine(DisplayLoadingScreen(SceneName));
    }

    public void ExitApplication()
    {
        backGround.SetActive(true);
#if UNITY_EDITOR
       EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
        if (Application.isEditor)
        {
            return;
        }
        if (Input.touchSupported)
        {
            Application.Quit(); 
        }
        System.Diagnostics.Process.GetCurrentProcess().Kill();
      
    }

    // this does the loading bar while the scene loads and returns when done
    public IEnumerator DisplayLoadingScreen(string new_SceneName)
    {
        loadingBar.transform.localScale = new Vector3( n_loadProgress, loadingBar.transform.localScale.y, loadingBar.transform.localScale.z);
   
        backGround.SetActive(true);
        loadingBar.SetActive(true);
        brackets.SetActive(true);
        
        AsyncOperation async = Application.LoadLevelAsync(new_SceneName);
        while (!async.isDone) // carry on updating while loading is not done
        {
            n_loadProgress = async.progress; // returns from 0.0 - 1.0
            n_loadProgress *= 0.2f; // scaling for graphics 
            loadingBar.transform.localScale = new Vector3(n_loadProgress, loadingBar.transform.localScale.y, loadingBar.transform.localScale.z); // moves the bar along 
  
            yield return null; // returns result every frame
        }
    }

    // this controls the speaker botton and unmutes or mutes it
    public void Mute_Unmute_sound()
    {
        if (!muted) // if everything is fine and music is playing 
        {
           // unmute the vibe 
            muted = !muted;
            master_mixer.SetFloat("MasterVolume", volume_master_stored);
            //  audio_unmuted.TransitionTo(0.1f);
        }
        else 
        {
            // mute the Vibe
            master_mixer.GetFloat("MasterVolume",out volume_master_stored);
            master_mixer.SetFloat("MasterVolume", -40.0f);
            muted = !muted;
        
        }
    
    }

    public void SaveToFile()
    {
        if (changed)
        {
          //  configFile.Write(System.BitConverter.GetBytes(mas_slider.GetComponent<UnityEngine.UI.Slider>().value), 0, sizeof(float));
          //  configFile.Write(System.BitConverter.GetBytes(mus_slider.GetComponent<UnityEngine.UI.Slider>().value), 0, sizeof(float));
         //   configFile.Write(System.BitConverter.GetBytes(sfx_slider.GetComponent<UnityEngine.UI.Slider>().value), 0, sizeof(float));
         //   configFile.Write(System.BitConverter.GetBytes(b_fullscreen), 0, sizeof(float));
       
            configFile.Close();
        }
    
    }

    // this is for the facebook link and stuff
    public void LoadURL(string link)
    {
        Application.OpenURL(link);
    }
}