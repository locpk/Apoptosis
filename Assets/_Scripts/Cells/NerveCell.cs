using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NerveCell : BaseCell
{


     void Awake()
    {

    }

    // Use this for initialization
     void Start()
    {

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
                break;
            case CellState.ATTACK_MOVING:
                break;
            case CellState.DEAD:
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
        base.Deplete(Time.fixedDeltaTime);
    }

    //LateUpdate is called after all Update functions have been called
     void LateUpdate()
    {

    }
}