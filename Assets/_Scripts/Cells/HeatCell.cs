﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeatCell : BaseCell
{
    public GameObject fireball;
    GameObjectManager test;
    float splitCD = 0;
    float fireballSpeed = 5;
    void Awake()
    {
        base.Awake();
        test = new GameObjectManager();
    }

    void DamagePreSecond()
    {
       GameObject test = Instantiate(fireball, transform.position, transform.rotation) as GameObject;
      
        
       Vector3 them2me = new Vector3((primaryTarget.transform.position.x - transform.position.x), (primaryTarget.transform.position.y - transform.position.y), (primaryTarget.transform.position.z - transform.position.z));
       test.GetComponent<Rigidbody>().velocity += them2me;
        primaryTarget.GetComponent<BaseCell>().currentProtein -= attackDamage;
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
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
                if (Input.GetKeyDown(KeyCode.D))
                {
                    base.CancerousSplit();
                }
                System.Collections.Generic.List<GameObject> enemyUnits = test.AiUnits();
                if (enemyUnits != null)
                {
                    for (int i = 0; i < enemyUnits.Count; i++)
                    {
                        if (Vector3.Distance(enemyUnits[i].transform.position, transform.position) <= fovRadius)
                        {
                           
                                Attack(enemyUnits[i]);
                                break;

                        }
                    }
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
                        base.ChaseTarget();
                        if (IsInvoking("DamagePreSecond"))
                        {
                            CancelInvoke("DamagePreSecond");
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
            case CellState.CONSUMING:
                if (!primaryTarget)
                {
                    if (targets.Count > 0)
                    {
                        primaryTarget = targets[0];
                        targets.RemoveAt(0);
                    }
                    else
                    {
                        currentState = CellState.IDLE;
                    }
                }
                break;
            case CellState.MOVING:
                base.Update();
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

    }

    void FixedUpdate()
    {
        base.FixedUpdate();
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
        base.LateUpdate();
    }

}
