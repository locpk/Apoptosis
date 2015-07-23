using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class Credits_Automation : MonoBehaviour {

    public GameObject credit_name;
    public GameObject credit_role;
    public GameObject credit_Description;

    private Animator animator;
    public float displayDuration;
    private float counter;
    private int index;

    private List<string> names;
    private List<string> roles;
    private List<string> descriptions;

    struct Credit
	{
        GameObject role;
        GameObject name;
        GameObject description;
	}


    // Use this for initialization
	void Start () {

        index = 0;

        animator = credit_name.GetComponent<Animator>();
      
        
        names.Add("JOHN folseke");       roles.Add("Executive Producer");                descriptions.Add("He was kind to his people...at first");
        names.Add("SHAWN paris");       roles.Add("Associate Producer");                descriptions.Add("");
        names.Add("CARIS frazier");     roles.Add("Artist");                            descriptions.Add("");
        names.Add("GREG bey");          roles.Add("Artist");                            descriptions.Add("");
        names.Add("DEVIN dellamo");     roles.Add("Scripting / Cameras");               descriptions.Add("");
        names.Add("JAMEEL knigh");      roles.Add("Scripting / Mechanics");             descriptions.Add("");
        names.Add("JENSHU chen");       roles.Add("Platforms / Animation");             descriptions.Add("");
        names.Add("DMITRII roets");     roles.Add("Graphics / UI Scripting / Design");  descriptions.Add("");
        names.Add("DAMIEN mullen");     roles.Add("Networks / Concept / Mechanics");    descriptions.Add("");
        names.Add("CHEN lu");           roles.Add("Level Design / Mechanics");          descriptions.Add("");
	}
	
	// Update is called once per frame
	void Update ()  
    {
        counter += Time.deltaTime;
      
        credit_name.GetComponent<Text>().text = names[index];
        if ( (counter >= displayDuration) && !(index < names.Count))
        {
            credit_name.GetComponent<Text>().text = names[index];
            credit_role.GetComponent<Text>().text = roles[index];
            credit_Description.GetComponent<Text>().text = descriptions[index];
            index++;
            counter = 0.0f;
        }
	}
}
