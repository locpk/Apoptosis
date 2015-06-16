using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Cancer Chance
/// </summary>
public static class CancerChance
{
    public const float LEVEL_1 = 0.0f;
    public const float LEVEL_2 = 0.25f;
    public const float LEVEL_3 = 0.35f;
    public const float LEVEL_4 = 0.45f;
    public const float LEVEL_5 = 1.0f;
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
    IDLE, ATTACK, MOVING, CONSUMING, ATTACK_MOVING, DEAD, CANCEROUS_SPLITTING, PERFECT_SPLITTING, EVOLVING, INCUBATING, MERGING,
}


/// <summary>
/// Define the base cell class
/// </summary>
public class BaseCell : MonoBehaviour
{

    public GameObject gCancerCellPrefab;
    public GameObject gStemCellPrefab;
    public GameObject gHeatCellPrefab;
    public GameObject gColdCellPrefab;
    public GameObject gAcidicCellPrefab;
    public GameObject gAlkaliCellPrefab;

    public float MAX_PROTEIN = 500.0f;
    public const float DEPLETE_TIME = 1.0f;
    public const float ATTACK_COOLDOWN = 1.0f;
    public const float moveSpeed = 3.0f;
    /// <summary>
    /// Fields
    /// </summary>
    //
    public int currentLevel = 0;
    public bool isSinglePlayer = true;
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
    public float fovRadius;
    public float attackDamage;
    public float attackRange;
    public float defense;
    public float depleteTimer;
    public float depleteAmount = 3.0f; // per second
    public float attackCooldown;
    public float splitCooldown;

    public int neighbors = 0;


    #region RPC Methods


    //[RPC] Methods, which called via network
    [RPC]
    public void ApplyDamage(float _received_damage)
    {
        currentProtein -= _received_damage - defense;
    }
    #endregion


    #region Standard Actions
    // Public Methods

    public virtual void Move(Vector3 _destination, int count)
    {
        //Move only there is a path.
        currentState = CellState.MOVING;
        navObstacle.enabled = false;
        navAgent.enabled = true;
        destination = _destination;
        navAgent.SetDestination(_destination);

        neighbors = count - 1;

    }

    public void SetSpeed(float _speed)
    {
        navAgent.speed = _speed;
    }

    public virtual void AttackMove(Vector3 _destination)
    {
        Move(_destination, 0);
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
    public void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Attack(GameObject _target)
    {
    }

    public virtual void Consume(GameObject _target)
    {

    }
    #endregion

    #region Special abilities
    public BaseCell PerfectSplit()
    {
        //if (currentLevel >= 5)
        //{
        //    return null;
        //}

        Vector3 newposition = this.transform.position;

        newposition += Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(GetComponent<SphereCollider>().radius * 0.1f, 0, 0);
        GameObject newCell;
        switch (celltype)
        {
            case CellType.STEM_CELL:
                newCell = GameObject.Instantiate(gStemCellPrefab, newposition, Quaternion.identity) as GameObject;
                newCell.gameObject.transform.Rotate(90, -180, -180);
                this.currentProtein *= 0.5f;
                this.currentLevel++;
                newCell.GetComponent<BaseCell>().currentProtein = this.currentProtein;
                newCell.GetComponent<BaseCell>().currentLevel++;
                newCell.GetComponent<BaseCell>().navAgent.updateRotation = false;
                return newCell.GetComponent<BaseCell>();
            case CellType.CANCER_CELL:
                newCell = GameObject.Instantiate(gCancerCellPrefab, newposition, Quaternion.identity) as GameObject;
                newCell.gameObject.transform.Rotate(90, -180, -180);
                this.currentProtein *= 0.5f;
                this.currentLevel++;
                newCell.GetComponent<BaseCell>().currentProtein = this.currentProtein;
                newCell.GetComponent<BaseCell>().currentLevel++;
                newCell.GetComponent<BaseCell>().navAgent.updateRotation = false;
                return newCell.GetComponent<BaseCell>();
            default:
                break;
        }


        return null;


    }

    public BaseCell CancerousSplit()
    {
        //if (currentLevel >= 5)
        //{
        //    return null;
        //}


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
        newposition += Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(GetComponent<SphereCollider>().radius * 0.1f, 0.0f, 0.0f);

        //half my protein
        this.currentProtein *= 0.5f;


        GameObject newCell;


        if (Random.Range(0.0f, 1.0f) > cancerousChance)
        {

            switch (this.celltype)
            {
                case CellType.HEAT_CELL:
                    newCell = GameObject.Instantiate(gHeatCellPrefab, newposition, Quaternion.identity) as GameObject;
                    newCell.gameObject.transform.Rotate(90, -180, -180);
                    newCell.GetComponent<BaseCell>().currentProtein = this.currentProtein;
                    newCell.GetComponent<BaseCell>().currentLevel++;
                    newCell.GetComponent<BaseCell>().navAgent.updateRotation = false;
                    return newCell.GetComponent<BaseCell>();
                case CellType.COLD_CELL:
                    newCell = GameObject.Instantiate(gColdCellPrefab, newposition, Quaternion.identity) as GameObject;
                    newCell.gameObject.transform.Rotate(90, -180, -180);
                    newCell.GetComponent<BaseCell>().currentProtein = this.currentProtein;
                    newCell.GetComponent<BaseCell>().currentLevel++;
                    newCell.GetComponent<BaseCell>().navAgent.updateRotation = false;
                    return newCell.GetComponent<BaseCell>();
                default:
                    break;
            }
        }
        else
        {
            newCell = GameObject.Instantiate(gCancerCellPrefab, newposition, Quaternion.identity) as GameObject;
            newCell.gameObject.transform.Rotate(90, -180, -180);
            newCell.GetComponent<BaseCell>().currentProtein = this.currentProtein;
            newCell.GetComponent<BaseCell>().currentLevel++;
            newCell.GetComponent<BaseCell>().navAgent.updateRotation = false;
            return newCell.GetComponent<BaseCell>();
        }

        return null;




    }


    public virtual void Mutation(CellType _newType)
    {
        isAlive = false;
        currentState = CellState.DEAD;
    }

    public virtual void Evolve(CellType _newType)
    {
        isAlive = false;
        currentState = CellState.DEAD;
    }



    public void Deplete(float _deltaTime)
    {
        depleteTimer -= _deltaTime;
        if (depleteTimer <= 0.0f)
        {
            depleteTimer = DEPLETE_TIME;
            currentProtein += depleteAmount;
        }
    }
    #endregion

    protected void Awake()
    {
        depleteTimer = DEPLETE_TIME;
        if (isSinglePlayer)
        {
            GetComponent<PhotonView>().enabled = false;
        }
        navAgent = GetComponent<NavMeshAgent>();
        navObstacle = GetComponent<NavMeshObstacle>();
        navAgent.speed = moveSpeed;
        photonView = GetComponent<PhotonView>();
        //  isMine = photonView.isMine;
    }

    // Use this for initialization
    protected void Start()
    {
        navAgent.enabled = false;
        navAgent.updateRotation = false;
        navObstacle.enabled = true;
    }

    protected void Update()
    {
        if (currentState == CellState.MOVING)
        {
			float radius = 0;
			if (neighbors > 19)
				radius = 7;
			else if (neighbors > 5) {
				radius = 5;
			}
			else if (neighbors > 0)
				radius = 3;

			if (Vector2.Distance(navAgent.destination, transform.position) <= 0) {
				currentState = CellState.IDLE;
				navAgent.SetDestination(transform.position);
			}
            if (Vector3.Distance(navAgent.destination, transform.position) <= radius * .05)
            {
				navAgent.enabled = false;
				navObstacle.enabled = true;
            }

        }
    }

    protected void FixedUpdate()
    {
        Deplete(Time.fixedDeltaTime);
        if (currentProtein <= 0.0f)
        {
            currentState = CellState.DEAD;
        }


        //if  (Mathf.Abs(Vector3.Distance(destination, transform.position)) <= 1.1f)
        //{

        //    navAgent.enabled = false;

        //    navObstacle.enabled = true;
        //    //currentState = CellState.IDLE;

        //}






        // transform.FindChild("Nucleus").transform.localScale = new Vector3(0.8f * currentProtein / MAX_PROTEIN, 0.8f * currentProtein / MAX_PROTEIN, 0.8f * currentProtein / MAX_PROTEIN);
    }

    protected void LateUpdate()
    {

    }
}
