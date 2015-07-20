using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBall : BaseProjectile
{
    public GameObject Target;
    public GameObject Owner;
    public float lifeTimer = 1.5f;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.fixedDeltaTime;
        if (lifeTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {

    }

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {

    }
    void OnTriggerEnter(Collider other)
    {


        if (Owner.GetComponent<BaseCell>().isAIPossessed == true && other.gameObject.GetComponent<BaseCell>())
        {
            if (other.gameObject.GetComponent<BaseCell>().isMine == true)
            {
                other.gameObject.GetComponent<BaseCell>().currentProtein = other.gameObject.GetComponent<BaseCell>().currentProtein - Owner.GetComponent<BaseCell>().attackDamage;
                other.gameObject.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                if (PhotonNetwork.connected)
                {
                    other.gameObject.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, Owner.GetComponent<BaseCell>().attackDamage);
                }
                Destroy(this.gameObject);
            }
        }


        if (Owner.GetComponent<BaseCell>().isAIPossessed == false && other.gameObject.GetComponent<BaseCell>())
        {
            if (other.gameObject.GetComponent<BaseCell>().isMine == false)
            {
                other.gameObject.GetComponent<BaseCell>().currentProtein = Target.GetComponent<BaseCell>().currentProtein - Owner.GetComponent<BaseCell>().attackDamage;
                other.gameObject.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                if (PhotonNetwork.connected)
                {
                    other.gameObject.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, Owner.GetComponent<BaseCell>().attackDamage);
                }
                Destroy(this.gameObject);
            }

        }





    }
}

