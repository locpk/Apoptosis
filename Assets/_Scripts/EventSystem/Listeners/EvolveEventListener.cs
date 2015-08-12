using UnityEngine;
using System.Collections;

public class EvolveEventListener : MonoBehaviour
{

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
        int nSelStem = GameObject.Find("PlayerControl").GetComponent<PlayerController>().allSelectableUnits.FindAll(item => item.GetComponent<BaseCell>().celltype == CellType.STEM_CELL).Count;
        if (nSelStem != GameObject.Find("PlayerControl").GetComponent<PlayerController>().NumStemCells)
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
}
