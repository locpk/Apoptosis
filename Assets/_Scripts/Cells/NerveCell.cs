using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NerveCell : BaseCell
{

    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject Lightning;
    float lightningSpeed = 19.0f;
    void Start()
    {
        base.bStart();
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

    void DamagePreSecond()
    {

        Vector3 them2me = primaryTarget.transform.position - transform.position;
        GameObject Lightningk = Instantiate(Lightning, transform.position, transform.rotation) as GameObject;
        //      Lightningk.GetComponent<Lighting>().transform.LookAt(primaryTarget.transform);
        Lightningk.GetComponent<Rigidbody>().velocity += them2me.normalized * lightningSpeed;
        Lightningk.GetComponent<Lighting>().Target = primaryTarget;
        Lightningk.GetComponent<Lighting>().realOwner = this.gameObject;
        Lightningk.GetComponent<Lighting>().speed = lightningSpeed;


    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CellState.IDLE:
                SetPrimaryTarget(null);
                if (IsInvoking("DamagePreSecond"))
                {
                    CancelInvoke("DamagePreSecond");
                }
                break;
            case CellState.ATTACK:
                if (primaryTarget != null)
                {
                    if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                    {
                        if (!IsInvoking("DamagePreSecond"))
                        {
                            InvokeRepeating("DamagePreSecond", 1.0f, 4.0f);

                        }
                    }

                    else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                    {
                        if (IsInvoking("DamagePreSecond"))
                        {
                            CancelInvoke("DamagePreSecond");
                        }
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
                if (IsInvoking("DamagePreSecond"))
                {
                    CancelInvoke("DamagePreSecond");
                }
                base.bUpdate();
                break;
            case CellState.DEAD:
                base.Die();
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
    public override void Attack(GameObject _target)
    {
        if (_target)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }


    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        base.bLateUpdate();
    }

}