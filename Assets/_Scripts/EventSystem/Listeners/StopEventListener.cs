using UnityEngine;
using System.Collections;

public class StopEventListener : MonoBehaviour {

    void OnEnable()
    {
        EventManager.OnStop += Stop;
    }


    void OnDisable()
    {
        EventManager.OnStop -= Stop;
    }

    void Stop()
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

    }
}
