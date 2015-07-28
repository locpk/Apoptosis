using UnityEngine;
using System.Collections;

public class Tier2HeatCell : BaseCell
{

    public delegate void TakeDamage();
    public TakeDamage multidamagesources;
    public GameObject fireball;
    // Use this for initialization
    float fireballSpeed = 15;
    //GameObject previousTarget;
    bool hasteActive = false;
    public float hasteTimer = 0.0f;
    public GameObject stemCell;
    public GameObject stun;
    int instanonce = 0;


    void Start()
    {
        base.bStart();
    }
    void DamagePerSecond()
    {
        if (primaryTarget != null)
        {
            //previousTarget = primaryTarget;
            Vector3 them2me = primaryTarget.transform.position - transform.position;
            GameObject thefireball = PhotonNetwork.connected ? PhotonNetwork.Instantiate("Fireball", transform.position, transform.rotation, 0) as GameObject
                : Instantiate(fireball, transform.position, transform.rotation) as GameObject;
            thefireball.GetComponent<Rigidbody>().velocity += them2me.normalized * fireballSpeed;
            thefireball.GetComponent<FireBall>().Target = primaryTarget;
            thefireball.GetComponent<FireBall>().Owner = this.gameObject;

        }
    }
    void HasteDamagePerSecond()
    {
        if (primaryTarget != null)
        {
            //previousTarget = primaryTarget;
            Vector3 them2me = primaryTarget.transform.position - transform.position;
            GameObject thefireball = PhotonNetwork.connected ? PhotonNetwork.Instantiate("Fireball", transform.position, transform.rotation, 0) as GameObject
                : Instantiate(fireball, transform.position, transform.rotation) as GameObject;
            thefireball.GetComponent<Rigidbody>().velocity += them2me.normalized * fireballSpeed;
            thefireball.GetComponent<FireBall>().Target = primaryTarget;
            thefireball.GetComponent<FireBall>().Owner = this.gameObject;

        }
    }

    public override void Move(Vector3 _destination)
{
 	 base.Move(_destination);
     navAgent.SetAreaCost(3, 1);
     navAgent.SetAreaCost(4, 5);
     navAgent.SetAreaCost(5, 5);
     navAgent.SetAreaCost(6, 5);
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
                navAgent.speed = 15.0f;
            }
            else
            {
                navAgent.speed = 9.0f;
            }

            switch (currentState)
            {
                case CellState.IDLE:

                    if (hasteActive)
                    {
                        if (IsInvoking("HasteDamagePerSecond"))
                        {
                            CancelInvoke("HasteDamagePerSecond");
                        }
                    }
                    if (IsInvoking("DamagePerSecond"))
                    {
                        CancelInvoke("DamagePerSecond");
                    }
                    base.bUpdate();
                    break;
                case CellState.ATTACK:

                    if (primaryTarget == null)
                    {
                        if (IsInvoking("DamagePerSecond"))
                        {
                            CancelInvoke("DamagePerSecond");
                        }
                        currentState = CellState.IDLE;
                        return;
                    }

                    if (primaryTarget != null)
                    {
                        if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= attackRange)
                        {
                            if (hasteActive)
                            {
                                if (!IsInvoking("HasteDamagePerSecond"))
                                {
                                    InvokeRepeating("HasteDamagePerSecond", 1.0f, 0.6f);

                                }
                            }
                            else
                            {
                                if (!IsInvoking("DamagePerSecond"))
                                {
                                    InvokeRepeating("DamagePerSecond", 1.0f, 1.0f);

                                }
                            }
                        }

                        else if (Vector3.Distance(primaryTarget.transform.position, transform.position) <= fovRadius)
                        {
                            if (hasteActive)
                            {
                                if (IsInvoking("HasteDamagePerSecond"))
                                {
                                    CancelInvoke("HasteDamagePerSecond");
                                }
                            }
                            else
                            {
                                if (IsInvoking("DamagePerSecond"))
                                {
                                    CancelInvoke("DamagePerSecond");
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
                        return;
                    }
                    break;
                case CellState.CONSUMING:
                    base.bUpdate();
                    break;
                case CellState.MOVING:
                    if (IsInvoking("DamagePerSecond"))
                    {
                        CancelInvoke("DamagePerSecond");
                    }
                    base.bUpdate();
                    break;
                case CellState.ATTACK_MOVING:
                    if (!navAgent.isActiveAndEnabled && !primaryTarget && targets.Count == 0)
                    {
                        currentState = CellState.IDLE;
                        return;
                    }
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
        navAgent.speed = 9.0f;

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

}
