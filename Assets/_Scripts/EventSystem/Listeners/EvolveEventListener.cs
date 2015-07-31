using UnityEngine;
using System.Collections;

public class EvolveEventListener : MonoBehaviour {

    void OnEnable()
    {
        EventManager.OnEvolve += Evolve;
    }


    void OnDisable()
    {
        EventManager.OnEvolve -= Evolve;
    }

    void Evolve(CellType _type)
    {
        StemCell curCell = this.GetComponent<StemCell>();
        if (!curCell.isSelected)
        {
            return;
        }
        curCell.targets.Clear();
        curCell.SetPrimaryTarget(null);
        curCell.Move(curCell.transform.position);
        curCell.currentState = CellState.IDLE;
         curCell.Mutation(_type);
       
    }
}
