using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Lighting : MonoBehaviour
{

    public GameObject realOwner = null;
    public GameObject nextTarget;
  public  List<GameObject> targetsToBounce;
    public GameObject previousTarget;
    public GameObject currentTarget = null;
    public float speed;
    float bounceRadius = 3.5f;
    public int bounceCounter = 0;
      float lifetimer = 1.5f;
    // Use this for initialization
    void Start()
    {
      //  realOwner = realOwner.gameObject;
        currentTarget = realOwner.GetComponent<BaseCell>().primaryTarget;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 them2me = currentTarget.transform.position - transform.position;
            GetComponent<Rigidbody>().velocity += them2me.normalized * speed;
         transform.LookAt(transform.position + Vector3.up, -them2me);
            //transform.LookAt(transform.position + Vector3.up, Vector3.Cross(Vector3.up, them2me));
       
        }

        lifetimer -= 1.0f * Time.fixedDeltaTime;
        if(lifetimer <= 0)
        {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        targetsToBounce = new List<GameObject>();

        if (realOwner.GetComponent<BaseCell>().isAIPossessed == false && other.gameObject.GetComponent<BaseCell>())
        {
            if (other.gameObject.GetComponent<BaseCell>().isMine == false)
            {
               
                currentTarget.GetComponent<BaseCell>().currentProtein -= realOwner.GetComponent<BaseCell>().attackDamage;
                currentTarget.GetComponent<BaseCell>().stunned = true;
                if (PhotonNetwork.connected)
                {
                    currentTarget.GetComponent<BaseCell>().photonView.RPC("StunMe", PhotonTargets.Others, null);
                    currentTarget.GetComponent<BaseCell>().photonView.RPC("ApplyDamage", PhotonTargets.Others, realOwner.GetComponent<BaseCell>().attackDamage);
                }
                currentTarget.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                bounceCounter++;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, bounceRadius);
                for (int i = 0; i < hitColliders.Length; i++)
                {


                    if (hitColliders[i].gameObject != realOwner && hitColliders[i].GetComponent<BaseCell>() && hitColliders[i].GetComponent<BaseCell>().isMine == false)
                    {
                        targetsToBounce.Add(hitColliders[i].gameObject);
                    }

                }
                //for (int i = 0; i < targetsToBounce.Count; i++)
                //{

                    nextTarget = targetsToBounce[0];
                    if (nextTarget == currentTarget || nextTarget == previousTarget || nextTarget.GetComponent<BaseCell>().hitBylightning == true)
                    {
                        nextTarget = null;
                    }
                    //break;
                //}
                if (nextTarget != null)
                {
                    previousTarget = currentTarget;
                    currentTarget = nextTarget;

                }






                if (bounceCounter >= 4 || targetsToBounce.Count == 0 || currentTarget == null || nextTarget == null)
                {
                    for (int i = 0; i < targetsToBounce.Count; i++)
                        targetsToBounce[i].GetComponent<BaseCell>().hitBylightning = false;

                    Destroy(this.gameObject);
                }
            }
        }
        else if (realOwner.GetComponent<BaseCell>().isAIPossessed == true && other.gameObject.GetComponent<BaseCell>())

            if (other.gameObject.GetComponent<BaseCell>().isMine == true)
            {
                currentTarget.GetComponent<BaseCell>().currentProtein -= realOwner.GetComponent<BaseCell>().attackDamage;
                currentTarget.GetComponent<BaseCell>().stunned = true;
                if (PhotonNetwork.connected)
                {
                    currentTarget.GetComponent<BaseCell>().photonView.RPC("StunMe", PhotonTargets.Others, null);
                    currentTarget.GetComponent<BaseCell>().photonView.RPC("ApplyDamage", PhotonTargets.Others, realOwner.GetComponent<BaseCell>().attackDamage);
                }
                currentTarget.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                bounceCounter++;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, bounceRadius);
                for (int i = 0; i < hitColliders.Length; i++)
                {


                    if (hitColliders[i].gameObject != realOwner && hitColliders[i].GetComponent<BaseCell>() && hitColliders[i].GetComponent<BaseCell>().isMine == true)
                    {
                        targetsToBounce.Add(hitColliders[i].gameObject);
                    }

                }
                //for (int i = 0; i < targetsToBounce.Count; i++)
                //{

                    nextTarget = targetsToBounce[0];
                    if (nextTarget == currentTarget || nextTarget == previousTarget || nextTarget.GetComponent<BaseCell>().hitBylightning == true )
                    {
                        nextTarget = null;
                    }
                //    break;
                //}
                if (nextTarget != null)
                {
                    previousTarget = currentTarget;
                    currentTarget = nextTarget;

                }


                if (bounceCounter >= 4 || targetsToBounce.Count == 0 || currentTarget == null || nextTarget == null)
                {
                    for (int i = 0; i < targetsToBounce.Count; i++)
                        targetsToBounce[i].GetComponent<BaseCell>().hitBylightning = false;

                    Destroy(this.gameObject);
                }
            }
        }




    
}