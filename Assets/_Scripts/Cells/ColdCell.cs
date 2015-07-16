using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColdCell : BaseCell
{
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    ColdCell mergingPartner;
    bool haveMergingPartner = false;
    public bool InCold;
    public PlayerController controller;
    public GameObject Tier2Cold;


    void Awake()
    {
        base.bAwake();
        multidamagesources += nothing;
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
        controller = GameObject.Find("PlayerControl").GetComponent<PlayerController>();

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


    public void Merge()
    {
        List<ColdCell> coldCellsMerge = new List<ColdCell>();
        List<BaseCell> possibleMergers = controller.selectedUnits;
        for (int i = 0; i < possibleMergers.Count; i++)
        {

            if (possibleMergers[i].celltype == CellType.COLD_CELL &&
                possibleMergers[i].GetComponent<ColdCell>().InCold == true &&
               possibleMergers[i] != this)
            {
                coldCellsMerge.Add(possibleMergers[i].GetComponent<ColdCell>());
            }

        }

        if (coldCellsMerge.Count >= 1)
        {
            for (int o = 0; o < coldCellsMerge.Count; o++)
            {
                if (mergingPartner == null || Vector3.Distance(this.transform.position, coldCellsMerge[o].transform.position)
                    < Vector3.Distance(this.transform.position, mergingPartner.transform.position) ||
                    (haveMergingPartner == false && mergingPartner.haveMergingPartner == false))
                {
                    mergingPartner = coldCellsMerge[o];
                    mergingPartner.haveMergingPartner = this;
                    haveMergingPartner = true;
                    mergingPartner.haveMergingPartner = true;

                }
            }

        }


    }

    void MergingTheCells(ColdCell other)
    {

        float distance = Vector3.Distance(this.transform.position, other.transform.position);
        if (distance <= GetComponent<SphereCollider>().radius * 2.0f)
        {
            Vector3 trackingPos = this.transform.position;
            Quaternion trackingRot = this.transform.rotation;
            GameObject cTier2Cold = Instantiate(Tier2Cold, trackingPos, trackingRot) as GameObject;
            cTier2Cold.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
            cTier2Cold.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
            cTier2Cold.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
            cTier2Cold.GetComponent<CellSplitAnimation>().originCell = this;
            cTier2Cold.GetComponent<CellSplitAnimation>().originCell1 = other;
            Deactive();
            other.Deactive();

        }
        else
        {

            Move(other.transform.position);

        }
    }

    void Start()
    {
        base.bStart();
    }
    void DamagePreSecond()
    {
        if (primaryTarget != null)
        {
            AoeDmg(transform.position, attackRange);
            primaryTarget.GetComponent<BaseCell>().currentProtein -= (attackDamage / primaryTarget.GetComponent<BaseCell>().defense);
        }
    }

    //  public void Guarding()
    //  {
    //      List<GameObject> aiUnits = GameObjectManager.FindAIUnits();
    //      for (int i = 0; i < aiUnits.Count; i++)
    //      {
    //          if (Vector3.Distance(aiUnits[i].transform.position, transform.position) <= fovRadius)
    //          {
    //              if (aiUnits[i] != this.gameObject)
    //              {
    //                  Attack(aiUnits[i]);
    //              }
    //              break;
    //          }
    //      }
    //  }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CellState.IDLE:
                //         Guarding();
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
                //  if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                //  {
                //      currentState = CellState.IDLE;
                //  }
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
        if (mergingPartner != null)
            MergingTheCells(mergingPartner);
    }



    //LateUpdate is called after all Update functions have been called

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

    void AoeDmg(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            BaseCell basecellerino = hitColliders[i].GetComponent<BaseCell>();
            if (basecellerino != null)
            {
                if (basecellerino.isAIPossessed && basecellerino != primaryTarget && basecellerino.isMine == false)
                {
                    basecellerino.currentProtein -= (attackDamage / basecellerino.defense);
                }

            }
        }
    }
}

