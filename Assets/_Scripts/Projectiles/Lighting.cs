using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Lighting : MonoBehaviour
{
    public GameObject Target;
    public GameObject Owner;
    public GameObject realOwner = null;
    public GameObject nextTarget;
  public  List<GameObject> targetsToBounce;
    public GameObject previousTarget;
    public GameObject currentTarget = null;
    public float speed;
    float bounceRadius = 3;
    public int bounceCounter = 0;
    // Use this for initialization
    void Start()
    {
        realOwner = realOwner.gameObject;
        currentTarget = realOwner.GetComponent<BaseCell>().primaryTarget;
        transform.LookAt(currentTarget.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 them2me = currentTarget.transform.position - transform.position;
            GetComponent<Rigidbody>().velocity += them2me.normalized * speed;
            transform.LookAt(currentTarget.transform);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        targetsToBounce = new List<GameObject>();
        if (other.gameObject.tag == "Unit" && !other.gameObject.GetComponent<BaseCell>().isMine)
        {

            currentTarget.GetComponent<BaseCell>().currentProtein -= realOwner.GetComponent<BaseCell>().attackDamage;
            currentTarget.GetComponent<BaseCell>().stunned = true;
            currentTarget.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
            bounceCounter++;
            Collider[] hitColliders = Physics.OverlapSphere (transform.position, bounceRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {


                if (hitColliders[i].gameObject != realOwner && hitColliders[i].GetComponent<BaseCell>() && hitColliders[i].GetComponent<BaseCell>().isMine == false)
                {
                    targetsToBounce.Add(hitColliders[i].gameObject);
                }

            }
            for (int i = 0; i < targetsToBounce.Count; i++)
            {

                nextTarget = targetsToBounce[i];
                if (nextTarget == currentTarget || nextTarget == previousTarget ||  nextTarget.GetComponent<BaseCell>().hitBylightning == true)
                {
                    nextTarget = null;
                }
                break;
            }
            if (nextTarget != null)
            {
                previousTarget = currentTarget;
                currentTarget = nextTarget;

            }






            if (bounceCounter >= 4 || targetsToBounce.Count == 0 || currentTarget == null || nextTarget == null)
            {
                currentTarget.GetComponent<BaseCell>().hitBylightning = false;
                Destroy(this.gameObject);
            }
        }
    }
}