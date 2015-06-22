using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SinglePlayerSpawner : MonoBehaviour {

    public List<Transform> enermyCellSet;
    public List<Transform> spawnAreas;
    public List<float> spawningWavesInTime;         // in sec, total 8 waves
    public List<int> spawnAmountPerWave;          // each wave has amount of cells

//    private List<BaseCell> spawnedList;           // 

    private int waveIndex = 0;
    private float timeSinceLevelStart = 0.0f;



    void Awake () {

    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLevelStart += Time.deltaTime;

        if (timeSinceLevelStart >= spawningWavesInTime[waveIndex]) {
            Debug.Log("Time: " + timeSinceLevelStart + " wave: " + waveIndex + " count: " + spawningWavesInTime [waveIndex]);
            for (int i = 0; i < spawnAmountPerWave[waveIndex]; i++) {
                SpawnEnemy(Random.Range(0, enermyCellSet.Count), Random.Range(0, spawnAreas.Count));
            }
            waveIndex++;
        }
	}

    void SpawnEnemy(int cellId, int areaId) {
        Vector3 spawnPos = Vector3.zero;
        Quaternion spwanAngle = Quaternion.identity;

        Transform area = spawnAreas[areaId];

        float _x = area.transform.position.x + Random.Range(-area.transform.localScale.x * 5, area.transform.localScale.x * 5);
        float _z = area.transform.position.z + Random.Range(-area.transform.localScale.z * 5, area.transform.localScale.z * 5);
        spawnPos = new Vector3(_x, area.transform.position.y + 0.5f, _z);
        spwanAngle.eulerAngles = new Vector3(90, 0, 0);
        BaseCell spawnedCell = Instantiate(enermyCellSet[cellId], spawnPos, spwanAngle) as BaseCell;
        if (spawnedCell) {
//            spawnedList.Add(spawnedCell);
        }

    }

}
