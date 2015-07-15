using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_ReturnCoundown : MonoBehaviour {

	// Use this for initialization
  
    public float transision_Delay;
    private int displayText;
    private Text text;
	void Start () 
    {
        text = GetComponent<Text>();
        displayText = 0;
	}
	
	// Update is called once per frame
	void Update () {

        // this does the coundown look and feel
        transision_Delay -= Time.deltaTime;
        displayText = (int)transision_Delay;
        text.text = displayText.ToString();

        if (displayText == 0)
        {
            Application.LoadLevel("MainMenu");
        //    screen.GetComponent<Menu>().DisplayLoadingScreen("MainMenu");
            
        }
        
	}
}
