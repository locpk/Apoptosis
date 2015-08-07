using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour {

    public Button button;
    
	// Use this for initialization
	void Start () {
        button.GetComponent<Button>().interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
