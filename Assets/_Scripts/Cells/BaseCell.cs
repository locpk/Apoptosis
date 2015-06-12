using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Different Cell types
/// </summary>
public enum CellType
{
    STEM_CELL, HEAT_CELL, COLD_CELL, ACIDIC_CELL, ALKALI_CEL, CANCER_CELL,
}


/// <summary>
/// Different cells states
/// </summary>
public enum CellState
{
    IDLE,ATTACK,MOVING,ATTACK_MOVING,DEAD,CANCEROUS_SPLITTING,PERFECT_SPLITTING,EVOLVING,INCUBATING,MERGING, 
}


/// <summary>
/// Define the base cell class
/// </summary>
public class BaseCell : MonoBehaviour
{
    
    public float MAX_PROTEIN = 500.0f;
    public const float DEPLETE_TIME = 20.0f;
    public const float ATTACK_COOLDOWN = 1.0f;
    /// <summary>
    /// Fields
    /// </summary>
    public bool isMine;
    public bool isAlive = true;
    public bool isAIPossessed;
    public bool isDepleting;
    public bool isSelected;
    public CellType celltype;
    public CellState currentState;
    public NavMeshAgent navAgent;
    public NavMeshObstacle navObstacle;
    public Vector3 destination;
    public List<GameObject> targets;
    public GameObject primaryTarget;
    public PhotonView photonView;
    public float currentProtein;
    public float moveSpeed;
    public float fovRadius;
    public float attackDamage;
    public float attackRange;
    public float defense;
    public float depleteTimer = DEPLETE_TIME;
    public float depleteAmount = 3.0f; // pre second
    public float attackCooldown;
    public float splitCooldown;




    //[RPC] Methods, which called via network
    [RPC]
    public void ApplyDamage(float _received_damage)
    {
        currentProtein -= _received_damage - defense;
    }

    // Public Methods

    public void Move(Vector3 _destination)
    {
        //Move only there is a path.
        if (navAgent.CalculatePath(_destination,new NavMeshPath()))
        {
            destination = _destination;
             navAgent.SetDestination(_destination);
        }
       
    }

    public void AttackMove(Vector3 _destination)
    {
        Move(_destination);
        foreach (var item in targets)
        {
            if (Vector3.Distance(this.transform.position, item.transform.position) <= attackRange)
            {
                SetPrimaryTarget(item);
                break;
            }
        }
    }

    public void SetPrimaryTarget(GameObject _target)
    {
        if (_target)
        {
            primaryTarget = _target;
            if (_target.GetComponent<BaseCell>())
            {
                Attack(_target);
            }
            else if (_target.GetComponent<Protein>())
            {
                Consume(_target);
            }
        }
    }

    public void SetTargets(List<GameObject> _targets)
    {
        if (_targets.Count > 0)
        {
            targets = _targets;
        }
        
    }
    
    public void Attack(GameObject _target)
    {
        Move(_target.transform.position);
        if (attackCooldown <= 0.0f)
        {
            attackCooldown = ATTACK_COOLDOWN;
            //
            //Attack effects
            //
            _target.GetComponent<BaseCell>().ApplyDamage(attackDamage);
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void Consume(GameObject _target)
    {

    }

    protected void Deplete(float _deltaTime)
    {
        depleteTimer -= _deltaTime;
        if (depleteTimer <= 0.0f)
        {
            depleteTimer = DEPLETE_TIME;
            currentProtein -= depleteAmount;
        }
    }


    // Private Methods
    void Awake()
    {

        navAgent = GetComponent<NavMeshAgent>();
        photonView = GetComponent<PhotonView>();
        navObstacle = GetComponent<NavMeshObstacle>();
        navAgent.speed = moveSpeed;
        isMine = photonView.isMine;
    }

    // Use this for initialization
    void Start()
    {

    }



}
