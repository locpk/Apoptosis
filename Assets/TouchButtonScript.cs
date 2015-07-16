using UnityEngine;
using System.Collections;

public class TouchButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Input.touchSupported)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
	

}
