using UnityEngine;
using System.Collections;

public class stun : MonoBehaviour {
    float lifetimer = 3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifetimer -= 1 * Time.fixedDeltaTime;
        if (lifetimer <= 0)
            Destroy(this.gameObject);
	}
}
