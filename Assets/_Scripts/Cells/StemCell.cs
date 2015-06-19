using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StemCell : BaseCell
{
    public bool isInAcidic;
    public bool isInAlkali;
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

    void DamagePerSecond()
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

    public void Guarding()
    {
        List<GameObject> aiUnits = GameObjectManager.FindAIUnits();
        foreach (var enemy in aiUnits)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) <= fovRadius)
            {
                Attack(enemy);
                break;
            }
        }
    }


    void Awake()
    {
        base.bAwake();
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
                if (IsInvoking("DamagePerSecond"))
                {
                    if (GetComponent<ParticleSystem>().isPlaying)
                    {

                        GetComponent<ParticleSystem>().Stop();
                    }
                    CancelInvoke("DamagePerSecond");
                }
                SetPrimaryTarget(null);
                navAgent.Stop();
                //guard mode auto attack enemy in range
                Guarding();
                break;
            case CellState.ATTACK:

                float distance = Vector3.Distance(primaryTarget.transform.position, transform.position);

                if (distance > attackRange && distance <= fovRadius)
                {
                    if (IsInvoking("DamagePerSecond"))
                    {
                        if (GetComponent<ParticleSystem>().isPlaying)
                        {

                            GetComponent<ParticleSystem>().Stop();
                        }
                        CancelInvoke("DamagePerSecond");
                    }
                    base.ChaseTarget();
                }
                else if (distance <= attackRange)
                {
                    if (!IsInvoking("DamagePerSecond"))
                    {
                        if (GetComponent<ParticleSystem>().isStopped || GetComponent<ParticleSystem>().isPaused)
                        {
                            GetComponent<ParticleSystem>().Play();
                        }
                        InvokeRepeating("DamagePerSecond", 1.0f, 1.0f);
                    }

                }
                else
                {

                    currentState = CellState.IDLE;
                }
                break;
            case CellState.CONSUMING:
                if (IsInvoking("DamagePerSecond"))
                {
                    if (GetComponent<ParticleSystem>().isPlaying)
                    {

                        GetComponent<ParticleSystem>().Stop();
                    }
                    CancelInvoke("DamagePerSecond");
                }
                //if (!primaryTarget)
                //{
                //    if (targets.Count > 0)
                //    {
                //        primaryTarget = targets[0];
                //        targets.RemoveAt(0);
                //    }
                //    else
                //    {
                //        currentState = CellState.IDLE;
                //    }
                //}
                break;
            case CellState.MOVING:
                if (IsInvoking("DamagePerSecond"))
                {
                    if (GetComponent<ParticleSystem>().isPlaying)
                    {

                        GetComponent<ParticleSystem>().Stop();
                    }
                    CancelInvoke("DamagePerSecond");
                }
                float dis = Vector3.Distance(primaryTarget.transform.position, transform.position);
                if (dis > fovRadius)
                {
                    SetPrimaryTarget(null);
                    navAgent.Stop();
                    currentState = CellState.IDLE;
                }
                else
                {
                    base.bUpdate();
                }
                break;
            case CellState.ATTACK_MOVING:
                if (IsInvoking("DamagePerSecond"))
                {
                    if (GetComponent<ParticleSystem>().isPlaying)
                    {

                        GetComponent<ParticleSystem>().Stop();
                    }
                    CancelInvoke("DamagePerSecond");
                }
                //if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                //{
                //    currentState = CellState.IDLE;
                //}
                break;
            case CellState.DEAD:
                base.Die();
                break;
            default:
                break;
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
