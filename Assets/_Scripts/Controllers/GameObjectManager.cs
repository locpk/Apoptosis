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
        List<GameObject> allAIunits = GameObject.FindGameObjectsWithTag("Unit").ToList<GameObject>();
        if (allAIunits.Count > 0 )
        {
            allAIunits.RemoveAll(item => item.GetComponent<BaseCell>().isAIPossessed == false);
        }
       
        return allAIunits;
    }


}