using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SinglePlayerSpawner_Level_1 : MonoBehaviour {

    public Transform wavesCollection;
    public List<Transform> enermyCellSet;
    public List<Transform> spawnAreas;
    public List<float> spawningWavesInTime;         // in sec, total 8 waves
    public List<int> spawnAmountPerWave;          // each wave has amount of cells


    private List<BaseCell> m_spawnedList;       

    private int m_waveIndex = 0;
    private float m_timeSinceLevelStart = 0.0f;



    void Awake () {
        m_spawnedList = new List<BaseCell>();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (spawningWavesInTime.Count >= m_waveIndex) {
            m_timeSinceLevelStart += Time.deltaTime;
            if (m_timeSinceLevelStart >= spawningWavesInTime[m_waveIndex]) {
         //       Debug.Log("Time: " + m_timeSinceLevelStart + " wave: " + m_waveIndex + " count: " + spawningWavesInTime [m_waveIndex]);
                for (int i = 0; i < spawnAmountPerWave[m_waveIndex]; i++) {
                    SpawnEnemy(Random.Range(0, enermyCellSet.Count), Random.Range(0, spawnAreas.Count));
                }
                m_waveIndex++;
            }
        }

        if (m_waveIndex == spawningWavesInTime.Count) {
            Destroy(gameObject);
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
        //BaseCell baseReadySpawnCell = enermyCellSet[cellId].gameObject.GetComponent<BaseCell>();

        Transform spawnedCell = Instantiate(enermyCellSet[cellId], spawnPos, spwanAngle) as Transform;
        spawnedCell.parent = wavesCollection.transform;

        
        
        if (spawnedCell) {
            spawnedCell.gameObject.AddComponent<AIWaveCell>();
            spawnedCell.GetComponent<AIWaveCell>().spawnPointOwner = area;
            switch (spawnedCell.gameObject.GetComponent<BaseCell>().celltype) {
                case CellType.STEM_CELL:
                    //m_spawnedList.Add(spawnedCell.gameObject.GetComponent<StemCell>());
                    break;
                case CellType.HEAT_CELL:
                    m_spawnedList.Add(spawnedCell.gameObject.GetComponent<HeatCell>());
                    break;
                case CellType.COLD_CELL:
                    m_spawnedList.Add(spawnedCell.gameObject.GetComponent<ColdCell>());
                    break;
                case CellType.HEAT_CELL_TIRE2:
                    m_spawnedList.Add(spawnedCell.gameObject.GetComponent<Tier2HeatCell>());
                    break;
                case CellType.COLD_CELL_TIRE2:
                    m_spawnedList.Add(spawnedCell.gameObject.GetComponent<Tier2ColdCell>());
                    break;
                case CellType.ACIDIC_CELL:
                    m_spawnedList.Add(spawnedCell.gameObject.GetComponent<AcidicCell>());
                    break;
                case CellType.ALKALI_CELL:
                    m_spawnedList.Add(spawnedCell.gameObject.GetComponent<AlkaliCell>());
                    break;
                case CellType.CANCER_CELL:
                    //m_spawnedList.Add(spawnedCell.gameObject.GetComponent<CancerCell>());
                    break;
                case CellType.NERVE_CELL:
                    m_spawnedList.Add(spawnedCell.gameObject.GetComponent<NerveCell>());
                    break;
                default:
                    break;
            }
        }   
    }

    public List<BaseCell> GetSpawnedList() {
        return m_spawnedList;
    }

}
