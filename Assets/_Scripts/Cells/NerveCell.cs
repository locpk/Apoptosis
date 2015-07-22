using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NerveCell : BaseCell
{

    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject Lightning;
    float lightningSpeed = 1.0f;
    public GameObject stun;
    int instanonce = 0;

  
    void Start()
    {
        base.bStart();
    }

    void Awake()
    {
        base.bAwake();
        multidamagesources += nothing;
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
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

       // Vector3 them2me = primaryTarget.transform.position - transform.position;
 
        GameObject Lightningk = Instantiate(Lightning, transform.position, transform.rotation) as GameObject;
        //      Lightningk.GetComponent<Lighting>().transform.LookAt(primaryTarget.transform);
        //6Lightningk.GetComponent<Rigidbody>().velocity += them2me.normalized * lightningSpeed;
       Lightningk.GetComponent<Lighting>().currentTarget = primaryTarget;
        Lightningk.GetComponent<Lighting>().realOwner = this.gameObject;
        Lightningk.GetComponent<Lighting>().speed = lightningSpeed;
        Debug.Log(Lightningk.transform.rotation);
        Lightningk.transform.rotation = this.transform.rotation;
        if (!sound_manager.sounds_attacks[5].isPlaying)
        {
            sound_manager.sounds_attacks[5].Play();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (stunned == true)
        {
            if (instanonce < 1)
            {
                Vector3 trackingPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                GameObject.Instantiate(stun, trackingPos, transform.rotation);
            }
            instanonce++;

            stunTimer -= 1 * Time.fixedDeltaTime;
            if (stunTimer > 0)
            {
                navAgent.enabled = false;
                navObstacle.enabled = true;
                primaryTarget = null;

                return;
            }
            if (this.stunTimer <= 0)
            {
                instanonce = 0;
                // Destroy(stun.gameObject);
                this.stunTimer = 3;
                this.stunned = false;
                this.hitCounter = 0;
                return;
            }
        }
        else
        {
            if (targets != null && targets.Count >= 1)
            {

                if (primaryTarget == null)
                {
                    for (int i = 0; i < targets.Count; i++)
                    {

                        if (i != targets.Count)
                        {
                            Debug.Log(primaryTarget);
                            primaryTarget = targets[i + 1];
                            Debug.Log(primaryTarget);
                            if (primaryTarget.GetComponent<BaseCell>())
                                currentState = CellState.ATTACK;
                            if (primaryTarget.GetComponent<Protein>())
                                currentState = CellState.CONSUMING;
                            break;
                        }
                    }
                }
            }
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