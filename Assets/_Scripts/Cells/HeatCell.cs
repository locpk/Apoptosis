using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HeatCell : BaseCell
{
	float splitCD = 0;

    new void Awake()
    {
		base.bAwake ();
    }

    // Use this for initialization
    new void Start()
    {
		base.bStart ();
    }

    // Update is called once per frame
    new void Update()
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
		splitCD += Time.deltaTime;
        //if (Input.GetMouseButtonUp(1))
        //{
        //    Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //}
     
        //if(Input.GetKey(KeyCode.D))
        // {
        //    if (splitCD >= 1.0f)
        //    {
        //    base.CancerousSplit();
        //    splitCD = 0;
        //    }
        //}
        base.bUpdate();
    }

    new void FixedUpdate()
    {
        base.bFixedUpdate();
    }

    //LateUpdate is called after all Update functions have been called
    new void LateUpdate()
    {

    }

	public override void Attack(GameObject _target)
	{
		if(Vector3.Distance(transform.position, base.primaryTarget.transform.position) <= attackRange)
		{
		base.Attack (base.primaryTarget);
		}
	}



}
