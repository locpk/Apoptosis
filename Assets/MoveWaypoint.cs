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
        if (other.tag == "Unit" && !other.GetComponent<BaseCell>().isMine)
        {
            EventManager.Attack(other.gameObject);
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Destroy(gameObject,5.0f);
        }
        else if (other.tag == "Protein")
        {
            EventManager.Consume(other.gameObject);
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
            Destroy(gameObject, 5.0f);
        }
    }
}
