using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public static class GameObjectManager {





    public static List<GameObject> FindAllUnits()
    {
        return GameObject.FindGameObjectsWithTag("Unit").ToList<GameObject>();
    }
    public List<GameObject> AiUnits()
   {
      System.Collections.Generic.List<GameObject> cells = GameObject.FindGameObjectsWithTag("Unit").ToList<GameObject>();
      for(int i = 0; i < cells.Count; i++)
      {
          if(!cells[i].GetComponent<BaseCell>().isAIPossessed)
          {
              cells.RemoveAt(i);
          }
      }
      return cells;
    
   }


    public static List<GameObject> FindAIUnits()
    {
        List<GameObject> allAIunits = GameObject.FindGameObjectsWithTag("Unit").ToList<GameObject>();
        allAIunits.RemoveAll(item => item.GetComponent<BaseCell>().isAIPossessed == false);
        return allAIunits;
    }


}