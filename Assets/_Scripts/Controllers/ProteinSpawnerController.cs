using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProteinSpawnerController : MonoBehaviour {

    public enum SpawnMode {
        Once,
        Regenerate
    }

    public SpawnMode spawnMode = SpawnMode.Once;
    public Protein protein;
    public float delayTimeInSecond = 5.0f;
    public float spawnCycleInSecond;
    public int maxProteins;
    private int currentProteins;
    private float proteinRadius;
    private List<Vector3> spawnPositionList;

    private int testcases = 0;

    void Awake () {
        if (protein == null) {
            Debug.LogError(protein.ToString() + " has not been set.");
            Destroy(this);
        }
        spawnPositionList = new List<Vector3>();
    }

    void Start() {
        proteinRadius = protein.GetComponent<NavMeshObstacle>().radius;
        StartCoroutine(SpawnProteinOnce(delayTimeInSecond));
    }

    void FixedUpdate() {

        if (spawnMode == SpawnMode.Regenerate) {

        }
    }

    IEnumerator SpawnProteinOnce(float delay) {
        yield return new WaitForSeconds(delay);
        Protein currProtein = null;
        for (int i = 0; i < maxProteins; i++) {
            Vector3 spawnPos = Vector3.zero;
            int count = 0;
            do {
                float _x = transform.position.x + Random.Range(-transform.localScale.x * 5, transform.localScale.x * 5);
                float _z = transform.position.z + Random.Range(-transform.localScale.z * 5, transform.localScale.z * 5);
                spawnPos = new Vector3(_x, transform.position.y + 0.5f, _z);

                if (spawnPositionList.Count <= 0) {
                    spawnPositionList.Add(spawnPos);
                } 
                foreach (var tmp in spawnPositionList) {
                    if (Vector3.Distance(tmp, spawnPos) > proteinRadius)
                        count++;
                }
                if (count == spawnPositionList.Count) {
                    spawnPositionList.Add(spawnPos);
                    break;
                }
                count = 0;
                testcases++;
                if (testcases >= 10000) {
                    Debug.LogError("MAX TEST CASE REACHED!!!");
                    break;
                }
            } while (true);
        }
        Quaternion spwanAngle = Quaternion.identity;;
        spwanAngle.eulerAngles = new Vector3(90, 0, 0);
        for (int i = 0; i < spawnPositionList.Count - 1; i++) {
            
            currProtein = Instantiate(protein, spawnPositionList[i], spwanAngle) as Protein;
            currProtein.transform.parent = transform.parent;
        }
        Debug.Log(testcases + " cases.");
    }

}
