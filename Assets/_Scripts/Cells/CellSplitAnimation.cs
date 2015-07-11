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



    public void CreateHeatCell()
    {

        GameObject newcell = GameObject.Instantiate(gHeatCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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

        GameObject newcell = GameObject.Instantiate(gTier2HeatCellPrefab, transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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

        GameObject newcell = GameObject.Instantiate(gStemCellPrefab, transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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
            GameObject newcell = GameObject.Instantiate(gStemCellPrefab, transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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
            GameObject newcell = GameObject.Instantiate(gHeatCellPrefab, transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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
        GameObject newcell = GameObject.Instantiate(gCancerCellPrefab, pos, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;



        pos.x += 1.0f;
        newcell = GameObject.Instantiate(gHeatCellPrefab, pos, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;


        Destroy(gameObject);
        originCell.Die();
    }

    public void CreateColdCell()
    {

        GameObject newcell = GameObject.Instantiate(gColdCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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
            GameObject newcell = GameObject.Instantiate(gColdCellPrefab, transform.GetChild(i).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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
        GameObject newcell = GameObject.Instantiate(gColdCellPrefab, transform.GetChild(0).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        if (!isAIPossessed)
        {
            newcell.GetComponent<BaseCell>().isMine = true;
            GameObject.Find("PlayerControl").GetComponent<PlayerController>().AddNewCell(newcell.GetComponent<BaseCell>());
        }


        newcell = GameObject.Instantiate(gCancerCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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
            GameObject newcell = GameObject.Instantiate(gCancerCellPrefab, pos, Quaternion.identity) as GameObject;
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

        GameObject newcell = GameObject.Instantiate(gAlkaliCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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

        GameObject newcell = GameObject.Instantiate(gAcidicCellPrefab, transform.GetChild(1).transform.position, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
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
