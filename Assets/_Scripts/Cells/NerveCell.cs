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
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
    }
    void MUltiDMg()
    {
        if (multidamagesources != null)
            multidamagesources();
    }

    public void AreaDamage()
    {
        currentProtein -= 10;
    }

    void DamagePerSecond()
    {

        // Vector3 them2me = primaryTarget.transform.position - transform.position;

        GameObject Lightningk = PhotonNetwork.connected ? PhotonNetwork.Instantiate("Lightining", transform.position, transform.rotation, 0)
            : Instantiate(Lightning, transform.position, transform.rotation) as GameObject;
        //      Lightningk.GetComponent<Lighting>().transform.LookAt(primaryTarget.transform);
        //6Lightningk.GetComponent<Rigidbody>().velocity += them2me.normalized * lightningSpeed;
        Lightningk.GetComponent<Lighting>().currentTarget = primaryTarget;
        Lightningk.GetComponent<Lighting>().realOwner = this.gameObject;
        Lightningk.GetComponent<Lighting>().speed = lightningSpeed;
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
            switch (currentState)
            {

                case CellState.IDLE:
                    if (IsInvoking("DamagePerSecond"))
                    {
                        CancelInvoke("DamagePerSecond");
                    }
                    base.bUpdate();
                    break;
                case CellState.ATTACK:
                    if (primaryTarget != null)
                    {
                        if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                        {
                            if (!IsInvoking("DamagePerSecond"))
                            {
                                InvokeRepeating("DamagePerSecond", 1.0f, 4.0f);

                            }
                        }

                        else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                        {
                            if (IsInvoking("DamagePerSecond"))
                            {
                                CancelInvoke("DamagePerSecond");
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
                case CellState.DEAD:
                    base.Die();
                    if (PhotonNetwork.connected)
                    {
                        photonView.RPC("Die", PhotonTargets.Others, null);
                    }
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