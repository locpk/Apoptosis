using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    AudioSource volTest;
    GameObject BGMVolSlider;
    GameObject SEVolSlider;
    GameObject FSToggle;
    Rect BGMBounds;
    Rect SEBounds;
    bool changed;
    byte[] configuration;
    System.IO.FileStream configFile;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        BGMVolSlider = GameObject.Find("BGM_Vol_Slider");
        SEVolSlider = GameObject.Find("SE_Vol_Slider");
        FSToggle = GameObject.Find("FullScreen_Toggle");
        volTest = BGMVolSlider.GetComponent<AudioSource>();
        BGMBounds = BGMVolSlider.GetComponent<RectTransform>().rect;
        BGMBounds.xMin += BGMVolSlider.GetComponent<RectTransform>().position.x;
        BGMBounds.xMax += BGMVolSlider.GetComponent<RectTransform>().position.x;
        BGMBounds.yMin += BGMVolSlider.GetComponent<RectTransform>().position.y;
        BGMBounds.yMax += BGMVolSlider.GetComponent<RectTransform>().position.y;
        SEBounds = SEVolSlider.GetComponent<RectTransform>().rect;
        SEBounds.xMin += SEVolSlider.GetComponent<RectTransform>().position.x;
        SEBounds.xMax += SEVolSlider.GetComponent<RectTransform>().position.x;
        SEBounds.yMin += SEVolSlider.GetComponent<RectTransform>().position.y;
        SEBounds.yMax += SEVolSlider.GetComponent<RectTransform>().position.y;
        if (!System.IO.File.Exists("OptionsMenu.cfg"))
        {
            configFile = new System.IO.FileStream("OptionsMenu.cfg", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);
            byte[] emptyparams = new byte[8];
            configFile.Write(emptyparams, 0, 8);
            configFile.Close();
        }
        else
        {
            configFile = new System.IO.FileStream("OptionsMenu.cfg", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            configuration = new byte[8];
            configFile.Read(configuration, 0, 8);
            BGMVolSlider.GetComponent<UnityEngine.UI.Slider>().value = System.BitConverter.ToSingle(configuration, 0);
            SEVolSlider.GetComponent<UnityEngine.UI.Slider>().value = System.BitConverter.ToSingle(configuration, sizeof(float));
            configFile.Close();
            configFile = new System.IO.FileStream("OptionsMenu.cfg", System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.None);
            FSToggle.GetComponent<UnityEngine.UI.Toggle>().isOn = Screen.fullScreen;
        }
        changed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FSToggle.GetActive() && Application.platform == RuntimePlatform.Android)
        {
            FSToggle.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (BGMBounds.Contains(Input.mousePosition))
            {
                volTest = BGMVolSlider.GetComponent<AudioSource>();
                volTest.loop = true;
                volTest.Play();
            }
            else if (SEBounds.Contains(Input.mousePosition))
            {
                volTest = SEVolSlider.GetComponent<AudioSource>();
                volTest.loop = true;
                volTest.Play();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            volTest.loop = false;
            changed = true;
        }
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void ChangeBGMVolume(float newValue)
    {
        BGMVolSlider.GetComponent<AudioSource>().volume = newValue;
    }

    public void ChangeSEVolume(float newValue)
    {
        SEVolSlider.GetComponent<AudioSource>().volume = newValue;
    }

    void OnDestroy()
    {
        if (changed)
        {
            configFile.Write(System.BitConverter.GetBytes(BGMVolSlider.GetComponent<UnityEngine.UI.Slider>().value), 0, sizeof(float));
            configFile.Write(System.BitConverter.GetBytes(SEVolSlider.GetComponent<UnityEngine.UI.Slider>().value), 0, sizeof(float));
            configFile.Close();
        }
    }
}
