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
        curCell.targets.Clear();
        curCell.SetPrimaryTarget(null);
        curCell.Move(curCell.transform.position);
        curCell.currentState = CellState.IDLE;
        switch (curCell.celltype)
        {
            case CellType.HEAT_CELL:
                curCell.GetComponent<HeatCell>().Merge();
                break;
            case CellType.COLD_CELL:
                curCell.GetComponent<ColdCell>().Merge();
                break;
            case CellType.ACIDIC_CELL:
                curCell.GetComponent<AcidicCell>().Merge();
                break;
            case CellType.ALKALI_CELL:
                curCell.GetComponent<AlkaliCell>().Merge();
                break;
            default:
                break;
        }
    }
}
