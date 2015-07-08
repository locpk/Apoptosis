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
         curCell.Mutation(_type);
       
    }
}
