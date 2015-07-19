using UnityEngine;
using System.Collections;

public class particles : MonoBehaviour {
  public  float lifetimer = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifetimer -= 1 * Time.fixedDeltaTime;
        if (lifetimer <= 0)
        {
            DestroyImmediate(gameObject);
      
        }
	}
}
