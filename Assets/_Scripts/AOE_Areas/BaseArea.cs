using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseArea : MonoBehaviour {

    private float effectTimer;
    private float effectAmount;

    public float EffectTimer {
        get { return effectTimer; }
        set { effectTimer = value; }
    }

    public float EffectAmount {
        get { return effectAmount; }
        set { effectAmount = value; }
    }

    public virtual void OnTriggerStay(Collider other) {
        if (/** other.gameObject.tag == "" **/false) {
            // TO-DO
        }
    }
    
	public virtual void Awake() {
        
    }

	public virtual void Start () {
	
	}
	
	public virtual void Update () {
	
	}

	public virtual void FixedUpdate() {
       
    }

	public virtual void LateUpdate() {
        
    }
}