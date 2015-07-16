using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AlkaliCell : BaseCell
{
    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject DOT;
    GameObject previousTarget;

    private Sound_Manager sound_manager;

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
        if (!sound_manager.sounds_miscellaneous[6].isPlaying)
        {
            sound_manager.sounds_miscellaneous[6].Play();

        }
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

            // play the sound
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
