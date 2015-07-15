using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIWaveCell : MonoBehaviour {

    private float m_visionRange;
    private RaycastHit[] m_cellsInSight;
    private BaseCell m_baseCell;

    public Transform spawnPointOwner;
    private List<Transform> m_waypointList;
    private Queue<Transform> m_waypointQueue;
    private float m_movingSpeed;
    private float m_attackSpeed;

    void Awake() {
        switch (GetComponent<BaseCell>().celltype) {
            case CellType.HEAT_CELL:
                m_baseCell = GetComponent<HeatCell>();
                break;
            case CellType.COLD_CELL:
                m_baseCell = GetComponent<ColdCell>();
                break;
            case CellType.HEAT_CELL_TIRE2:
                m_baseCell = GetComponent<Tier2HeatCell>();
                break;
            case CellType.COLD_CELL_TIRE2:
                //m_baseCell = GetComponent<Tier2HeatCell>();
                break;
            case CellType.ACIDIC_CELL:
                m_baseCell = GetComponent<AcidicCell>();
                break;
            case CellType.ALKALI_CELL:
                m_baseCell = GetComponent<AlkaliCell>();
                break;
            case CellType.CANCER_CELL:
                m_baseCell = GetComponent<CancerCell>();
                break;
            case CellType.NERVE_CELL:
                m_baseCell = GetComponent<NerveCell>();
                break;
            default:
                break;
        }

        m_waypointList = new List<Transform>();
        m_waypointQueue = new Queue<Transform>();

        m_visionRange = GetComponent<BaseCell>().fovRadius;
    }

	void Start () {
        m_baseCell.isMine = false;
        m_baseCell.isAIPossessed = false;
        m_baseCell.tag = "EnemyCell";
        //m_baseCell.SetSpeed(m_baseCell.navAgent.speed * .5f);
        m_movingSpeed = m_baseCell.navAgent.speed;
        m_attackSpeed = m_movingSpeed * .5f;
        m_baseCell.currentState = CellState.IDLE;
        if (GetComponent<FogOfWarHider>() == null) {
            gameObject.AddComponent<FogOfWarHider>();
        }
	}
	
	void FixedUpdate () {
        if (m_waypointList.Count <= 0) {
            spawnPointOwner.GetComponentsInChildren<Transform>(m_waypointList);
            foreach (var waypoint in m_waypointList) {
                m_waypointQueue.Enqueue(waypoint);
            }
            m_waypointQueue.Dequeue();  // drop the first transform;
            
        }

        m_cellsInSight = Physics.SphereCastAll(transform.position, m_visionRange, transform.forward);
        bool targetFound = false;
        foreach (RaycastHit hitInfo in m_cellsInSight) {
            if (hitInfo.collider.gameObject.tag == "Unit") {
                if (m_baseCell.navAgent.speed >= m_attackSpeed) m_baseCell.SetSpeed(m_attackSpeed);


                targetFound = true;
                if (Vector3.Distance(transform.position, hitInfo.transform.position) > m_baseCell.attackRange) {
                    m_baseCell.primaryTarget = null;
                    m_baseCell.Move(hitInfo.transform.position);

                } else {
                    if (m_baseCell.primaryTarget == null) {
                        m_baseCell.primaryTarget = hitInfo.transform.gameObject;
                        m_baseCell.currentState = CellState.ATTACK;
                    }
                }
                break;
            } else {
                if (m_baseCell.navAgent.speed <= m_movingSpeed) m_baseCell.SetSpeed(m_movingSpeed);

            }
        }
        if (!targetFound) {

        }

	}

    void OnDrawGizmosSelected() {
        if (m_visionRange > 0.0f) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, m_visionRange);
		}
    }
}
