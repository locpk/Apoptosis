using UnityEngine;
using System.Collections;

public class MoveWaypoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit" && other.GetComponent<BaseCell>().isSelected)
        {
            Destroy(gameObject);
        }
    }
}
