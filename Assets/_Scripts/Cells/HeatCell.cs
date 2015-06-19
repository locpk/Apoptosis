using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HeatCell : BaseCell
{
    float splitCD = 0;

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
        if (Input.GetMouseButtonUp(1))
        {
            Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (splitCD >= 1.0f)
            {
                base.CancerousSplit();
                splitCD = 0;
            }
        }
        base.bUpdate();
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
        if (_target != null)
        {


            if (Vector3.Distance(transform.position, _target.transform.position) > attackRange)
            {
                Move(_target.transform.position);
            }
            if (Vector3.Distance(transform.position, _target.transform.position) <= attackRange)
            {
                currentState = CellState.ATTACK;
                Move(transform.position);
                _target.GetComponent<BaseCell>().currentProtein = _target.GetComponent<BaseCell>().currentProtein - attackDamage;
            }
        }
        else
            currentState = CellState.IDLE;
    }
    public void AutoAttack()
    {
        GameObject closestAiguy = null;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Unit"))
        {
            if (enemy.GetComponent<BaseCell>().isAIPossessed)
            {

                if (closestAiguy == null || Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, closestAiguy.transform.position) )
                {
          
                    closestAiguy = enemy;
                    if(Vector3.Distance(transform.position, closestAiguy.transform.position) <= attackRange)
                    {
                        SetPrimaryTarget(closestAiguy);
                        break;
                    }
                }
            }
        }

     


    }




}
