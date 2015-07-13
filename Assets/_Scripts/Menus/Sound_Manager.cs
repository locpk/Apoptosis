using UnityEngine;
using System.Collections;

public class Sound_Manager : MonoBehaviour {

    public AudioSource backGround_music;
    public AudioSource SFX_clip;
    public static Sound_Manager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.0f;

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
