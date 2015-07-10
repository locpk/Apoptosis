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
        curCell.Revert();
        
    }
}
