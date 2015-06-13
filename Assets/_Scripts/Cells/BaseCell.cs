using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Cancer Chance
/// </summary>
 public static class CancerChance
    {
        public const float LEVEL_1 = 0.0f;
        public const float LEVEL_2 = 0.1f;
        public const float LEVEL_3 = 0.2f;
        public const float LEVEL_4 = 0.3f;
        public const float LEVEL_5 = 0.4f;
    }

/// <summary>
/// Different Cell types
/// </summary>
public enum CellType
{
    STEM_CELL, HEAT_CELL, COLD_CELL, HEAT_CELL_TIRE2, COLD_CELL_TIRE2, ACIDIC_CELL, ALKALI_CELL, CANCER_CELL,
}


/// <summary>
/// Different cells states
/// </summary>
public enum CellState
{
    IDLE, ATTACK, MOVING, ATTACK_MOVING, DEAD, CANCEROUS_SPLITTING, PERFECT_SPLITTING, EVOLVING, INCUBATING, MERGING,
}


/// <summary>
/// Define the base cell class
/// </summary>
public class BaseCell : MonoBehaviour
{

    public  GameObject cancerCellPrefab;

    public float MAX_PROTEIN = 500.0f;
    public const float DEPLETE_TIME = 20.0f;
    public const float ATTACK_COOLDOWN = 1.0f;
    /// <summary>
    /// Fields
    /// </summary>
    //
    public int currentLevel = 0;
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
        if (navAgent.CalculatePath(_destination, new NavMeshPath()))
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

    public void PerfectSplit()
    {
        if (currentLevel >= 5)
        {
            return;
        }

        Vector3 newposition = this.transform.position;
        newposition += Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(GetComponent<SphereCollider>().radius, 0, 0);
        GameObject newCell = GameObject.Instantiate(gameObject, newposition, Quaternion.identity) as GameObject;
        this.currentProtein *= 0.5f;
        switch (this.celltype)
        {
            case CellType.STEM_CELL:
                newCell.GetComponent<StemCell>().currentProtein = this.currentProtein;
                newCell.GetComponent<StemCell>().currentLevel++;
                break;
            case CellType.CANCER_CELL:
                newCell.GetComponent<CancerCell>().currentProtein = this.currentProtein;
                newCell.GetComponent<CancerCell>().currentLevel++;
                break;
            default:
                break;
        }

    }

    public void CancerousSplit()
    {
        if (currentLevel >= 5)
        {
            return;
        }


        float cancerousChance = 0.0f;
        switch (currentLevel)
        {
            case 1:
                cancerousChance = CancerChance.LEVEL_1;
                break;
            case 2:
                cancerousChance = CancerChance.LEVEL_2;
                break;
            case 3:
                cancerousChance = CancerChance.LEVEL_3;
                break;
            case 4:
                cancerousChance = CancerChance.LEVEL_4;
                break;
            case 5:
                cancerousChance = CancerChance.LEVEL_5;
                break;
            default:
                break;
        }

        //Get a new position around myself
        Vector3 newposition = this.transform.position;
        newposition += Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(GetComponent<SphereCollider>().radius, 0, 0);

        //half my protein
        this.currentProtein *= 0.5f;


        GameObject newCell;


        if (Random.Range(0.0f,1.0f) <= cancerousChance)
        {
            newCell = GameObject.Instantiate(cancerCellPrefab, newposition, Quaternion.identity) as GameObject;
            switch (this.celltype)
            {
                case CellType.HEAT_CELL:
                    newCell.GetComponent<HeatCell>().currentProtein = this.currentProtein;
                    newCell.GetComponent<HeatCell>().currentLevel++;
                    break;
                case CellType.COLD_CELL:
                    newCell.GetComponent<ColdCell>().currentProtein = this.currentProtein;
                    newCell.GetComponent<ColdCell>().currentLevel++;
                    break;
                default:
                    break;
            }
        }
        else
        {
            newCell = GameObject.Instantiate(gameObject, newposition, Quaternion.identity) as GameObject;
            newCell.GetComponent<CancerCell>().currentProtein = this.currentProtein;
        }
       
        
        
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
