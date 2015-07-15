using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Sound_Manager : MonoBehaviour {

    public AudioSource backGround_music;
    public AudioSource SFX_clip;
    public static Sound_Manager instance = null;

    public AudioMixer master_mixer;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.0f;
    private float volume = 0.0f;

    Button mute_button; // used for displaying mute, unmute

    public List<AudioSource> sounds_evolution;
    public List<AudioSource> sounds_attacks;
    public List<AudioSource> sounds_miscellaneous;

	// Use this for initialization
	void Start ()
    {
        // singleton check and creation for the sound manager.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        mute_button = GameObject.FindGameObjectWithTag("Mute_Button").GetComponent<Button>();
        backGround_music.Play();
	}

    void Update()
    {
        master_mixer.GetFloat("MasterVolume", out volume);
        if (volume > -39.0f) // if the vilume is unmuted
        {
         //   mute_button.om
         //   mute_button.animationTriggers.;
        } 
    
        var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
 
         if (Input.GetKeyDown(KeyCode.H)) // force hover
         {
             ExecuteEvents.Execute(mute_button.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
         }
         if (Input.GetKeyDown(KeyCode.U)) // un-hover (end hovering)
         {
             ExecuteEvents.Execute(mute_button.gameObject, pointer, ExecuteEvents.pointerExitHandler);
         }
         if (Input.GetKeyDown(KeyCode.S)) // submit (~click)
         {
             ExecuteEvents.Execute(mute_button.gameObject, pointer, ExecuteEvents.submitHandler);
         }
         if (Input.GetKeyDown(KeyCode.P)) // down: press
         {
             ExecuteEvents.Execute(mute_button.gameObject, pointer, ExecuteEvents.pointerDownHandler);
         }
         if (Input.GetKeyUp(KeyCode.P)) // up: release
         {
             ExecuteEvents.Execute(mute_button.gameObject, pointer, ExecuteEvents.pointerUpHandler);
         }


    }

    public void PlaySingleAudio( AudioClip clip )
    {
        SFX_clip.clip = clip;
        SFX_clip.Play();
    }

    public void PlayAudioSource(AudioSource source)
    {
        source.Play();
    }

    public void RandomizeSFX(params AudioClip [] clips )
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange,highPitchRange);

        SFX_clip.pitch = randomPitch;
        SFX_clip.clip = clips[randomIndex];
        SFX_clip.Play();

    }

}
