using UnityEngine;
using System.Collections;

public class IdleState : BaseState {

    
    public override void Input()
    {

    }

    public override void Update(BaseCell _cell)
    {
        _cell.Deplete(Time.deltaTime);
        if (_cell.currentProtein <= 0.0f)
        {
            _cell.Die();
        }
    }
}
