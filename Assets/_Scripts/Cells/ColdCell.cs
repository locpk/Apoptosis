using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColdCell : BaseCell
{

    public Sprite health_10;
    public Sprite health_50;
    public Sprite health_100;
    void Awake()
    {
        base.bAwake();
        
    }

    // Use this for initialization
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

   

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CellState.IDLE:
                if (IsInvoking("DamagePreSecond"))
                {
                    CancelInvoke("DamagePreSecond");
                }
                base.Guarding();
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
                    if (IsInvoking("DamagePreSecond"))
                    {
                        CancelInvoke("DamagePreSecond");
                    }
                    currentState = CellState.IDLE;
                }
                break;
            case CellState.MOVING:
                if (IsInvoking("DamagePreSecond"))
                {
                    CancelInvoke("DamagePreSecond");
                }
                base.bUpdate();
               

                break;
            case CellState.ATTACK_MOVING:
                //  if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                //  {
                //      currentState = CellState.IDLE;
                //  }
                break;
            case CellState.CONSUMING:
                if (IsInvoking("DamagePreSecond"))
                {
                    CancelInvoke("DamagePreSecond");
                }
                base.bUpdate();
                break;
            case CellState.DEAD:
                base.Die();
                break;

            default:
                break;
        }
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
        float healthRatio = currentProtein / MAX_PROTEIN;
        if (healthRatio <= 0.5f && healthRatio > 0.1f)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = health_50;
        }
        else if (healthRatio <= 0.1f)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = health_10;
        }

        //base.bFixedUpdate();
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

