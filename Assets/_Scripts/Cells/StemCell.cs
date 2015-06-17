using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StemCell : BaseCell
{

    public GameObject stemtoHeat;
    public GameObject stemtoCold;
    public GameObject stemtoAlkali;
    public GameObject stemtoAcidic;
    public override void Mutation(CellType _newType)
    {
        if (currentProtein <= 50.0f)
        {
            return;
        }
        GameObject newCell;
        switch (_newType)
        {
            case CellType.HEAT_CELL:
                newCell = GameObject.Instantiate(stemtoHeat, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                currentState = CellState.DEAD;
                break;
            case CellType.COLD_CELL:
                newCell = GameObject.Instantiate(stemtoCold, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                currentState = CellState.DEAD;
                break;
            case CellType.ACIDIC_CELL:
                newCell = GameObject.Instantiate(stemtoAcidic, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                currentState = CellState.DEAD;
                break;
            case CellType.ALKALI_CELL:
                newCell = GameObject.Instantiate(stemtoAlkali, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                currentState = CellState.DEAD;
                break;
            default:
                break;
        }
    }

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


    void Awake()
    {
        base.Awake();
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
                //guard mode auto attack enemy in range
                //if (Vector3.Distance(GameObject.Find("HeatCell").transform.position, transform.position) <= fovRadius)
                //{
                //    Attack(GameObject.Find("HeatCell"));
                //}

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
                    else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                    {
                        base.ChaseTarget();
                        if (IsInvoking("DamagePreSecond"))
                        {
                            if (GetComponent<ParticleSystem>().isPlaying)
                            {

                                GetComponent<ParticleSystem>().Stop();
                            }
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
                GetComponent<Animator>().Play("StemMovement");
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
