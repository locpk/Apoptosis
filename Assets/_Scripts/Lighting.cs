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
    public GameObject previousTarget;
    public GameObject currentTarget;
    public float speed;
    float bounceRadius = 6;
    public int bounceCounter = 0;
    // Use this for initialization
    void Start()
    {
        currentTarget = realOwner.GetComponent<BaseCell>().primaryTarget;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Unit" && !other.gameObject.GetComponent<BaseCell>().isMine)
        {

            currentTarget.GetComponent<BaseCell>().currentProtein -= realOwner.GetComponent<BaseCell>().attackDamage;
            bounceCounter++;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, bounceRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject != realOwner && hitColliders[i].GetComponent<BaseCell>() && !hitColliders[i].GetComponent<BaseCell>().isMine)
                {
                    targetsToBounce.Add(hitColliders[i].gameObject);
                }

            }
            for (int i = 0; i < targetsToBounce.Count; i++)
            {
                nextTarget = targetsToBounce[i];
                if (targetsToBounce[i] != currentTarget || targetsToBounce[i] == previousTarget)
                {
                    nextTarget = null;
                    break;
                }
                break;
            }
            if (nextTarget != null)
            {
                previousTarget = currentTarget;
                currentTarget = nextTarget;
            }
            Vector3 them2me = currentTarget.transform.position - transform.position;
            GetComponent<Rigidbody>().velocity += them2me.normalized * speed;





            if (bounceCounter >= 4 || targetsToBounce.Count == 0 || nextTarget == null)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

//     if (nextTarget != null)
//     {
//         if (nextTarget == previousTarget)
//         {
//             Destroy(this.gameObject);
//             Destroy(this);
//
//         }
//         currentTarget = nextTarget;
//         Vector3 them2me = currentTarget.transform.position - transform.position;
//         GetComponent<Rigidbody>().velocity += them2me.normalized * speed;
//     }
//
//
//     if (bounceCounter >= 4 || targetsToBounce.Count == 0 || nextTarget == null)
//     {
//         Destroy(this.gameObject);
//     }
//
// }



