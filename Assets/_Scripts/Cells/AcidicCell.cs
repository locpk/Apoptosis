using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcidicCell : BaseCell
{

    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject stun;
    public GameObject Acid;
    int instanonce = 0;
    Sound_Manager sound_manager;

    void Awake()
    {
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
        base.bAwake();
        multidamagesources += nothing;
          InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
    }

    void DamagePreSecond()
    {
        if (primaryTarget != null)
        {
            GameObject kAcid = Instantiate(Acid, transform.position, transform.rotation) as GameObject;
            kAcid.GetComponent<Acidd>().Target = primaryTarget;
            kAcid.GetComponent<Acidd>().Owner = this.gameObject;
            Vector3 them2me = kAcid.GetComponent<Acidd>().Target.transform.position - transform.position;
            kAcid.GetComponent<Rigidbody>().velocity += them2me.normalized * kAcid.GetComponent<Acidd>().speed;

            if (!sound_manager.sounds_attacks[2].isPlaying)
            {
                sound_manager.sounds_attacks[2].Play();

            }
        }
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
                            if (Vector3.Distance(primaryTarget.transform.position, transform.position) > attackRange)
                            {
                                base.ChaseTarget();
                            }
                        }
                        else
                        {
                            SetPrimaryTarget(null);
                            navAgent.Stop();
                        }

                    }
                    else
                    {
                        currentState = CellState.IDLE;
                    }
                    break;
                case CellState.MOVING:
                    base.bUpdate();
                    if (primaryTarget != null)
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
                    break;
                case CellState.ATTACK_MOVING:
                    break;
                case CellState.CONSUMING:
                    base.bUpdate();
                    break;
                case CellState.DEAD:
                    base.Die();
                    break;


                default:
                    break;
            }
            base.bUpdate();
        }
    }

    void FixedUpdate()
    {
        base.bFixedUpdate();
    }

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        base.bLateUpdate();
    }
}
