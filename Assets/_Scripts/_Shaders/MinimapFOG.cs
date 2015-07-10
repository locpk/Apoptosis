using UnityEngine;
using System.Collections;

public class MinimapFOG : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //grab the object's "grandparent" that is attached to this script
        GameObject obj = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        if (obj.tag == "Unit") //check if it is a cell unit
        {
            if (!obj.GetComponent<BaseCell>().isMine) //if the cell is not mine, 
            {
                obj.GetComponentInChildren<MeshRenderer>().enabled = false; //turn the minimap image of it off
            }
        }
        else if (obj.tag == "Protein") //check if it is a protein,
        {
            obj.GetComponentInChildren<MeshRenderer>().enabled = false; //turn the minimap image of it off
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit") //if the object that entered the sphere radius is a unit
        {
            if (!other.GetComponent<BaseCell>().isMine) //and is not mine
            {
                other.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true; //turn it's minimap image on
            }
        }
        else if (other.tag == "Protein") //if the object that entered the sphere radius is a protein
        {
            other.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true; //turn it's minimap image on
        }
    }

    void OnTriggerStay(Collider other)
    {
        //while the object is still inside the sphere, keep the minimap image on (this will keep the image on while the object is still
        //                                                                        within any of your cell's radii)
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
        //if the object leaves the sphere radius, turn the minimap image off
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
