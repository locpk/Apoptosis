using UnityEngine;
using System.Collections;

public class AICancerCell : MonoBehaviour {

    private float m_visionRange;
    private RaycastHit[] m_cellsInSight;
    private CancerCell m_cancerCell;
    private delegate void RandomSpliting();
    RandomSpliting dele_randomSet;

    void Awake () {
        m_visionRange = GetComponent<BaseCell>().fovRadius;
        if (m_cancerCell == null) {
            m_cancerCell = GetComponent<CancerCell>();
        }
    }

	void Start () {
        m_cancerCell.isMine = false;
        m_cancerCell.isAIPossessed = false;
        m_cancerCell.tag = "CancerCell";
        m_cancerCell.SetSpeed(m_cancerCell.navAgent.speed * .25f);
        m_cancerCell.currentState = CellState.IDLE;
        if (GetComponent<FogOfWarHider>() == null) gameObject.AddComponent<FogOfWarHider>();
        if (GetComponent<FogOfWarViewer>()) Destroy(GetComponent<FogOfWarViewer>());
        dele_randomSet += RandomMove;
        dele_randomSet += RandomSplit;
        InvokeRepeating("ExecuteDelegate", 1.0f, 5.0f);

	}
	
	void FixedUpdate () {
        m_cellsInSight = Physics.SphereCastAll(transform.position, m_visionRange, transform.forward);
        bool targetFound = false;
        foreach (RaycastHit hitInfo in m_cellsInSight) {
            if (hitInfo.collider.gameObject.tag == "Unit" || hitInfo.collider.gameObject.tag == "EnemyCell") {
                dele_randomSet -= RandomMove;
                dele_randomSet -= RandomSplit;

                targetFound = true;
                if (Vector3.Distance(transform.position, hitInfo.transform.position) > m_cancerCell.attackRange) {
                    m_cancerCell.primaryTarget = null;
                    m_cancerCell.Move(hitInfo.transform.position);

                } else {
                    if (m_cancerCell.primaryTarget == null) {
                        m_cancerCell.primaryTarget = hitInfo.transform.gameObject;
                        m_cancerCell.currentState = CellState.ATTACK;
                    }
                }
                break;
            }
        }
        if (!targetFound) {
            m_cancerCell.currentState = CellState.IDLE;
                dele_randomSet += RandomSplit;
                dele_randomSet += RandomMove;
        }
	}



    void RandomMove() {
        Vector3 _des = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        m_cancerCell.Move(_des + transform.position);
    }

    void RandomSplit() {
        int rnd = Random.Range(0, 100);
        Debug.Log("chance: " + rnd);
        if (rnd <= 10) {
            Instantiate(gameObject, new Vector3(transform.position.x, 1f, transform.position.z), transform.rotation);
            Instantiate(gameObject, new Vector3(transform.position.x, 1f, transform.position.z), transform.rotation);
            Destroy(gameObject);
        }
    }

    void ExecuteDelegate() {
        if (dele_randomSet != null) {
            dele_randomSet();
        }
    }

    void OnDrawGizmosSelected() {
        if (m_visionRange > 0.0f) {
			Gizmos.color = new Color(255, 0, 255);
			Gizmos.DrawWireSphere(transform.position, m_visionRange);
		}
    }
}
