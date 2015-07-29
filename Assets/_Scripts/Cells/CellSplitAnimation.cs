using UnityEngine;
using System.Collections;

public class CellSplitAnimation : MonoBehaviour
{

    public GameObject gCancerCellPrefab;
    public GameObject gStemCellPrefab;
    public GameObject gHeatCellPrefab;
    public GameObject gTier2HeatCellPrefab;
    public GameObject gColdCellPrefab;
    public GameObject gTier2ColdCellPrefab;
    public GameObject gAlkaliCellPrefab;
    public GameObject gAcidicCellPrefab;
    public GameObject gNerveCellPrefab;

    public BaseCell originCell;
    public BaseCell originCell1;
    public int currentLevel;
    public float currentProtein;
    public bool isAIPossessed = false;


    void Awake()
    {
        if (!isAIPossessed)
        {
            gameObject.AddComponent<FogOfWarViewer>();
        }
    }


    public void CreateNerveCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("NerveCell", transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gNerveCellPrefab, transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }
        originCell.Die();
        originCell1.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
            originCell1.photonView.RPC("Die", PhotonTargets.Others, null);
        }

        Destroy(gameObject);
    }

    public void CreateHeatCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("HeatCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gHeatCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateTier2HeatCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("Tier2HeatCell", transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gTier2HeatCellPrefab, transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }
        originCell.Die();
        originCell1.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
            originCell1.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateStemCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("StemCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gStemCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }

        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }
    public void CreateStemCells()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("StemCell", transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
                : GameObject.Instantiate(gStemCellPrefab, transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
            newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
            newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
            newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
            newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
            if (!isAIPossessed)
            {
                newcell.GetComponent<BaseCell>().isMine = true;
            }


        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }


    public void CreateHeatCells()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("HeatCell", transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
                : GameObject.Instantiate(gHeatCellPrefab, transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
            newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
            newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
            newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
            newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
            if (!isAIPossessed)
            {
                newcell.GetComponent<BaseCell>().isMine = true;
            }
        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateHeatandCancerCells()
    {
        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("HeatCell", transform.GetChild(2).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gHeatCellPrefab, transform.GetChild(2).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }


        newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("CancerCell", transform.GetChild(0).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gCancerCellPrefab, transform.GetChild(0).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;



        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateTier2ColdCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("Tier2ColdCell", transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gTier2ColdCellPrefab, transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }
        originCell.Die();
        originCell1.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
            originCell1.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }


    public void CreateColdCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("ColdCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gColdCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateColdCells()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("ColdCell", transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
                : GameObject.Instantiate(gColdCellPrefab, transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
            newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
            newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
            newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
            newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
            if (!isAIPossessed)
            {
                newcell.GetComponent<BaseCell>().isMine = true;
            }
        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateColdandCancerCells()
    {
        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("ColdCell", transform.GetChild(0).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gColdCellPrefab, transform.GetChild(0).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }


        newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("CancerCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gCancerCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;



        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateCancerCells()
    {
        Vector3 pos = transform.position;
        pos.x -= 0.5f;
        for (int i = 0; i < 2; i++)
        {
            GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("CancerCell", transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
                : GameObject.Instantiate(gCancerCellPrefab, transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
            newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
            newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
            newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
            newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
            newcell.gameObject.transform.Rotate(90, -180, -180);
            pos.x += 1.0f;
        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }


    public void CreateAlkaliCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("AlkaliCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gAlkaliCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }

    public void CreateAcidicCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("AcidicCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject
            : GameObject.Instantiate(gAcidicCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
        }
        originCell.Die();
        if (PhotonNetwork.connected)
        {
            originCell.photonView.RPC("Die", PhotonTargets.Others, null);
        }
        Destroy(gameObject);
    }






}
