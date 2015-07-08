using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MM_ShowHighlight : MonoBehaviour {

    public GameObject highlight;
    

	// Use this for initialization
	void Start () {

     //   highlight.GetComponent<Image>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    /// <summary>
    /// why is this not working ??? 
    /// </summary>


    void Select()
    { 
        highlight.GetComponent<Image>().enabled = true;
    
    }

     void OnMouseEnter () 
    {

    }


     void OnMouseOver()
     {
         highlight.GetComponent<Image>().enabled = true;
     }
     // ...and the mesh finally turns white when the mouse moves away.
     void OnMouseExit()
     {
         highlight.GetComponent<Image>().enabled = false;
     }
}
