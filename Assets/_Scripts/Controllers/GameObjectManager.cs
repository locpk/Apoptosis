using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameObjectManager : MonoBehaviour {

    public List<GameObject> AllUnits;



   public List<GameObject> FindAllUnits()
    {
        return GameObject.FindGameObjectsWithTag("Unit").ToList<GameObject>();
    }
    public List<GameObject> AiUnits()
   {
      System.Collections.Generic.List<GameObject> cells = GameObject.FindGameObjectsWithTag("Unit").ToList<GameObject>();
      for(int i = 0; i < cells.Count; i++)
      {
          if(cells[i].GetComponent<BaseCell>().isAIPossessed)
          {
              cells.RemoveAt(i);
          }
      }
      return cells;
    
   }



	void Awake() {
        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
       
    }

	//LateUpdate is called after all Update functions have been called
	void LateUpdate() {
        
    }
}