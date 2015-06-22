using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public static class GameObjectManager {





    public static List<GameObject> FindAllUnits()
    {
        return GameObject.FindGameObjectsWithTag("Unit").ToList<GameObject>();
    }


    public static List<GameObject> FindAIUnits()
    {
        List<GameObject> allAIunits = FindAllUnits();
        if (allAIunits.Count > 0 )
        {
            allAIunits.RemoveAll(item => item.GetComponent<BaseCell>().isAIPossessed == false);
        }
       
        return allAIunits;
    }

    public static List<GameObject> FindPlayerUnits()
    {
        List<GameObject> allPlayerUnits = FindAllUnits();
        if (allPlayerUnits.Count > 0)
        {
            allPlayerUnits.RemoveAll(item => item.GetComponent<BaseCell>().isAIPossessed == true);
        }

        return allPlayerUnits;
    }

    public static List<GameObject> FindTheirUnits()
    {
        List<GameObject> allTheirUnits = FindAllUnits();
        if (allTheirUnits.Count > 0)
        {
            allTheirUnits.RemoveAll(item => item.GetComponent<BaseCell>().isMine == true);
        }

        return allTheirUnits;
    }
}