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
    STEM_CELL, HEAT_CELL, COLD_CELL, HEAT_CELL_TIRE2, COLD_CELL_TIRE2, ACIDIC_CELL, ALKALI_CELL, CANCER_CELL, NERVE_CELL
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
public class BaseCell : Photon.PunBehaviour
{
    public GameObject gCancerCellPrefab;
    public GameObject gStemCellPrefab;
    public GameObject gHeatCellPrefab;
    public GameObject gHeatCancerPrefab;
    public GameObject gColdCellPrefab;
    public GameObject gColdCancerPrefab;
    public GameObject gAcidicCellPrefab;
    public GameObject gAlkaliCellPrefab;
    public GameObject gRevertHeatPrefab;
    public GameObject gRevertColdPrefab;
    public GameObject gRevertNervePrefab;

    public Sprite health_10;
    public Sprite health_50;
    public Sprite health_100;

    public const float MAX_PROTEIN = 500.0f;
    public const float DEPLETE_TIME = 5.0f;
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
    public float currentProtein;
    public float fovRadius;
    public float attackDamage;
    public float attackRange;
    //public float defense;
    public float depleteTimer;
    public float depleteAmount = 3.0f; // per second
    public float attackCooldown;
    public float splitCooldown;

    public int NumEnemiesLeft = 0;
    public bool hitBylightning = false;

    public int hitCounter = 0;
    public bool stunned = false;
    public float stunTimer = 3;

    public PlayerController pcontroller;

    protected Sound_Manager sound_manager;


    #region RPC Methods


    ////[PunRPC] Methods, which called via network
    //[PunRPC]
    //public void ApplyDamage(float _received_damage)
    //{
    //    currentProtein -= _received_damage - defense;
    //}

    [PunRPC]
    public void ApplyDamage(float damage)
    {
        currentProtein -= damage;
        GetComponent<Animator>().SetTrigger("BeingAttackTrigger");
    }

    [PunRPC]
    public void StunMe()
    {
        stunned = true;
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(currentProtein);
        }
        else
        {
            // Network player, receive data
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.currentProtein = (float)stream.ReceiveNext();
        }
    }


    #region Standard Actions
    // Public Methods

    [PunRPC]
    public virtual void Move(Vector3 _destination)
    {

        currentState = CellState.MOVING;
        navObstacle.enabled = false;
        navAgent.enabled = true;
        destination = _destination;
        navAgent.SetDestination(_destination);

    }

    public void SetSpeed(float _speed)
    {
        navAgent.speed = _speed;
    }

    public virtual void AttackMove(Vector3 _destination)
    {
        currentState = CellState.ATTACK_MOVING;
        navObstacle.enabled = false;
        navAgent.enabled = true;
        destination = _destination;
        navAgent.SetDestination(_destination);
    }

    public void SetPrimaryTarget(GameObject _target)
    {
        if (IsInvoking("ConsumePerSecond"))
        {
            CancelInvoke("ConsumePerSecond");
        }

        primaryTarget = _target;

    }

    public void SetTargets(List<GameObject> _targets)
    {
        if (_targets.Count > 0)
        {
            targets = new List<GameObject>(_targets);
        }

    }

    [PunRPC]
    public void Die()
    {

        if (isMine)
        {
            PlayerController.cap--;
            if (PlayerController.cap < 0)
            {
                PlayerController.cap = 0;
            }
        }

        isMine = false;
        isAlive = false;
        pcontroller.RemoveDeadCell(this);

        if (!sound_manager.sounds_miscellaneous[3].isPlaying)
        {
            sound_manager.sounds_miscellaneous[3].Play();
        }
        if (!isSinglePlayer)
        {
            if (photonView.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void Deactive()
    {
        GameObject.Find("PlayerControl").GetComponent<PlayerController>().DeselectCell(this);
        gameObject.SetActive(false);
        transform.position = new Vector3(2500.0f, 2500.0f, 2500.0f);



    }

    public virtual void Attack(GameObject _target)
    {
    }

    void ConsumePerSecond()
    {
        if (primaryTarget && primaryTarget.GetComponent<Protein>())
        {
            primaryTarget.GetComponent<PhotonView>().RPC("Harvest", PhotonTargets.Others, null);
            currentProtein += primaryTarget.GetComponent<Protein>().Harvest();
            if (currentProtein > MAX_PROTEIN)
            {
                currentProtein = MAX_PROTEIN;
            }
        }

    }

    public void Consume(GameObject _target)
    {
        Protein targetProtein = _target.GetComponent<Protein>();
        if (targetProtein)
        {
            SetPrimaryTarget(_target);
            currentState = CellState.CONSUMING;
        }
    }

    public void Guarding()
    {
        //List<GameObject> aiUnits = GameObjectManager.FindAIUnits();
        //if (aiUnits.Count > 0)
        //{
        //    foreach (var enemy in aiUnits)
        //    {
        //        if (Vector3.Distance(enemy.transform.position, transform.position) <= fovRadius)
        //        {
        //            if (enemy != this)
        //            {
        //                Attack(enemy);
        //            }
        //            break;
        //        }
        //    }
        //}

    }

    public void AIAttacking()
    {
        List<GameObject> playerUnits = GameObjectManager.FindPlayerUnits();
        if (playerUnits.Count > 0)
        {
            foreach (var enemy in playerUnits)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) <= fovRadius)
                {
                    if (enemy != this)
                    {
                        Attack(enemy);
                    }
                    break;
                }
            }
        }
    }

    #endregion

    #region Special abilities
    public void PerfectSplit()
    {
        if (currentProtein <= 50.0f || PlayerController.cap + 1 > PlayerController.MAX_CAP)
        {
            return;
        }


        GameObject cellSplitAnimation;
        switch (celltype)
        {
            case CellType.STEM_CELL:
                cellSplitAnimation = GameObject.Instantiate(gStemCellPrefab, transform.position, Quaternion.identity) as GameObject;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentLevel = currentLevel + 1;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentProtein = currentProtein * 0.5f;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().originCell = this;
                Deactive();
                break;
            case CellType.CANCER_CELL:
                cellSplitAnimation = GameObject.Instantiate(gCancerCellPrefab, transform.position, Quaternion.identity) as GameObject;
                cellSplitAnimation.GetComponent<BaseCell>().currentLevel = currentLevel + 1;
                cellSplitAnimation.GetComponent<BaseCell>().currentProtein = currentProtein * 0.5f;
                cellSplitAnimation.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().originCell = this;
                Deactive();
                break;
            default:
                break;
        }




    }

    public void Revert()
    {
        GameObject cellSplitAnimation;
        switch (celltype)
        {
            case CellType.HEAT_CELL_TIRE2:
                cellSplitAnimation = GameObject.Instantiate(gRevertHeatPrefab, transform.position, Quaternion.identity) as GameObject;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().originCell = this;
                Deactive();
                break;
            case CellType.COLD_CELL_TIRE2:
                cellSplitAnimation = GameObject.Instantiate(gRevertColdPrefab, transform.position, Quaternion.identity) as GameObject;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().originCell = this;
                Deactive();
                break;
            case CellType.NERVE_CELL:
                cellSplitAnimation = GameObject.Instantiate(gRevertNervePrefab, transform.position, Quaternion.identity) as GameObject;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                cellSplitAnimation.GetComponent<CellSplitAnimation>().originCell = this;
                Deactive();
                break;
            default:
                break;
        }

    }

    public void CancerousSplit()
    {
        if (currentProtein <= 1.0f || PlayerController.cap + 1 > PlayerController.MAX_CAP)
        {
            return;
        }

        float cancerousChance = 0.0f;
        switch (currentLevel)
        {
            case 0:
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
                cancerousChance = CancerChance.LEVEL_5;
                break;
        }


        //half my protein
        this.currentProtein *= 0.5f;
        currentLevel++;

        GameObject newCell;


        if (Random.Range(0.0f, 1.0f) > cancerousChance)
        {

            switch (this.celltype)
            {
                case CellType.HEAT_CELL:
                    newCell = GameObject.Instantiate(gHeatCellPrefab, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                    newCell.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
                    newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                    newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                    newCell.GetComponent<CellSplitAnimation>().originCell = this;
                    Deactive();
                    break;
                case CellType.COLD_CELL:
                    newCell = GameObject.Instantiate(gColdCellPrefab, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                    newCell.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
                    newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                    newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                    newCell.GetComponent<CellSplitAnimation>().originCell = this;
                    Deactive();
                    break;
                default:
                    break;
            }
        }
        else
        {


            switch (this.celltype)
            {
                case CellType.HEAT_CELL:
                    newCell = GameObject.Instantiate(gHeatCancerPrefab, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                    newCell.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
                    newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                    newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                    newCell.GetComponent<CellSplitAnimation>().originCell = this;
                    Deactive();

                    break;
                case CellType.COLD_CELL:
                    newCell = GameObject.Instantiate(gColdCancerPrefab, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                    newCell.GetComponent<CellSplitAnimation>().currentLevel = currentLevel;
                    newCell.GetComponent<CellSplitAnimation>().currentProtein = currentProtein;
                    newCell.GetComponent<CellSplitAnimation>().isAIPossessed = isAIPossessed;
                    newCell.GetComponent<CellSplitAnimation>().originCell = this;
                    Deactive();
                    break;
                default:
                    break;
            }
        }





    }


    public virtual void Mutation(CellType _newType)
    {

    }

    public virtual void Evolve(CellType _newType)
    {

    }


    public void ChaseTarget()
    {
        if (primaryTarget)
        {
            Move(primaryTarget.transform.position);
        }
    }
    public void Deplete(float _deltaTime)
    {
        if (isDepleting)
        {
            depleteTimer -= _deltaTime;
            if (depleteTimer <= 0.0f)
            {
                depleteTimer = DEPLETE_TIME;
                currentProtein -= depleteAmount;
            }
        }

    }
    #endregion

    protected void bAwake()
    {
        sound_manager = GameObject.FindGameObjectWithTag("Sound_Manager").GetComponent<Sound_Manager>();
        navAgent = GetComponent<NavMeshAgent>();
        navObstacle = GetComponent<NavMeshObstacle>();
        navAgent.speed = moveSpeed;
        pcontroller = GameObject.Find("PlayerControl").GetComponent<PlayerController>();

        if (PhotonNetwork.connected)
        {
            isSinglePlayer = (bool)photonView.instantiationData[0];
        }
        else
        {
            isSinglePlayer = true;
        }


        depleteTimer = DEPLETE_TIME;
        if (isSinglePlayer)
        {
            // GetComponent<PhotonView>().enabled = false;

        }
        else
        {
            isMine = photonView.isMine;
        }

        if (isMine)
        {
            gameObject.AddComponent<FogOfWarViewer>();
        }
        else if (!isMine && isAIPossessed)
        {
            gameObject.AddComponent<FogOfWarHider>();

        }
        else if (!isMine)
        {
            gameObject.AddComponent<FogOfWarHider>();
        }


        pcontroller.AddNewCell(this);
        
    }



    // Use this for initialization
    protected void bStart()
    {

        navAgent.enabled = false;
        navAgent.updateRotation = false;
        navObstacle.enabled = true;
        isAlive = true;
        if (!isMine && transform.tag != "Animation" && this.celltype != CellType.CANCER_CELL)
        {

            this.gameObject.transform.FindChild("MinimapIndicator").GetComponent<MeshRenderer>().material.color = Color.red;
            this.gameObject.transform.FindChild("AlertPing").GetComponent<SpriteRenderer>().enabled = false;
        }

        Move(transform.position);
        
    }

    protected void bUpdate()
    {

        if (currentState == CellState.IDLE)
        {

            if (IsInvoking("ConsumePerSecond"))
            {
                CancelInvoke("ConsumePerSecond");
            }
        }
        if (currentState == CellState.MOVING)
        {
            if (isStopped())
            {

                navAgent.enabled = false;
                navObstacle.enabled = true;
                if (primaryTarget != null)
                {
                    if (primaryTarget.tag == "Protein")
                    {

                        currentState = CellState.CONSUMING;
                        return;
                    }
                    else if (primaryTarget.tag == "Unit")
                    {

                        currentState = CellState.ATTACK;
                        return;
                    }
                }
                else
                {
                    currentState = CellState.IDLE;
                    return;
                }
            }
        }
        else if (currentState == CellState.CONSUMING)
        {
            if (primaryTarget)
            {
                if (!primaryTarget.GetComponent<Protein>().consumers.Contains(this))
                {
                    primaryTarget.GetComponent<Protein>().consumers.Add(this);
                }
                float distance = Vector3.Distance(primaryTarget.transform.position, transform.position);

                if (distance > attackRange && distance <= fovRadius)
                {
                    if (IsInvoking("ConsumePerSecond"))
                    {

                        CancelInvoke("ConsumePerSecond");
                    }
                    ChaseTarget();
                }
                else if (distance <= attackRange)
                {
                    if (!IsInvoking("ConsumePerSecond"))
                    {

                        InvokeRepeating("ConsumePerSecond", 1.0f, 1.0f);
                    }

                }
                else
                {

                    ChaseTarget();
                }
            }
            else
            {

                currentState = CellState.IDLE;
                return;
            }
        }
        else if (currentState == CellState.ATTACK_MOVING)
        {

            List<GameObject> theirUnits = GameObjectManager.FindTheirUnits();
            if (theirUnits.Count > 0)
            {
                foreach (var enemy in theirUnits)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) <= attackRange)
                    {
                        if (enemy != this.gameObject)
                        {
                            Attack(enemy);
                            return;
                        }

                    }
                }
            }
            if (isStopped())
            {

                navAgent.enabled = false;
                navObstacle.enabled = true;
                currentState = CellState.IDLE;
                return;
            }
        }
    }

    public bool isStopped()
    {
        if (navAgent.isActiveAndEnabled)
        {
            if (!navAgent.pathPending)
            {
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0.0f)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    protected void bFixedUpdate()
    {
        Deplete(Time.fixedDeltaTime);
        if (currentProtein <= 0.0f)
        {
            currentState = CellState.DEAD;
        }
        if (isMine && currentState != CellState.ATTACK && currentState != CellState.IDLE && currentState != CellState.CONSUMING)
        {
            transform.FindChild("AlertPing").GetComponent<SpriteRenderer>().enabled = false;
        }
        if (!isMine && celltype != CellType.CANCER_CELL)
        {
            this.gameObject.transform.FindChild("AlertPing").GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    protected void bLateUpdate()
    {
        if (transform.FindChild("Nucleus"))
        {
            float healthRatio = currentProtein / MAX_PROTEIN;
            if (healthRatio <= 0.5f && healthRatio > 0.1f)
            {
                transform.FindChild("Nucleus").GetComponent<SpriteRenderer>().sprite = health_50;
            }
            else if (healthRatio <= 0.1f)
            {
                transform.FindChild("Nucleus").GetComponent<SpriteRenderer>().sprite = health_10;
            }
            else
            {
                transform.FindChild("Nucleus").GetComponent<SpriteRenderer>().sprite = health_100;
            }
        }


        if (currentProtein <= 0.0f)
        {
            Die();
            if (PhotonNetwork.connected)
            {
                photonView.RPC("Die", PhotonTargets.Others, null);
            }
            transform.FindChild("AlertPing").GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
