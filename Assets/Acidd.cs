using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Acidd : MonoBehaviour
{
    Animator myanim;
    public GameObject Target;
    public GameObject Owner;
    public List<GameObject> caughtInAOETargets;
    public float speed = .5f;
    // Use this for initialization
    void Start()
    {
        myanim = GetComponent<Animator>();
        //   myanim.StartPlayback();

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Unit" && !other.gameObject.GetComponent<BaseCell>().isMine)
        {

            myanim.SetTrigger("Start");
  //          other.GetComponent<BaseCell>().currentProtein -= Owner.GetComponent<BaseCell>().attackDamage;
            GetComponent<SphereCollider>().radius = 3;
            // myanim.SetTrigger("Acid");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Unit" && !other.gameObject.GetComponent<BaseCell>().isMine)
        {
            if (!IsInvoking("AoeDmg"))
            {
                InvokeRepeating("AoeDmg", 1.0f, 1.0f);

            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsInvoking("AoeDmg"))
        {
            CancelInvoke("AoeDmg");
        }
    }
    
    void DestroyMyself()
    {
        Destroy(this.gameObject);
    }
    void AoeDmg()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject != Owner && hitColliders[i].GetComponent<BaseCell>() && hitColliders[i].GetComponent<BaseCell>().isMine == false)
            {
                caughtInAOETargets.Add(hitColliders[i].gameObject);
            }
        }

       for(int i = 0 ; i < caughtInAOETargets.Count; i++)
       {
           caughtInAOETargets[i].GetComponent<BaseCell>().currentProtein -= Owner.GetComponent<AcidicCell>().attackDamage;

           caughtInAOETargets[i].GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
       }
    }

}


//if (other.gameObject.tag == "Unit" && !other.gameObject.GetComponent<BaseCell>().isMine)
//     {
//         Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
//         for (int i = 0; i < hitColliders.Length; i++)
//         {
//               if (hitColliders[i].gameObject != realOwner && hitColliders[i].GetComponent<BaseCell>() && hitColliders[i].GetComponent<BaseCell>().isMine == false)
//       {
//            targetsToBounce.Add(hitColliders[i].gameObject);
//        }
//         }
//     }