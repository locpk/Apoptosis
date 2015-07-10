using UnityEngine;
using System.Collections;

public class SplitEventListener : MonoBehaviour
{

    void OnEnable()
    {
        EventManager.OnSplit += Split;
    }


    void OnDisable()
    {
        EventManager.OnSplit -= Split;
    }

    void Split()
    {
        BaseCell curCell = this.GetComponent<BaseCell>();
        if (!curCell.isSelected)
        {
            return;
        }
        switch (curCell.celltype)
        {
            case CellType.STEM_CELL:
                curCell.PerfectSplit();
                break;
            case CellType.HEAT_CELL:
                curCell.CancerousSplit();
                break;
            case CellType.COLD_CELL:
                curCell.CancerousSplit();
                break;
            default:
                break;
        }


    }
}
