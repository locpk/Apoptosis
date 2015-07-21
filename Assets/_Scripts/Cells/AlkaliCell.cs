using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AlkaliCell : BaseCell
{
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject DOT;
    GameObject previousTarget;
    public GameObject stun;
    int instanonce = 0;
   

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
        if (primaryTarget != null)
        {
            previousTarget = primaryTarget;
            Vector3 newvec =  new Vector3(primaryTarget.transform.position.x, primaryTarget.transform.position.y, (primaryTarget.transform.position.z + primaryTarget.GetComponent<SphereCollider>().radius/4));
            GameObject theDOT= Instantiate(DOT,  newvec ,primaryTarget.transform.rotation) as GameObject;
    
            theDOT.GetComponent<Dot>().Target = primaryTarget;
            theDOT.GetComponent<Dot>().Owner = this.gameObject;


            if (!sound_manager.sounds_attacks[4].isPlaying)
            {
                sound_manager.sounds_attacks[4].Play();
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        base.bStart();
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
            if (this.stunTimer <= 0)
            {
                instanonce = 0;
                // Destroy(stun.gameObject);
                this.stunTimer = 3;
                this.stunned = false;
                this.hitCounter = 0;

            }
        }
        else
        {
            if (targets != null && targets.Count > 1)
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

                    Guarding();
                    break;
                case CellState.ATTACK:
                    if (primaryTarget != null)
                    {
                        if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                        {
                            if (!IsInvoking("DamagePreSecond"))
                            {
                                InvokeRepeating("DamagePreSecond", 1.0f, 3.0f);

                            }
                        }
                        else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                        {
                            if (IsInvoking("DamagePreSecond"))
                            {
                                CancelInvoke("DamagePreSecond");
                            }
                            base.ChaseTarget();
                        }

                    }
                    else
                    {
                        currentState = CellState.IDLE;
                    }
                    break;

                case CellState.MOVING:
                    base.bUpdate();
                    if (primaryTarget && base.isStopped())
                    {
                        if (primaryTarget.GetComponent<BaseCell>())
                        {
                            currentState = CellState.ATTACK;
                        }
                        else if (primaryTarget.GetComponent<Protein>())
                        {
                            currentState = CellState.CONSUMING;
                        }
                    }
                    else if (!primaryTarget || base.isStopped())
                    {
                        currentState = CellState.IDLE;
                    }


                    break;
                case CellState.ATTACK_MOVING:
                    // if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                    // {
                    //     currentState = CellState.IDLE;
                    // }
                    base.bUpdate();
                    break;
                case CellState.DEAD:
                    base.Die();
                    break;
                case CellState.CONSUMING:
                    base.bUpdate();
                    break;
                default:
                    break;

            }
        }
    }



    public override void Attack(GameObject _target)
    {
        if (_target)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }

    

    void FixedUpdate()
    {
        base.bFixedUpdate();
    }

    void LateUpdate()
    {
        base.bLateUpdate();
    }
}
