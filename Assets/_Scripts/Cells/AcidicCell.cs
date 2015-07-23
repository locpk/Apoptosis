using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcidicCell : BaseCell
{

    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject stun;
    public GameObject Acid;
    int instanonce = 0;
    public AlkaliCell mergePartner;
    public bool haveMergePartner = false;
    public GameObject nerveCell;

    void Awake()
    {
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
        base.bAwake();
        multidamagesources += nothing;
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
    
    }

    public void Merge()
    {
        List<BaseCell> alkaliCellMerge;
        List<BaseCell> possibleMergers = pcontroller.selectedUnits;

        alkaliCellMerge = possibleMergers.FindAll(item => item.celltype == CellType.ALKALI_CELL && item.GetComponent<AlkaliCell>());

        if (alkaliCellMerge.Count >= 1)
        {
            for (int i = 0; i < alkaliCellMerge.Count; i++)
            {
                if (mergePartner == null || Vector3.Distance(this.transform.position, alkaliCellMerge[i].transform.position)
                          < Vector3.Distance(this.transform.position, mergePartner.transform.position) ||
                     (haveMergePartner == false && mergePartner.haveMergePartner == false))
                {
                    if (mergePartner != null)
                    {
                        break;
                    }
                    mergePartner = alkaliCellMerge[i].GetComponent<AlkaliCell>();
                    mergePartner.mergePartner = this;
                    haveMergePartner = true;
                    mergePartner.haveMergePartner = true;
                }
            }
        }
    }

    void MergingTheCells(AlkaliCell other)
    {

        float distance = Vector3.Distance(this.transform.position, other.transform.position);
        if (distance < GetComponent<SphereCollider>().radius *1.3f)
        {
            Vector3 trackingPos = this.transform.position;
            Quaternion trackingRot = this.transform.rotation;



            GameObject knerveCell = Instantiate(nerveCell, trackingPos, trackingRot) as GameObject;

            if (!sound_manager.sounds_evolution[5].isPlaying)
            {
                sound_manager.sounds_evolution[5].Play();
            }
            Deactive();
            other.Deactive();
            pcontroller.AddNewCell(knerveCell.GetComponent<BaseCell>());
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
            GameObject kAcid = Instantiate(Acid, transform.position, transform.rotation) as GameObject;
            kAcid.GetComponent<Acidd>().Target = primaryTarget;
            kAcid.GetComponent<Acidd>().Owner = this.gameObject;
            Vector3 them2me = kAcid.GetComponent<Acidd>().Target.transform.position - transform.position;
            kAcid.GetComponent<Rigidbody>().velocity += them2me.normalized * kAcid.GetComponent<Acidd>().speed;

            if (!sound_manager.sounds_attacks[2].isPlaying)
            {
                sound_manager.sounds_attacks[2].Play();

            }
        }
    }
    void MUltiDMg()
    {
        multidamagesources();
       

    }
    public void AreaDamage()
    {
        currentProtein -= 10;
    }
    void nothing()
    {

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
                Destroy(stun.gameObject);
                this.stunTimer = 3;
                this.stunned = false;
                this.hitCounter = 0;
                return;
            }
        }
    
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

                            if (primaryTarget.GetComponent<BaseCell>())
                                currentState = CellState.ATTACK;
                            if (primaryTarget.GetComponent<Protein>())
                                currentState = CellState.CONSUMING;
                            break;
                        }
                    }
                }
            }
            switch (currentState)
            {
                case CellState.IDLE:
                       SetPrimaryTarget(null);
                    if (IsInvoking("DamagePreSecond"))
                    {
                        CancelInvoke("DamagePreSecond");
                    }
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
                            if (Vector3.Distance(primaryTarget.transform.position, transform.position) > attackRange)
                            {
                                base.ChaseTarget();
                            }
                        }
                    }
                        else
                        {
                            currentState = CellState.IDLE;
                        }
                    

                    
                
                    break;
                case CellState.MOVING:
                    base.bUpdate();
                    if (primaryTarget != null)
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
                    break;
                case CellState.ATTACK_MOVING:
                    break;
                case CellState.CONSUMING:
                    base.bUpdate();
                    break;
                case CellState.DEAD:
                    base.Die();
                    break;


                default:
                    break;
            }
            base.bUpdate();
            if (mergePartner != null)
                MergingTheCells(mergePartner);
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
