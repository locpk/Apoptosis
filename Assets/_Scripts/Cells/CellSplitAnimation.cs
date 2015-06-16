﻿using UnityEngine;
using System.Collections;

public class CellSplitAnimation : MonoBehaviour
{

    public GameObject gCancerCellPrefab;
    public GameObject gStemCellPrefab;
    public GameObject gHeatCellPrefab;
    public GameObject gColdCellPrefab;


    public int currentLevel;
    public float currentProtein;
    public bool isAIPossessed = false;
    public void CreateStemCells()
    {
        Vector3 pos = transform.position; 
        pos.x -= 0.5f;
        for (int i = 0; i < 2; i++)
        {
            GameObject newcell = GameObject.Instantiate(gStemCellPrefab, pos, Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
            newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
            newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
            newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
            newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
            pos.x += 1.0f;
        }
        Destroy(gameObject);
    }


    public void CreateHeatCells()
    {
        Vector3 pos = transform.position;
        pos.x -= 0.5f;
        for (int i = 0; i < 2; i++)
        {
            GameObject newcell = GameObject.Instantiate(gHeatCellPrefab, pos, Quaternion.identity) as GameObject;
            newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
            newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
            newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
            newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
            newcell.gameObject.transform.Rotate(90, -180, -180);
            pos.x += 1.0f;
        }
        Destroy(gameObject);
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
    }



    public void CreateColdCells()
    {
        Vector3 pos = transform.position;
        pos.x -= 0.5f;
        for (int i = 0; i < 2; i++)
        {
            GameObject newcell = GameObject.Instantiate(gColdCellPrefab, pos, Quaternion.identity) as GameObject;
            newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
            newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
            newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
            newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
            newcell.gameObject.transform.Rotate(90, -180, -180);
            pos.x += 1.0f;
        }
        Destroy(gameObject);
    }

    public void CreateColdandCancerCells()
    {
        Vector3 pos = transform.position;
        pos.x -= 0.5f;
        GameObject newcell = GameObject.Instantiate(gCancerCellPrefab, pos, Quaternion.identity) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        newcell.gameObject.transform.Rotate(90, -180, -180);


        pos.x += 1.0f;
        newcell = GameObject.Instantiate(gColdCellPrefab, pos, Quaternion.identity) as GameObject;
        newcell.GetComponent<BaseCell>().currentProtein = currentProtein;
        newcell.GetComponent<BaseCell>().isAIPossessed = isAIPossessed;
        newcell.GetComponent<BaseCell>().currentLevel = currentLevel;
        newcell.GetComponent<BaseCell>().currentState = CellState.IDLE;
        newcell.gameObject.transform.Rotate(90, -180, -180);


        Destroy(gameObject);
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
    }






    public void UpdateProtein()
    {
        transform.GetChild(0).GetComponent<BaseCell>().currentProtein = currentProtein;
        transform.GetChild(1).GetComponent<BaseCell>().currentProtein = currentProtein;
    }


}