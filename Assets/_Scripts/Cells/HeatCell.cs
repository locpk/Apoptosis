using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeatCell : BaseCell
{
    public GameObject fireball;
    public GameObject Tier2Heat;
    HeatCell mergePartner;
    bool haveMergePartner = false;
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    // float splitCD = 0;
    float fireballSpeed = 10;
    public bool Inheat;
    public PlayerController controller;


    void Merge()
    {



        List<HeatCell> heatCellsMerge = new List<HeatCell>();
        List<BaseCell> possibleMergers = controller.selectedUnits;
        for (int i = 0; i < possibleMergers.Count; i++)
        {

            if (possibleMergers[i].celltype == CellType.HEAT_CELL &&
                possibleMergers[i].GetComponent<HeatCell>().Inheat == true &&
               possibleMergers[i] != this)
            {
                heatCellsMerge.Add(possibleMergers[i].GetComponent<HeatCell>());
            }

        }

        if (heatCellsMerge.Count >= 1)
        {
            for (int o = 0; o < heatCellsMerge.Count; o++)
            {
                if (mergePartner == null || Vector3.Distance(this.transform.position, heatCellsMerge[o].transform.position)
                    < Vector3.Distance(this.transform.position, mergePartner.transform.position) || 
                    (haveMergePartner == false && mergePartner.haveMergePartner == false ))
                {
                    mergePartner = heatCellsMerge[o];
                    mergePartner.mergePartner = this;
                    haveMergePartner = true;
                    mergePartner.haveMergePartner = true;

                }
            }

        }


    }


    void MergingTheCells(HeatCell other)
    {

          float distance = Vector3.Distance(this.transform.position, other.transform.position);
      if (Vector3.Distance(this.transform.position, other.transform.position) < 2.0f)
      {
          Vector3 trackingPos = this.transform.position;
          Quaternion trackingRot = this.transform.rotation;
          Die();
          other.Die();
   
          Instantiate(Tier2Heat, trackingPos, trackingRot);
          GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(Tier2Heat.GetComponent<BaseCell>());
      }
      else
      {
   
        Move(other.transform.position);
   
      }

    }
    void Awake()
    {
        base.bAwake();
        multidamagesources += nothing;
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);

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
    void DamagePreSecond()
    {
        GameObject fire = Instantiate(fireball, transform.position, transform.rotation) as GameObject;


        Vector3 them2me = primaryTarget.transform.position - transform.position;
        fire.GetComponent<Rigidbody>().velocity += them2me.normalized * fireballSpeed;
        primaryTarget.GetComponent<BaseCell>().currentProtein -= (attackDamage / primaryTarget.GetComponent<BaseCell>().defense);
    }



    // Use this for initialization
    void Start()
    {
        base.bStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAIPossessed)
        {
            transform.position = new Vector3(transform.position.x + .00100f, 0, transform.position.z + .00100f);
        }
        else
        {
            switch (currentState)
            {
                case CellState.IDLE:
                    SetPrimaryTarget(null);
                    if (IsInvoking("DamagePreSecond"))
                    {
                        CancelInvoke("DamagePreSecond");
                    }
                    //System.Collections.Generic.List<GameObject> enemyUnits = GameObjectManager.FindAIUnits();
                    //if (enemyUnits != null)
                    //{
                    //    for (int i = 0; i < enemyUnits.Count; i++)
                    //    {
                    //        if (Vector3.Distance(enemyUnits[i].transform.position, transform.position) <= fovRadius)
                    //        {
                    //            if (enemyUnits[i] != this.gameObject)
                    //            {
                    //                Attack(enemyUnits[i]);
                    //            }
                    //            break;

                    //        }
                    //    }
                    //}
                    if (Input.GetKeyUp(KeyCode.Q) && isSelected)
                    {
                        Merge();
                    }
               
                    break;
                case CellState.ATTACK:
                    if (primaryTarget != null)
                    {
                        if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                        {
                            if (!IsInvoking("DamagePreSecond"))
                            {
                                InvokeRepeating("DamagePreSecond", 1.0f, 1.0f);

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
                case CellState.CONSUMING:
                    base.bUpdate();
                    break;
                case CellState.MOVING:
                    if (IsInvoking("DamagePreSecond"))
                    {
                        CancelInvoke("DamagePreSecond");
                    }
                    base.bUpdate();
                    break;
                case CellState.ATTACK_MOVING:
                    if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                    {
                        currentState = CellState.IDLE;
                    }
                    break;
                case CellState.DEAD:
                    base.Die();
                    break;

                default:
                    break;
            }
            if (mergePartner != null)
                MergingTheCells(mergePartner);
        }
    }

    void FixedUpdate()
    {
        base.bFixedUpdate();
    }
    public override void Attack(GameObject _target)
    {
        if (_target)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }


    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        base.bLateUpdate();
    }

    void Evolve(GameObject other)
    {

    }
}
