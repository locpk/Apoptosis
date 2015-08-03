using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(MeshRenderer))]
public class MinimapFOG : MonoBehaviour
{
    private AudioMixerSnapshot snapshot_attack;
    private AudioMixerSnapshot snapshot_normal;

    private Sound_Manager sound_manager; 

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
      //  sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>().GetInstance();
        
      //  snapshot_normal = sound_manager.GetInstance().master_mixer.FindSnapshot("Snapshot");
      //  snapshot_attack = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>().GetInstance().master_mixer.FindSnapshot("Snapshot_Attack");
    }

    void Start()
    {
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>().GetInstance();
        if (sound_manager.GetInstance() != null)
        {
        snapshot_normal = sound_manager.GetInstance().master_mixer.FindSnapshot("Snapshot");
        snapshot_attack = sound_manager.GetInstance().master_mixer.FindSnapshot("Snapshot_Attack");
            
        }
    
    }
   


    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit" && other.GetComponent<BaseCell>().isAIPossessed ) //if the object that entered the sphere radius is a unit
        {
          
   //         snapshot_attack.TransitionTo(3.0f);
            if (!other.GetComponent<BaseCell>().isMine && this.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<BaseCell>().isMine)
            {
            }
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        //if the object leaves the sphere radius, turn the minimap image off
        if (other.tag == "Unit")
        {
         
            if (!other.GetComponent<BaseCell>().isMine && this.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<BaseCell>().isMine)
            {
    //            snapshot_normal.TransitionTo(3.0f);
            }
        }
    }
}
