using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StemCell : BaseCell
{


    public override void Mutation(CellType _newType)
    {

        switch (_newType)
        {
            case CellType.HEAT_CELL:
                // GameObject.Instantiate(whateverthetypeis, transform.position, Quaternion.identity);
                break;
            case CellType.COLD_CELL:
                break;
            default:
                break;
        }
        base.Mutation(_newType);
    }

<<<<<<< HEAD
<<<<<<< HEAD
    void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
=======
    void DamagePreSecond()
    {
        primaryTarget.GetComponent<BaseCell>().currentProtein -= attackDamage;
    }

    public override void Attack(GameObject _target)
    {
        if (_target)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }

=======
    void DamagePreSecond()
    {
        primaryTarget.GetComponent<BaseCell>().currentProtein -= attackDamage;
    }

    public override void Attack(GameObject _target)
    {
        if (_target)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }

>>>>>>> origin/Junshu

    void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
        
<<<<<<< HEAD
>>>>>>> origin/Junshu
=======
>>>>>>> origin/Junshu
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case CellState.IDLE:
                //guard mode auto attack enemy in range
                if (Input.GetKeyDown(KeyCode.D))
                {
                    base.PerfectSplit();
                }
                Attack(GameObject.Find("HeatCell"));
                break;
            case CellState.ATTACK:
                if (primaryTarget)
                {
                    if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                    {
                        if (!IsInvoking("DamagePreSecond"))
                        {
                            if (GetComponent<ParticleSystem>().isStopped || GetComponent<ParticleSystem>().isPaused)
                            {
                                GetComponent<ParticleSystem>().Play();
                            }
                            InvokeRepeating("DamagePreSecond", 1.0f, 1.0f);
                        }
                        
                    }
                    else
                    {
                        base.ChaseTarget();
                        if (IsInvoking("DamagePreSecond"))
                        {
                            if (GetComponent<ParticleSystem>().isPlaying)
                            {

                                GetComponent<ParticleSystem>().Pause();
                            }
                            CancelInvoke("DamagePreSecond");
                        }
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
                //Switch to split image
                //disable navAgent
                //start splitting timer
                //initialize splitting after timer

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
<<<<<<< HEAD
<<<<<<< HEAD
        base.Update();
=======
        

>>>>>>> origin/Junshu
=======
        

>>>>>>> origin/Junshu
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        base.LateUpdate();
    }
}
