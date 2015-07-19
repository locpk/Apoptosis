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

    private Sound_Manager sound_manager;
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
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
                newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                newCell.GetComponent<CellSplitAnimation>().originCell = this;
                this.Deactive();
                break;
            case CellType.COLD_CELL:
                newCell = GameObject.Instantiate(stemtoCold, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
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
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
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
                newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
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
        multidamagesources();
    }
    public void AreaDamage()
    {
        currentProtein -= 10;
    }
    void nothing()
    {

    }
    

    void DamagePerSecond()
    {
        if (primaryTarget != null)
        {
            primaryTarget.GetComponent<BaseCell>().currentProtein -= attackDamage;
            primaryTarget.GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
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
        multidamagesources += nothing;
        InvokeRepeating("MUltiDMg", 1.0f, 1.0f);
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
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
       
    


            switch (currentState)
            {

                case CellState.IDLE:
                    SetPrimaryTarget(null);
                    if (IsInvoking("DamagePerSecond"))
                    {
                        if (GetComponent<ParticleSystem>().isPlaying)
                        {

                            GetComponent<ParticleSystem>().Stop();
                        }
                        CancelInvoke("DamagePerSecond");
                    }

                    //guard mode auto attack enemy in range
                    //base.Guarding();
                    break;
                case CellState.ATTACK:

                    float distance = Vector3.Distance(primaryTarget.transform.position, transform.position);

                    if (distance > attackRange && distance <= fovRadius)
                    {
                        if (IsInvoking("DamagePerSecond"))
                        {
                            if (GetComponent<ParticleSystem>().isPlaying)
                            {

                                GetComponent<ParticleSystem>().Stop();
                            }
                            CancelInvoke("DamagePerSecond");
                        }
                        base.ChaseTarget();
                    }
                    else if (distance <= attackRange)
                    {
                        if (!IsInvoking("DamagePerSecond"))
                        {
                            InvokeRepeating("DamagePerSecond", 1.0f, 1.0f);
                        }
                        if (GetComponent<ParticleSystem>().isStopped || GetComponent<ParticleSystem>().isPaused)
                        {
                            GetComponent<ParticleSystem>().Play();
                        }

                    }
                    else
                    {
                        if (IsInvoking("DamagePerSecond"))
                        {
                            if (GetComponent<ParticleSystem>().isPlaying)
                            {

                                GetComponent<ParticleSystem>().Stop();
                            }
                            CancelInvoke("DamagePerSecond");
                        }
                        currentState = CellState.IDLE;
                    }
                    break;
                case CellState.CONSUMING:
                    base.bUpdate();

                    break;
                case CellState.MOVING:

                    base.bUpdate();
                    if (primaryTarget && base.isStopped())
                    {
                        if (primaryTarget.GetComponent<BaseCell>())
                        {
                            currentState = CellState.ATTACK;
                            return;
                        }
                        else if (primaryTarget.GetComponent<Protein>())
                        {
                            currentState = CellState.CONSUMING;
                            return;
                        }
                    }
                    else if (!primaryTarget || base.isStopped())
                    {
                        currentState = CellState.IDLE;
                        return;
                    }


                    break;
                case CellState.ATTACK_MOVING:
                    base.bUpdate();

                    break;
                case CellState.DEAD:
                    base.Die();
                    break;
                default:
                    break;
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
