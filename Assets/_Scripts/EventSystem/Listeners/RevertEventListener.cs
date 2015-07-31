using UnityEngine;
using System.Collections;

public class RevertEventListener : MonoBehaviour {

    void OnEnable()
    {
        EventManager.OnRevert += Revert;
    }


    void OnDisable()
    {
        EventManager.OnRevert -= Revert;
    }

    void Revert()
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
        curCell.Revert();
        
    }
}
