using UnityEngine;
using System.Collections;

public class Tier2HeatCell : BaseCell {

    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
	// Use this for initialization

    void Start()
    {
        base.bStart();
    }
	
	// Update is called once per frame
	void Update () 
    {

        switch (currentState)
        {
            case CellState.IDLE:
                SetPrimaryTarget(null);
 
                break;
            case CellState.ATTACK:
                if (primaryTarget != null)
                {
                    if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                    {
                
                    }

                    else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                    {
                      
                        if (Vector3.Distance(primaryTarget.transform.position, transform.position) > attackRange)
                        {
                            base.ChaseTarget();
                        }
                    }

                }
                else
                {
                    currentState = CellState.IDLE;
                }
                break;
            case CellState.CONSUMING:
                base.bUpdate();
                break;
            case CellState.MOVING:

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

            default:
                break;
        }

	}
    void Awake()
    {
        base.bAwake();
        multidamagesources += nothing;
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);

    }
    void MUltiDMg()
    {
        multidamagesources();
  
    }
    public void AreaDamage()
    {
        currentProtein -= 10;
    }
    void nothing()
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

    void LateUpdate()
    {
        base.bLateUpdate();
    }

    void FixedUpdate()
    {
        base.bFixedUpdate();
    }

}
