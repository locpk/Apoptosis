using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(MeshRenderer))]
public class MinimapFOG : MonoBehaviour
{
    public AudioMixerSnapshot snapshot_attack;
    public AudioMixerSnapshot snapshot_normal;

    // Use this for initialization
    void Awake()
    {
        //grab the object's "grandparent" that is attached to this script
        GameObject obj = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        if (obj.tag == "Unit") //check if it is a cell unit
        {
            
            if (!obj.GetComponent<BaseCell>().isMine) //if the cell is not mine, 
            {
                obj.transform.FindChild("MinimapIndicator").GetComponent<MeshRenderer>().enabled = false; //turn the minimap image of it off
            }
        }
        if (obj.tag == "Protein") //check if it is a protein,
        {
            obj.transform.FindChild("MinimapIndicator").GetComponent<MeshRenderer>().enabled = false; //turn the minimap image of it off
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
          
            if(snapshot_attack) snapshot_attack.TransitionTo(3.0f);
            //if (!other.GetComponent<BaseCell>().isMine && this.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<BaseCell>().isMine)
            //{
            //}
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        //if the object leaves the sphere radius, turn the minimap image off
        if (other.tag == "Unit")
        {
         
            if (snapshot_normal) snapshot_normal.TransitionTo(3.0f);
            //if (!other.GetComponent<BaseCell>().isMine && this.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<BaseCell>().isMine)
            //{
            //}
        }
    }
}
