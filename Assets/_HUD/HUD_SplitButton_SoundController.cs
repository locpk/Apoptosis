using UnityEngine;
using System.Collections;

public class HUD_SplitButton_SoundController : MonoBehaviour {

    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
    
    void OnMouseEnter()
    {
        audio.Play();
    }

    void OnHighlight()
    {

        audio.Play();
    }

	// Update is called once per frame
	void Update () 
    
    {
	
	}
}
