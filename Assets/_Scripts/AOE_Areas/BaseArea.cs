using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseArea : MonoBehaviour {

    public enum AreaType {
        HEAT_AREA, COLD_AREA, ACIDIC_AREA, ALKALI_AREA, DENSE_AREA, AREA_COUNTS
    }

    public AreaType areaType;

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