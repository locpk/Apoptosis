using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Lighting : MonoBehaviour
{
    public GameObject Target;
    public GameObject Owner;
    public GameObject realOwner;
    public GameObject nextTarget;
    List<GameObject> targetsToBounce;
    public float speed;
    float bounceRadius = 3;
  public  int bounceCounter = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        targetsToBounce = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, bounceRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject != realOwner && hitColliders[i].GetComponent<BaseCell>())
            {
                targetsToBounce.Add(hitColliders[i].gameObject);
            }

        }
     // for (int i = 0; i < targetsToBounce.Count; i++)
     // {
     //     if (bounceCounter >= 4)
     //     {
     //         break;
     //     }
     //     Vector3 them2me = targetsToBounce[i].transform.position - transform.position;
     //     GetComponent<Rigidbody>().velocity += them2me.normalized * speed;
     //     targetsToBounce[i].GetComponent<BaseCell>().currentProtein -= realOwner.GetComponent<BaseCell>().attackDamage;
     //     bounceCounter++;
     // }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Unit" && !other.gameObject.GetComponent<BaseCell>().isMine)
        {
            Target.GetComponent<BaseCell>().currentProtein = Target.GetComponent<BaseCell>().currentProtein - realOwner.GetComponent<BaseCell>().attackDamage;
      
        for (int i = 0; i < targetsToBounce.Count; i++)
        {
            if(bounceCounter >=4 )
            {
                break;
            }
            Vector3 them2me = targetsToBounce[i].transform.position - transform.position;
            GetComponent<Rigidbody>().velocity += them2me.normalized * speed;
            targetsToBounce[i].GetComponent<BaseCell>().currentProtein -= realOwner.GetComponent<BaseCell>().attackDamage;
            bounceCounter++;
        }


            if (bounceCounter >= 4 || targetsToBounce.Count == 0)
            {
                Destroy(this.gameObject);
            }

        }

    }
}
