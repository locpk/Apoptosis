using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AlkaliCell : BaseCell
{

    void Awake()
    {
        base.bAwake();
    }
    void DamagePerSecond()
    {
        if (primaryTarget != null)
        {
            primaryTarget.GetComponent<BaseCell>().currentProtein -= attackDamage;
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
                    else
                    {
                        SetPrimaryTarget(null);
                        navAgent.Stop();
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
                if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                {
                    currentState = CellState.IDLE;
                }
                break;
            case CellState.DEAD:
                base.Die();
                break;
            case CellState.CONSUMING:
                base.bUpdate();
                break;
            default:
                break;

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

    public void Guarding()
    {
        List<GameObject> aiUnits = GameObjectManager.FindAIUnits();
        for (int i = 0; i < aiUnits.Count; i++)
        {
            if (Vector3.Distance(aiUnits[i].transform.position, transform.position) <= fovRadius)
            {
                if (aiUnits[i] != this.gameObject)
                {
                    Attack(aiUnits[i]);
                }
                break;
            }
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
