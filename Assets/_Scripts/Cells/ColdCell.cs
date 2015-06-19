using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColdCell : BaseCell
{

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
                break;
            case CellState.ATTACK:
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
        base.bFixedUpdate();
    }

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {

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
    void Consume()
    {
        base.Consume(base.primaryTarget);
    }

    void LateUpdate()
    {
        base.bLateUpdate();
    }
}