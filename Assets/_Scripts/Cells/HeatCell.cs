using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HeatCell : BaseCell
{
	float splitCD = 0;
    int terrainLayer;

    new void Awake()
    {
		base.Awake ();
        terrainLayer = 1 << LayerMask.NameToLayer ("Terrain");
    }

    // Use this for initialization
    new void Start()
    {
		base.Start ();
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
        if (Input.GetMouseButtonUp(1)) {
            RaycastHit hitInfo;
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer)) {
                Move(hitInfo.point);
            }
        }
     
		if(Input.GetKey(KeyCode.D))
		 {
			if (splitCD >= 1.0f)
			{
			    base.CancerousSplit();
			    splitCD = 0;
			}
		}
        base.Update();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
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
