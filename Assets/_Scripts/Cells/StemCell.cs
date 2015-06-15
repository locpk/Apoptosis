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

    new void Awake()
    {

    }

    // Use this for initialization
    new void Start()
    {

    }

    // Update is called once per frame
    new void Update()
    {
        switch (currentState)
        {
            case CellState.IDLE:
                //guard mode auto attack enemy in range
                break;
            case CellState.ATTACK:
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
                if (!navAgent.isActiveAndEnabled)
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

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //LateUpdate is called after all Update functions have been called
    new void LateUpdate()
    {

    }
}
