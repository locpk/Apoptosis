using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class Credits_Automation : MonoBehaviour {

    public GameObject credit_name;
    public GameObject credit_role;
    public GameObject credit_Description;

   
    public float displayDuration;
    private float counter;
    private int index = 0;

    private string[] names = {   "JOHN oleske",
                                 "SHAWN paris",
                                 "CARIS frazier",
                                 "GREG bey",
                                 "DEVIN dellamano",
                                 "JAMEEL knight", 
                                 "JUNSHU chen",
                                 "DMITRII roets",
                                 "DAMIEN mullen",
                                 "CHEN lu",
                                 "THANK you",
                                 ""};

    private string[] roles = {"Executive Producer",
                             "Associate Producer",
                             "Artist",
                             "Artist",
                             "Scripting / Cameras",
                             "Scripting / Mechanics",
                             "Platforms / Animation",
                             "Graphics / UI Scripting / Design",
                             "Networks / Concept / Mechanics",
                             "Level Design / Mechanics",
                             "",
                             ""};

    private string[] descriptions = { "He was kind to his people...at first",// john
                                    "", // paris
                                    "", // caris
                                    "", // greg
                                    "",//devin
                                    "Units, Gameplay",//jameel
                                    "cross-platform, animations, units, gameplay", // JUNSHU chen 
                                    "game design, graphics, menus, HUD",// dmitrii
                                    "tutorial, controlls, multiplayer",// damien
                                    "AI, levels, fog of war", // chen
                                    "Divide and conquer",
                                    ""}; // end

   




    // Use this for initialization
	void Start () {
  
        
        
	}
	
	// Update is called once per frame
	void Update ()  
    {
        counter += Time.deltaTime;
        if ( counter > displayDuration )
        {
            
            credit_name.GetComponent<Text>().text = names[index];
            credit_role.GetComponent<Text>().text = roles[index] + index.ToString();
            credit_Description.GetComponent<Text>().text = descriptions[index];
            index++;
            counter = 0.0f;
        }
        if (index > 11)
        {
            Application.LoadLevel("MainMenu");
           
        }
	}
}
