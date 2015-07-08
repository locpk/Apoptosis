using UnityEngine;
using System.Collections;

public class AttackEventListener : MonoBehaviour {

    void OnEnable()
    {
        EventManager.OnAttack += Attack;
    }


    void OnDisable()
    {
        EventManager.OnAttack -= Attack;
    }

    void Attack(GameObject _target)
    {
        BaseCell curCell = this.GetComponent<BaseCell>();
        if (!curCell.isSelected)
        {
            return;
        }
        curCell.Attack(_target);
    }
}
