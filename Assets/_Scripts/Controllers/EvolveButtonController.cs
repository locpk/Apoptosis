using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EvolveButtonController : MonoBehaviour {

    public GameObject heatButton;
    public GameObject coldButton;
    public GameObject acidButton;
    public GameObject alkaliButton;
    private Animator animator;
  

    //private bool isOverUI = false;

    private bool show = false;
    //public void TurnOnOverUI() { isOverUI = true; }
    //public void TurnOffOverUI() { isOverUI = false; }


	// Use this for initialization
	void Start () 
    {
        animator = GameObject.FindGameObjectWithTag("Evolve_Animator").GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //if (heatButton.GetActive()
        //    && coldButton.GetActive()
        //    && acidButton.GetActive()
        //    && alkaliButton.GetActive())
        //{
            
        //}

	}

    public void CheckButtons()
    {
        show = !show;
        if (show)
        {
            heatButton.SetActive(true);
            coldButton.SetActive(true);
            acidButton.SetActive(true);
            alkaliButton.SetActive(true);
            animator.SetBool("SHOW_BUTTONS", show);
           
        }
        else
        {
            animator.SetBool("SHOW_BUTTONS", show);
            heatButton.SetActive(false);
            coldButton.SetActive(false);
            acidButton.SetActive(false);
            alkaliButton.SetActive(false);
        }
      //  animator.SetBool("SHOW_BUTTONS", true);
      //  if (!heatButton.gameObject.GetActive())
      //  {
      //      heatButton.SetActive(true);
      //      animator.SetBool("SHOW_BUTTONS",true);
      //  }
      //  else
      //  {
      //      heatButton.SetActive(false);
      //      animator.SetBool("SHOW_BUTTONS", false);
      //  }
      //
    //    if (!coldButton.gameObject.GetActive())
    //    {
    //        coldButton.SetActive(true);
    //    }
    //    else
    //    {
    //        coldButton.SetActive(false);
    //    }
    //
    //    if (!acidButton.gameObject.GetActive())
    //    {
    //        acidButton.SetActive(true);
    //    }
    //    else
    //    {
    //        acidButton.SetActive(false);
    //    }
    //
    //    if (!alkaliButton.gameObject.GetActive())
    //    {
    //        alkaliButton.SetActive(true);
    //    }
    //    else
    //    {
    //        alkaliButton.SetActive(false);
    //    }
    }

    public void Hide_Evolve_Buttons()
    {
        animator.SetBool("SHOW_BUTTONS", false);
        show = !show;
    
    }
}
