using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Sound_Manager : MonoBehaviour {

    public AudioSource backGround_music;
    public AudioSource SFX_clip;
    private static Sound_Manager instance = null;
    private static Object single_lock = new Object();
    public static Sound_Manager Instance
    {
        get {

            if (instance == null)
            {
                lock (single_lock)
                {
                    if (instance == null)
                    {
                        instance = ((GameObject)Instantiate(Resources.Load("Sound_Manager"))).GetComponent<Sound_Manager>();
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
            }
            return instance;
        }
    }

    public AudioMixer master_mixer;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.0f;
    private float volume = 0.0f;

   private Button mute_button; // used for displaying mute, unmute

    public List<AudioSource> sounds_evolution;
    public List<AudioSource> sounds_attacks;
    public List<AudioSource> sounds_miscellaneous;

    public AudioSource win_music;
    public AudioSource lose_music;

    private AudioMixerSnapshot snapshot_normal;
    private AudioMixerSnapshot snapshot_muted;

	// Use this for initialization
	void Awake ()
    {
        if (this == instance)
        {
            return;
        }
        // singleton check and creation for the sound manager.
        if (instance == null)
        {
            lock (single_lock)
            {
                if (instance == null)
                {

                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                    DestroyObject(gameObject);
                              
            }
        }

        else  
        {
            Destroy(gameObject);
        }

      //  mute_button = GameObject.FindGameObjectWithTag("Mute_Button").GetComponent<Button>();
        backGround_music.Play();

     //   snapshot_muted = Sound_Manager.instance.GetComponent<AudioMixerSnapshot>().audioMixer.FindSnapshot("Snapshot_muted");
     //   snapshot_muted = Sound_Manager.instance.snapshot_muted.TransitionTo(3.0f); 
        snapshot_muted = instance.master_mixer.FindSnapshot("Snapshot_muted");
        snapshot_normal = instance.master_mixer.FindSnapshot("Snapshot");

        DontDestroyOnLoad(instance);
	}

    void Update()
    {
    
     

    }

    public void Mute_Unmute_Sound(bool _mute)
    {
        if (_mute)
            snapshot_muted.TransitionTo(0.5f);
        else
            snapshot_normal.TransitionTo(0.5f);
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

    public void SetMASTER_volume(float _volume)
    {
        master_mixer.SetFloat("MasterVolume", _volume);
    
    }

    public void SetMUSIC_volume(float _volume)
    {
        master_mixer.SetFloat("Music_Volume", _volume);
    
    }

    public void SetSFX_volume(float _volume)
    {
        master_mixer.SetFloat("SFX_Volume", _volume);

    }
}
