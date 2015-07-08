using UnityEngine;
using System.Collections;

public class ConsumeEventListener : MonoBehaviour {

    void OnEnable()
    {
        EventManager.OnConsume += Consume;
    }


    void OnDisable()
    {
        EventManager.OnConsume -= Consume;
    }

    void Consume(GameObject _target)
    {
        BaseCell curCell = this.GetComponent<BaseCell>();
        if (!curCell.isSelected)
        {
            return;
        }
        curCell.Consume(_target);

    }
}
