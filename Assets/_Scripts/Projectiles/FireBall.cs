using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBall : BaseProjectile {


	void Awake() {
        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
       
    }

	//LateUpdate is called after all Update functions have been called
	void LateUpdate() {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Unit" && !other.gameObject.GetComponent<BaseCell>().isMine)
        {
            Destroy(this.gameObject);
        }
       
    }
}