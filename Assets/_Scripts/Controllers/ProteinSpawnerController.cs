using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProteinSpawnerController : MonoBehaviour {

    public enum SpawnMode {
        Once,
        Regenerate
    }

    public SpawnMode spawnMode = SpawnMode.Once;
    public GameObject protein;
    public float delayTimeInSecond = 5.0f;
    public float spawnCycleInSecond;
    public int maxProteins;
    private float proteinRadius;
    private List<Vector3> spawnPositionList;

    private int testcases = 0;
    private bool preSpawned = false;
    void Awake () {
        if (protein == null) {
            Debug.LogError(this.ToString() + " has not been set.");
            Destroy(this);
        }
        spawnPositionList = new List<Vector3>();
    }

    void Start() {
        proteinRadius = protein.GetComponent<NavMeshObstacle>().radius;
        StartCoroutine(SpawnProteinOnce(delayTimeInSecond));
    }

    void FixedUpdate() {

        if (spawnMode == SpawnMode.Regenerate && maxProteins > transform.parent.childCount - 1 && preSpawned) {
            if (!IsInvoking("RepeatingSpawnProtein")) {
                InvokeRepeating("RepeatingSpawnProtein", 1.0f, spawnCycleInSecond);
            }
        } else {
            if (IsInvoking("RepeatingSpawnProtein")) {
                CancelInvoke("RepeatingSpawnProtein");
            }
        }
    }

    void RepeatingSpawnProtein() {
        Vector3 spawnPos = Vector3.zero;
        GameObject currProtein = null;
        Quaternion spwanAngle = Quaternion.identity;
        spwanAngle.eulerAngles = new Vector3(90, 0, 0);
        int count = 0;
        int currentProteinListCount = transform.parent.childCount;
        testcases = 0;

        List<Vector3> currProteinList = new List<Vector3>();
        do {
            float _x = transform.position.x + Random.Range(-transform.localScale.x * 5, transform.localScale.x * 5);
            float _z = transform.position.z + Random.Range(-transform.localScale.z * 5, transform.localScale.z * 5);
            spawnPos = new Vector3(_x, transform.position.y + 0.5f, _z);
            foreach (Transform child in transform.parent) {
                if ((Vector3.Distance(child.position, spawnPos) > proteinRadius * 2.5f) && (child.tag == "Protein"))
                    count++;
            }

            if (count == currentProteinListCount - 1) {
                currProtein = PhotonNetwork.connected ? PhotonNetwork.InstantiateSceneObject("Protein", spawnPos, spwanAngle, 0, null) : Instantiate(protein, spawnPos, spwanAngle) as GameObject;
                currProtein.transform.parent = transform.parent;
                GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewProtein(currProtein.GetComponent<Protein>());

                //Destroy(currProtein.GetComponent<FogOfWarHider>()); // using for debug!!!!

                break;
            }

            testcases++;
            if (testcases >= 5000) {
                Debug.LogError("MAX TEST CASE REACHED!!!");
                break;
            }
        } while (true);


    }

    IEnumerator SpawnProteinOnce(float delay) {
        yield return new WaitForSeconds(delay);
        GameObject currProtein = null;

        for (int i = 0; i < maxProteins; i++) {
            Vector3 spawnPos = Vector3.zero;
            int count = 0;
            do {
                float _x = transform.position.x + Random.Range(-transform.localScale.x * 5, transform.localScale.x * 5);
                float _z = transform.position.z + Random.Range(-transform.localScale.z * 5, transform.localScale.z * 5);
                spawnPos = new Vector3(_x, transform.position.y + 0.5f, _z);

                if (spawnPositionList.Count <= 0) {
                    spawnPositionList.Add(spawnPos); break;
                } 
                foreach (var tmp in spawnPositionList) {
                    if (Vector3.Distance(tmp, spawnPos) > proteinRadius * 2.5f)
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
        for (int i = 0; i < spawnPositionList.Count; i++) {


            currProtein = PhotonNetwork.connected ? PhotonNetwork.InstantiateSceneObject("Protein", spawnPositionList[i], spwanAngle, 0, null):Instantiate(protein, spawnPositionList[i], spwanAngle) as GameObject;
            currProtein.transform.parent = transform.parent;
            
            //Destroy(currProtein.GetComponent<FogOfWarHider>()); // using for debug!!!!

            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewProtein(currProtein.GetComponent<Protein>());
        }
        Debug.Log(testcases + " cases.");
        testcases = 0;
        preSpawned = true;
    }

}
