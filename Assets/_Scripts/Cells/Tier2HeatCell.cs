using UnityEngine;
using System.Collections;

public class Tier2HeatCell : BaseCell
{

    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject fireball;
    // Use this for initialization
    float fireballSpeed = 15;
    GameObject previousTarget;
    bool hasteActive = false;
    public float hasteTimer = 0.0f;
    public GameObject stemCell;
    void Start()
    {
        base.bStart();
    }
    void DamagePreSecond()
    {
        if (primaryTarget != null)
        {
            previousTarget = primaryTarget;
            Vector3 them2me = primaryTarget.transform.position - transform.position;
            GameObject thefireball = Instantiate(fireball, transform.position, transform.rotation) as GameObject;
            thefireball.GetComponent<Rigidbody>().velocity += them2me.normalized * fireballSpeed;
            thefireball.GetComponent<FireBall>().Target = primaryTarget;
            thefireball.GetComponent<FireBall>().Owner = this.gameObject;

        }
    }
    void HasteDamagePreSecond()
    {
        if (primaryTarget != null)
        {
            previousTarget = primaryTarget;
            Vector3 them2me = primaryTarget.transform.position - transform.position;
            GameObject thefireball = Instantiate(fireball, transform.position, transform.rotation) as GameObject;
            thefireball.GetComponent<Rigidbody>().velocity += them2me.normalized * fireballSpeed;
            thefireball.GetComponent<FireBall>().Target = primaryTarget;
            thefireball.GetComponent<FireBall>().Owner = this.gameObject;

        }
    }





    // Update is called once per frame
    void Update()
    {
        
        hasteTimer += 1 * Time.deltaTime;
        if (hasteTimer >= 5)
        {
            hasteActive = true;
            if (hasteTimer >= 10)
            {
                hasteTimer = 0.0f;
                hasteActive = false;
            }
        }
        if (hasteActive)
        {
            navAgent.speed = 5.0f;
        }
        else
        {
            navAgent.speed = moveSpeed;
        }

        switch (currentState)
        {
            case CellState.IDLE:
                SetPrimaryTarget(null);
                if (hasteActive)
                {
                    if (IsInvoking("HasteDamagePreSecond"))
                    {
                        CancelInvoke("HasteDamagePreSecond");
                    }
                }
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
                        if (hasteActive)
                        {
                            if (!IsInvoking("HasteDamagePreSecond"))
                            {
                                InvokeRepeating("HasteDamagePreSecond", 1.0f, 0.6f);

                            }
                        }
                        else
                        {
                            if (!IsInvoking("DamagePreSecond"))
                            {
                                InvokeRepeating("DamagePreSecond", 1.0f, 1.0f);

                            }
                        }
                    }

                    else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                    {
                        if (hasteActive)
                        {
                            if (IsInvoking("HasteDamagePreSecond"))
                            {
                                CancelInvoke("HasteDamagePreSecond");
                            }
                        }
                        else
                        {
                            if (IsInvoking("DamagePreSecond"))
                            {
                                CancelInvoke("DamagePreSecond");
                            }
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
