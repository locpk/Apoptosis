using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tier2ColdCell : BaseCell
{
    public GameObject stemCell;
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject particles;

    PlayerController controller;

    public GameObject stun;
    int instanonce = 0;



    // Use this for initialization
    void Start()
    {
        controller = GameObject.Find("PlayerControl").GetComponent<PlayerController>();
        base.bStart();


    }
    void DamagePreSecond()
    {
        if (primaryTarget != null)
        {
            AoeDmg(transform.position, attackRange);
            //  primaryTarget.GetComponent<BaseCell>().currentProtein -= attackDamage;
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


            stunTimer -= 1.0f * Time.fixedDeltaTime;
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
                this.stunTimer = 2;
                this.stunned = false;
                this.hitCounter = 0;
                navObstacle.enabled = false;
                navAgent.enabled = true;
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

                            if (i == 0 && targets.Count == 1)
                                primaryTarget = targets[i];
                            else
                                primaryTarget = targets[i + 1];

                            if (primaryTarget != null)
                            {
                                if (primaryTarget.GetComponent<BaseCell>())
                                    currentState = CellState.ATTACK;
                                if (primaryTarget.GetComponent<Protein>())
                                    currentState = CellState.CONSUMING;
                            }
                            break;
                        }
                    }
                }
            }
            switch (currentState)
            {
                case CellState.IDLE:

                    if (Input.GetKeyUp(KeyCode.Alpha1) && isSelected == true)
                    {
                        Vector3 trackingPos = this.transform.position;
                        Quaternion trackingRot = this.transform.rotation;
                        Die();
                        if (PhotonNetwork.connected)
                        {
                            photonView.RPC("Die", PhotonTargets.Others, null);
                        }
                        GameObject gstem = Instantiate(stemCell, trackingPos, trackingRot) as GameObject;
                    }
                    //         Guarding();
                    break;
                case CellState.ATTACK:
                    if (primaryTarget != null)
                    {
                        if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                        {
                            if (!IsInvoking("DamagePreSecond"))
                            {
                                InvokeRepeating("DamagePreSecond", 1.0f, 1.5f);

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
                    //  if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                    //  {
                    //      currentState = CellState.IDLE;
                    //  }
                    break;
                case CellState.CONSUMING:
                    base.bUpdate();
                    break;
                case CellState.DEAD:
                    base.Die();
                    if (PhotonNetwork.connected)
                    {
                        photonView.RPC("Die", PhotonTargets.Others, null);
                    }
                    break;

                default:
                    break;

            }
        }
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
    void AoeDmg(Vector3 center, float radius)
    {
        if (isAIPossessed == false)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                BaseCell basecellerino = hitColliders[i].GetComponent<BaseCell>();
                if (basecellerino != null)
                {
                    if (basecellerino.isMine == false)
                    {
                        basecellerino.currentProtein -= attackDamage;
                        basecellerino.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                        ++basecellerino.hitCounter;
                        Vector3 tracking = new Vector3(basecellerino.transform.position.x, basecellerino.transform.position.y + 2, basecellerino.transform.position.z);
                        // Vector3
                        Instantiate(particles, tracking, basecellerino.transform.rotation);
                        if (PhotonNetwork.connected)
                        {
                            basecellerino.gameObject.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, attackDamage);
                        }
                        if (basecellerino.hitCounter >= 4)
                        {
                            basecellerino.stunned = true;
                            if (PhotonNetwork.connected)
                            {
                                basecellerino.gameObject.GetPhotonView().RPC("StunMe", PhotonTargets.Others, null);
                            }
                        }
                    }

                }
            }
        }
        else
        {

            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                BaseCell basecellerino = hitColliders[i].GetComponent<BaseCell>();
                if (basecellerino != null)
                {
                    if (basecellerino.isMine == true)
                    {
                        basecellerino.currentProtein -= attackDamage;
                        basecellerino.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
                        ++basecellerino.hitCounter;
                        Vector3 tracking = new Vector3(basecellerino.transform.position.x, basecellerino.transform.position.y + 2, basecellerino.transform.position.z);

                        // Vector3
                        Instantiate(particles, tracking, basecellerino.transform.rotation);
                        if (PhotonNetwork.connected)
                        {
                            basecellerino.gameObject.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, attackDamage);
                        }
                        if (basecellerino.hitCounter >= 4)
                        {
                            basecellerino.stunned = true;
                            if (PhotonNetwork.connected)
                            {
                                basecellerino.gameObject.GetPhotonView().RPC("StunMe", PhotonTargets.Others, null);
                            }
                        }

                    }

                }
            }
        }
    }

}
