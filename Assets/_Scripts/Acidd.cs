using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Acidd : MonoBehaviour
{
    Animator myanim;
    public GameObject Target;
    public GameObject Owner;
    public List<GameObject> caughtInAOETargets;
    public float speed = 8.0f;
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

        if (Owner.GetComponent<BaseCell>().isAIPossessed == true && other.gameObject.GetComponent<BaseCell>())
        {
            if (other.gameObject.GetComponent<BaseCell>().isMine == true)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                myanim.SetTrigger("Start");
                other.gameObject.GetComponent<BaseCell>().currentProtein = other.gameObject.GetComponent<BaseCell>().currentProtein - Owner.GetComponent<BaseCell>().attackDamage;
                other.gameObject.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                GetComponent<SphereCollider>().radius = 3;

            }
        }

        if (Owner.GetComponent<BaseCell>().isAIPossessed == false && other.gameObject.GetComponent<BaseCell>())
        {
            if (other.gameObject.GetComponent<BaseCell>().isMine == false)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                myanim.SetTrigger("Start");
                other.gameObject.GetComponent<BaseCell>().currentProtein = other.gameObject.GetComponent<BaseCell>().currentProtein - Owner.GetComponent<BaseCell>().attackDamage;
                other.gameObject.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
            }
        }

    }
    void OnTriggerStay(Collider other)
    {


        if (!IsInvoking("AoeDmg"))
        {
            InvokeRepeating("AoeDmg", 1.0f, 1.0f);

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
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    void AoeDmg()
    {
        if (Owner != null && Owner.GetComponent<BaseCell>().isAIPossessed == true) // Ai attack
        {
            caughtInAOETargets = new List<GameObject>();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject != Owner && hitColliders[i].GetComponent<BaseCell>() && hitColliders[i].GetComponent<BaseCell>().isMine == true)
                {
                    caughtInAOETargets.Add(hitColliders[i].gameObject);
                }
            }
            for (int i = 0; i < caughtInAOETargets.Count; i++)
            {
                caughtInAOETargets[i].GetComponent<BaseCell>().currentProtein -= Owner.GetComponent<AcidicCell>().attackDamage;

                caughtInAOETargets[i].GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                if (PhotonNetwork.connected)
                {
                    caughtInAOETargets[i].gameObject.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, Owner.GetComponent<AcidicCell>().attackDamage);
                }
            }
        }
        else if (Owner != null)
        {
            caughtInAOETargets = new List<GameObject>();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject != Owner && hitColliders[i].GetComponent<BaseCell>() && hitColliders[i].GetComponent<BaseCell>().isMine == false)
                {
                    caughtInAOETargets.Add(hitColliders[i].gameObject);
                }
            }
            for (int i = 0; i < caughtInAOETargets.Count; i++)
            {
                caughtInAOETargets[i].GetComponent<BaseCell>().currentProtein -= Owner.GetComponent<AcidicCell>().attackDamage;

                caughtInAOETargets[i].GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                if (PhotonNetwork.connected)
                {
                    caughtInAOETargets[i].gameObject.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, Owner.GetComponent<AcidicCell>().attackDamage);
                }
            }
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