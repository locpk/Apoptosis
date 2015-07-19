using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EvolveButtonController : MonoBehaviour {

    public GameObject heatButton;
    public GameObject coldButton;
    public GameObject acidButton;
    public GameObject alkaliButton;

    //private bool isOverUI = false;

    //public void TurnOnOverUI() { isOverUI = true; }
    //public void TurnOffOverUI() { isOverUI = false; }


	// Use this for initialization
	void Start () 
    {
	
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
        if (!heatButton.gameObject.GetActive())
        {
            heatButton.SetActive(true);
        }
        else
        {
            heatButton.SetActive(false);
        }

        if (!coldButton.gameObject.GetActive())
        {
            coldButton.SetActive(true);
        }
        else
        {
            coldButton.SetActive(false);
        }

        if (!acidButton.gameObject.GetActive())
        {
            acidButton.SetActive(true);
        }
        else
        {
            acidButton.SetActive(false);
        }

        if (!alkaliButton.gameObject.GetActive())
        {
            alkaliButton.SetActive(true);
        }
        else
        {
            alkaliButton.SetActive(false);
        }
    }
}
