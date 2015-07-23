using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayedSpawnEnemyController : MonoBehaviour {

    public List<Transform> enermyCellSet;
    public float delayedTimeInSecond = 1.0f;
    public int numberOfCells = 1;
    private List<Transform> m_spawnedList;

    void Awake () {
        m_spawnedList = new List<Transform>();
    }

	void Start () {
	    
	}
	
	void Update () {
	    
	}

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Unit" && enermyCellSet.Count > 0) {
            Debug.Log(this + ": Someone in my place");
            StopCoroutine("ReadyToConvert");
            StartCoroutine(ReadyToConvert(delayedTimeInSecond));

        }
    }


    // Coroutine function
    IEnumerator ReadyToConvert(float delayed) {
        
        yield return new WaitForSeconds(delayed);

        for (int i = 0; i < numberOfCells; i++) {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spwanAngle = Quaternion.identity;
            float _x = transform.position.x + Random.Range(-transform.localScale.x * 5, transform.localScale.x * 5);
            float _z = transform.position.z + Random.Range(-transform.localScale.z * 5, transform.localScale.z * 5);
            spawnPos = new Vector3(_x, transform.position.y + 0.5f, _z);
            spwanAngle.eulerAngles = new Vector3(90, 0, 0);

            Transform spawnedCell = Instantiate(enermyCellSet[Random.Range(0, enermyCellSet.Count)], spawnPos, spwanAngle) as Transform;
            spawnedCell.transform.parent = transform.parent;
            //Debug.Log(spawnedCell.gameObject.GetComponent<BaseCell>().celltype + " alive!!");

            if (spawnedCell) {

                spawnedCell.gameObject.AddComponent<AITrapCell>();

                m_spawnedList.Add(spawnedCell);
            }   
        }

        DestroyObject(gameObject);
    }

    public List<Transform> GetSpawnedList() {
        return m_spawnedList;
    }
}
