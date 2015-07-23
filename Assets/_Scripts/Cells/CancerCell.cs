using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CancerCell : BaseCell
{
    public GameObject stun;
   int instanonce = 0;
     void Awake()
    {
        base.bAwake();
        isAIPossessed = true;
        isMine = false;

    }

    // Use this for initialization
     void Start()
    {
        base.bStart();
       if (isAIPossessed)
       {
           navAgent.enabled = false;
           
       }
    }

    // Update is called once per frame
     void Update()
     {

     
         if (stunned == true)
         {
             if (instanonce < 1)
             {
                 Vector3 trackingPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                 GameObject.Instantiate(stun, trackingPos, transform.rotation);
             }
                instanonce++;

             stunTimer -= 1 * Time.fixedDeltaTime;
             if (this.stunTimer <= 0)
             {
                 instanonce=0;
                // Destroy(stun.gameObject);
                 this.stunTimer = 3;
                 this.stunned = false;
                 this.hitCounter = 0;
 
             }
         }
         else
         {
             if (targets != null && targets.Count > 1)
             {

                 if (primaryTarget == null)
                 {
                     for (int i = 0; i < targets.Count; i++)
                     {

                         if (i != targets.Count)
                         {
                             Debug.Log(primaryTarget);
                             primaryTarget = targets[i + 1];
                             Debug.Log(primaryTarget);
                             if (primaryTarget.GetComponent<BaseCell>())
                                 currentState = CellState.ATTACK;
               
                             break;
                         }
                     }
                 }
             }

             switch (currentState)
             {
                 case CellState.IDLE:
                     break;
                 case CellState.ATTACK:
                     break;
                 case CellState.MOVING:
                     break;
                 case CellState.ATTACK_MOVING:
                     break;
                 case CellState.DEAD:
                     base.Die();
            if (PhotonNetwork.connected)
            {
                photonView.RPC("Die", PhotonTargets.Others, null);
            }
                     break;
                 case CellState.CANCEROUS_SPLITTING:
                     break;
                 case CellState.PERFECT_SPLITTING:
                     break;
                 case CellState.EVOLVING:
                     break;
                 case CellState.INCUBATING:
                     break;
                 case CellState.MERGING:
                     break;
                 default:
                     break;
             }
             base.bUpdate();
         }
     }
     void FixedUpdate()
    {
        base.bFixedUpdate();
    }

    //LateUpdate is called after all Update functions have been called
     void LateUpdate()
    {
        base.bLateUpdate();
    }
}
