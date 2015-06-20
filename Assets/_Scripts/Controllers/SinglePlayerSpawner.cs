using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SinglePlayerSpawner : MonoBehaviour {

    public List<Transform> enermyCellSet;
    public List<Transform> spawnAreas;
    public List<int> spawningWavesInTime;         // in sec, total 8 waves
    public List<int> spawnAmountPerWave;          // each wave has amount of cells

    private List<BaseCell> spawnedList;           // 

    private int waveIndex = 0;
    private float timeSinceLevelStart = 0.0f;



    void Awake () {

    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
