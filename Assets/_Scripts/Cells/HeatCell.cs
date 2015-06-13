using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HeatCell : BaseCell
{
	float splitCD = 0;
    void Awake()
    {
		base.Awake ();
    }

    // Use this for initialization
    void Start()
    {

		base.Start ();
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
		splitCD += Time.deltaTime;

		if(Input.GetKey(KeyCode.D))
		 {
			if (splitCD >= 1.0f)
			{
			base.PerfectSplit();
			splitCD = 0;
			}
		}
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {

    }

	void Attack()
	{
		if(Vector3.Distance(transform.position, base.primaryTarget.transform.position) <= attackRange)
		{
		base.Attack (base.primaryTarget);
		}
	}
	void Consume()
	{
		base.Consume (base.primaryTarget);
	}
}
