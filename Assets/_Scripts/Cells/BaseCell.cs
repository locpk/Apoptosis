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
    
    public const float MAX_PROTEIN = 500.0f;
    public const float DEPLETE_TIME = 20.0f;
    /// <summary>
    /// Fields
    /// </summary>
    
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

    }

    public void AttackMove(Vector3 _destination)
    {

    }

    public void SetPrimaryTarget(GameObject _target)
    {

    }

    public void SetTargets(List<GameObject> _targets)
    {

    }
    
    public void Attack(GameObject _target)
    {

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

    }

    // Use this for initialization
    void Start()
    {

    }


    void FixedUpdate()
    {
        
    }
}
