using UnityEngine;
using System.Collections;

public class MM_OptionsEffect : MonoBehaviour {

    private Animator animator;

    //private GameObject OptionsPanelObject;
    //private Animator options_animator;
	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();
        //OptionsPanelObject = GameObject.FindGameObjectWithTag("Options_Panel");
        //options_animator = OptionsPanelObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Slide_Down()
    {
        animator.SetBool("b_MoveDown", true);
        
    }

    public void Slide_Up()
    {
        animator.SetBool("b_MoveDown", false);

    }
}
