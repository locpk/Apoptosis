using UnityEngine;
using System.Collections;

public class MergeEventListener : MonoBehaviour {

    void OnEnable()
    {
        EventManager.OnMerge += Merge;
    }


    void OnDisable()
    {
        EventManager.OnMerge -= Merge;
    }

    void Merge()
    {
        BaseCell curCell = this.GetComponent<BaseCell>();
        if (!curCell.isSelected)
        {
            return;
        }
        switch (curCell.celltype)
        {
            case CellType.HEAT_CELL:
                curCell.GetComponent<HeatCell>().Merge();
                break;
            case CellType.COLD_CELL:
                curCell.GetComponent<ColdCell>().Merge();
                break;
            default:
                break;
        }
    }
}
