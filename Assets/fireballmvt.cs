using UnityEngine;
using System.Collections;

public class fireballmvt : MonoBehaviour {

	// Use this for initialization

   public  Vector3 vel;
   public GameObject Owner;
   Rigidbody rb;
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if( Owner.GetComponent<BaseCell>().primaryTarget != null)
        {
        Vector3 Owner2Target = Owner.GetComponent<BaseCell>().primaryTarget.transform.position - Owner.GetComponent<BaseCell>().transform.position;
        transform.LookAt(Owner.GetComponent<BaseCell>().primaryTarget.transform);
        rb.AddForce(transform.forward);

        }
        
        
	}
}
