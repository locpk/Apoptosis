using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
	AudioSource volTest;
	GameObject BGMVolSlider;
	Rect BGMBounds;
	Rect SEBounds;
	GameObject SEVolSlider;
    // Use this for initialization
    void Start()
	{
		BGMVolSlider = GameObject.Find ("BGM_Vol_Slider");
		SEVolSlider = GameObject.Find ("SE_Vol_Slider");
		volTest = BGMVolSlider.GetComponent<AudioSource> ();
		BGMBounds = BGMVolSlider.GetComponent<RectTransform> ().rect;
		BGMBounds.xMin += BGMVolSlider.GetComponent<RectTransform>().position.x;
		BGMBounds.xMax += BGMVolSlider.GetComponent<RectTransform>().position.x;
		BGMBounds.yMin += BGMVolSlider.GetComponent<RectTransform>().position.y;
		BGMBounds.yMax += BGMVolSlider.GetComponent<RectTransform>().position.y;
		SEBounds = SEVolSlider.GetComponent<RectTransform> ().rect;
		SEBounds.xMin += SEVolSlider.GetComponent<RectTransform>().position.x;
		SEBounds.xMax += SEVolSlider.GetComponent<RectTransform>().position.x;
		SEBounds.yMin += SEVolSlider.GetComponent<RectTransform>().position.y;
		SEBounds.yMax += SEVolSlider.GetComponent<RectTransform>().position.y;
    }

    // Update is called once per frame
    void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			if (BGMBounds.Contains(Input.mousePosition)) {
				volTest = BGMVolSlider.GetComponent<AudioSource>();
				volTest.loop = true;
				volTest.Play();
			}
			else if (SEBounds.Contains(Input.mousePosition)) {
				volTest = SEVolSlider.GetComponent<AudioSource>();
				volTest.loop = true;
				volTest.Play();
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			volTest.loop = false;
		}
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

	public void ChangeBGMVolume(float newValue)
	{
		BGMVolSlider.GetComponent<AudioSource> ().volume = newValue;
	}

	public void ChangeSEVolume(float newValue)
	{
		SEVolSlider.GetComponent<AudioSource> ().volume = newValue;
	}

	void OnDestroy()
	{
		byte[] toWrite;
		toWrite [0] = BGMVolSlider.GetComponent<AudioSource> ().volume;
		System.IO.FileStream fout = System.IO.File.Open ("GameOptions.cfg", System.IO.FileMode.OpenOrCreate);
		//fout.BeginWrite(BGMVolSlider.GetComponent<AudioSource>().volume, 0, sizeof(float), 
	}
}
