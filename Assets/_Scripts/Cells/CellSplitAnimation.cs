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

    public void CreateHeatCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("HeatCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] {(bool)false}) as GameObject 
            : GameObject.Instantiate(gHeatCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }
        Destroy(gameObject);
        originCell.Die();
    }

    public void CreateTier2HeatCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("Tier2HeatCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject 
            : GameObject.Instantiate(gTier2HeatCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }
        Destroy(gameObject);
        originCell.Die();
        originCell1.Die();
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
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }

        Destroy(gameObject);
        originCell.Die();
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
                GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
            }
        }
        Destroy(gameObject);
        originCell.Die();
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
                GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
            }
        }
        Destroy(gameObject);
        originCell.Die();
    }

    public void CreateHeatandCancerCells()
    {
        Vector3 pos = transform.position;

        pos.x -= 0.5f;
        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("CancerCell", transform.GetChild(0).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject 
            : GameObject.Instantiate(gCancerCellPrefab, transform.GetChild(0).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;



        pos.x += 1.0f;
        newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("HeatCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject 
            : GameObject.Instantiate(gHeatCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;


        Destroy(gameObject);
        originCell.Die();
    }

    public void CreateTier2ColdCell()
    {

        GameObject newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("Tier2ColdCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject 
            : GameObject.Instantiate(gTier2ColdCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }
        Destroy(gameObject);
        originCell.Die();
        originCell1.Die();
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
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }
        Destroy(gameObject);
        originCell.Die();
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
                GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
            }
        }
        Destroy(gameObject);
        originCell.Die();
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
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }


        newcell = PhotonNetwork.connected ? PhotonNetwork.Instantiate("CancerCell", transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0, new object[] { (bool)false }) as GameObject 
            : GameObject.Instantiate(gCancerCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;



        Destroy(gameObject);
        originCell.Die();
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
        Destroy(gameObject);
        originCell.Die();
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
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }
        Destroy(gameObject);
        originCell.Die();
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
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }
        Destroy(gameObject);
        originCell.Die();
    }






}
