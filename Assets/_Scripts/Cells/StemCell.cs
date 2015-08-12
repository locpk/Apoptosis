using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StemCell : BaseCell
{
    public bool isInAcidic;
    public bool isInAlkali;
    public GameObject stemtoHeat;
    public GameObject stemtoCold;
    public GameObject stemtoAlkali;
    public GameObject stemtoAcidic;
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject stun;
    int instanonce = 0;

    ParticleSystem m_particleSystem;

    public override void Mutation(CellType _newType)
    {
        if (currentProtein <= 50.0f)
        {
            return;
        }
        GameObject newCell;
        switch (_newType)
        {
            case CellType.HEAT_CELL:
                newCell = GameObject.Instantiate(stemtoHeat, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                newCell.GetComponent<CellSplitAnimation>().originCell = this;
                this.Deactive();
                break;
            case CellType.COLD_CELL:
                newCell = GameObject.Instantiate(stemtoCold, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                newCell.GetComponent<CellSplitAnimation>().originCell = this;
                this.Deactive();
                break;
            case CellType.ACIDIC_CELL:
                if (!isInAcidic)
                {
                    return;
                }
                newCell = GameObject.Instantiate(stemtoAcidic, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                newCell.GetComponent<CellSplitAnimation>().originCell = this;
                this.Deactive();
                break;
            case CellType.ALKALI_CELL:
                if (!isInAlkali)
                {
                    return;
                }
                newCell = GameObject.Instantiate(stemtoAlkali, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                newCell.GetComponent<CellSplitAnimation>().originCell = this;
                if (!sound_manager.sounds_evolution[3].isPlaying)
                {
                    sound_manager.sounds_evolution[3].Play();
                }
                this.Deactive();
                break;
            default:
                break;
        }
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
        if (primaryTarget != null)
        {
            if (primaryTarget.GetComponent<BaseCell>()) primaryTarget.GetComponent<BaseCell>().currentProtein -= attackDamage;
            if (primaryTarget.GetComponent<Animator>()) primaryTarget.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
        }
        if (PhotonNetwork.connected)
        {
            primaryTarget.GetPhotonView().RPC("ApplyDamage", PhotonTargets.Others, attackDamage);
        }

        if (!sound_manager.sounds_attacks[3].isPlaying)
        {
            sound_manager.sounds_attacks[3].Play();
        }
    }

    public override void Attack(GameObject _target)
    {
        if (_target && _target != this.gameObject)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.ATTACK;
        }
    }




    void Awake()
    {
        base.bAwake();
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
        navAgent.speed = 7.0f;
    }

    // Use this for initialization
    void Start()
    {
        base.bStart();
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    public override void Move(Vector3 _destination)
    {
        base.Move(_destination);
        navAgent.SetAreaCost(3, 2);
        navAgent.SetAreaCost(4, 1);
        navAgent.SetAreaCost(5, 2);
        navAgent.SetAreaCost(6, 1);
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
                    if (m_particleSystem.isPlaying)
                    {

                        m_particleSystem.Stop();
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
                                InvokeRepeating("DamagePerSecond", 1.0f, 1.0f);
                            }
                            if (m_particleSystem.isStopped || m_particleSystem.isPaused)
                            {
                                m_particleSystem.Play();
                            }
                            navAgent.enabled = false;
                            navObstacle.enabled = true;

                        }

                        else// if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                        {
                            if (IsInvoking("DamagePerSecond"))
                            {
                                CancelInvoke("DamagePerSecond");
                            }
                            if (m_particleSystem.isPlaying)
                            {

                                m_particleSystem.Stop();
                            }
                            if (Vector3.Distance(primaryTarget.transform.position, transform.position) > attackRange)
                            {
                                base.ChaseTarget();
                            }

                        }

                    }
                    else
                    {
                        if (IsInvoking("DamagePerSecond"))
                        {
                            CancelInvoke("DamagePerSecond");
                        }
                        if (m_particleSystem.isPlaying)
                        {

                            m_particleSystem.Stop();
                        }
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
                    base.bUpdate();

                    break;
                case CellState.DEAD:
                    base.Die();
                    if (PhotonNetwork.connected)
                    {
                        pcontroller.gameObject.GetPhotonView().RPC("Die", PhotonTargets.Others, null);
                    }
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

    //LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        base.bLateUpdate();
    }
}
