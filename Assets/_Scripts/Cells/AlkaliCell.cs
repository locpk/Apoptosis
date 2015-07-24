using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AlkaliCell : BaseCell
{
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject DOT;

    public GameObject stun;
    int instanonce = 0;
    public AcidicCell mergePartner;
    public bool haveMergePartner = false;
    public GameObject nerveCell;
    
    void Awake()
    {
        base.bAwake();
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);

        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
    }
   
    void MUltiDMg() {
        if (multidamagesources != null)
            multidamagesources();
    }

    public void AreaDamage()
    {
        currentProtein -= 10;
    }
    public void Merge()
    {
        List<BaseCell> acidicCellMerge;
        List<BaseCell> possibleMergers = pcontroller.selectedUnits;

        acidicCellMerge = possibleMergers.FindAll(item => item.celltype == CellType.ACIDIC_CELL && item.GetComponent<AcidicCell>());

        if (acidicCellMerge.Count >= 1)
        {
            for (int i = 0; i < acidicCellMerge.Count; i++)
            {
                if (mergePartner == null || Vector3.Distance(this.transform.position, acidicCellMerge[i].transform.position)
                          < Vector3.Distance(this.transform.position, mergePartner.transform.position) ||
                     (haveMergePartner == false && mergePartner.haveMergePartner == false))
                {
                    if (mergePartner != null)
                    {
                        break;
                    }
                    mergePartner = acidicCellMerge[i].GetComponent<AcidicCell>();
                    mergePartner.mergePartner = this;
                    haveMergePartner = true;
                    mergePartner.haveMergePartner = true;
                }
            }
        }
    }
    void MergingTheCells(AcidicCell other)
    {

        float distance = Vector3.Distance(this.transform.position, other.transform.position);
        if (distance < GetComponent<SphereCollider>().radius *1.3f)
        {
            Vector3 trackingPos = this.transform.position;
            Quaternion trackingRot = this.transform.rotation;



            GameObject knerveCell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("NerveCell", trackingPos, trackingRot, 0, new object[] {(bool)false} )
                : Instantiate(nerveCell, trackingPos, trackingRot) as GameObject;

            if (!sound_manager.sounds_evolution[5].isPlaying)
            {
                sound_manager.sounds_evolution[5].Play();
            }
            Deactive();
            other.Deactive();
        }
        else
        {

            Move(other.transform.position);

        }

    }
    void DamagePreSecond()
    {
        if (primaryTarget != null)
        {
            //previousTarget = primaryTarget;
            Vector3 newvec = new Vector3(primaryTarget.transform.position.x, primaryTarget.transform.position.y, (primaryTarget.transform.position.z + primaryTarget.GetComponent<SphereCollider>().radius));
            GameObject theDOT = PhotonNetwork.connected ? PhotonNetwork.Instantiate("DOT", newvec, primaryTarget.transform.rotation, 0)
                : Instantiate(DOT, newvec, primaryTarget.transform.rotation) as GameObject;

            theDOT.GetComponent<Dot>().Target = primaryTarget;
            theDOT.GetComponent<Dot>().Owner = this.gameObject;


            if (!sound_manager.sounds_attacks[4].isPlaying)
            {
                sound_manager.sounds_attacks[4].Play();
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        base.bStart();
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
            if (stunTimer > 0)
            {
                navAgent.enabled = false;
                navObstacle.enabled = true;
                primaryTarget = null;

                return;
            }
            if (this.stunTimer <= 0)
            {
                instanonce = 0;
                // Destroy(stun.gameObject);
                this.stunTimer = 3;
                this.stunned = false;
                this.hitCounter = 0;

            }
        }
        else
        {
            if (targets != null && targets.Count >= 1)
            {

                if (primaryTarget == null)
                {
                    for (int i = 0; i < targets.Count; i++)
                    {

                        if (i != targets.Count)
                        {

                            if (i == 0 && targets.Count == 1)
                                primaryTarget = targets[i];
                            else
                                primaryTarget = targets[i + 1];

                            if (primaryTarget != null)
                            {
                            if (primaryTarget.GetComponent<BaseCell>())
                                currentState = CellState.ATTACK;
                            if (primaryTarget.GetComponent<Protein>())
                                currentState = CellState.CONSUMING;
                            }
                            break;
                        }
                    }
                }
            }
            switch (currentState)
            {
                case CellState.IDLE:

                    Guarding();
                    break;
                case CellState.ATTACK:
                    if (primaryTarget != null)
                    {
                        if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                        {
                            if (!IsInvoking("DamagePreSecond"))
                            {
                                InvokeRepeating("DamagePreSecond", 1.0f, 3.0f);

                            }
                        }
                        else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                        {
                            if (IsInvoking("DamagePreSecond"))
                            {
                                CancelInvoke("DamagePreSecond");
                            }
                            base.ChaseTarget();
                        }

                    }
                    else
                    {
                        currentState = CellState.IDLE;
                    }
                    break;

                case CellState.MOVING:
                    base.bUpdate();
                    if (primaryTarget && base.isStopped())
                    {
                        if (primaryTarget.GetComponent<BaseCell>())
                        {
                            currentState = CellState.ATTACK;
                        }
                        else if (primaryTarget.GetComponent<Protein>())
                        {
                            currentState = CellState.CONSUMING;
                        }
                    }
                    else if (!primaryTarget || base.isStopped())
                    {
                        currentState = CellState.IDLE;
                    }


                    break;
                case CellState.ATTACK_MOVING:
                    // if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                    // {
                    //     currentState = CellState.IDLE;
                    // }
                    base.bUpdate();
                    break;
                case CellState.DEAD:
                    base.Die();
                    if (PhotonNetwork.connected)
                    {
                        photonView.RPC("Die", PhotonTargets.Others, null);
                    }
                    break;
                case CellState.CONSUMING:
                    base.bUpdate();
                    break;
                default:
                    break;
            }
            if (mergePartner != null)
                MergingTheCells(mergePartner);
        }
    }



    public override void Attack(GameObject _target)
    {
        if (_target)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }



    void FixedUpdate()
    {
        base.bFixedUpdate();
    }

    void LateUpdate()
    {
        base.bLateUpdate();
    }
}
