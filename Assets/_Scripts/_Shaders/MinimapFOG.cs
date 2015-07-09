using UnityEngine;
using System.Collections;

public class MinimapFOG : MonoBehaviour
{

    

    // Use this for initialization
    void Start()
    {
        GameObject obj = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        if (obj.tag == "Unit")
        {
            if (!obj.GetComponent<BaseCell>().isMine)
            {
                obj.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }

        if (obj.tag == "Protein")
        {
            obj.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
        {
            if (!other.GetComponent<BaseCell>().isMine)
            {
                GameObject temp = other.gameObject;
                temp.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }

        if (other.tag == "Protein")
        {
            GameObject temp = other.gameObject;
            temp.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Unit")
        {
            if (!other.GetComponent<BaseCell>().isMine)
            {
                GameObject temp = other.gameObject;
                temp.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }

        if (other.tag == "Protein")
        {
            GameObject temp = other.gameObject;
            temp.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Unit")
        {
            if (!other.GetComponent<BaseCell>().isMine)
            {
                GameObject obj = other.gameObject;
                obj.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }

        if (other.tag == "Protein")
        {
            GameObject temp = other.gameObject;
            temp.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }
}
