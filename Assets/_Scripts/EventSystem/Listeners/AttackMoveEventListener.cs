using UnityEngine;
using System.Collections;

public class AttackMoveEventListener : MonoBehaviour
{

    void OnEnable()
    {
        EventManager.OnAttackMove += AttackMove;
    }


    void OnDisable()
    {
        EventManager.OnAttackMove -= AttackMove;
    }

    void AttackMove(Vector3 _dest)
    {
        BaseCell curCell = this.GetComponent<BaseCell>();
        if (!curCell.isSelected)
        {
            return;
        }
        curCell.AttackMove(_dest);
    }
}
