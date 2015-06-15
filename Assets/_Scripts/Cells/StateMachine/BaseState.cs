using UnityEngine;
using System.Collections;

public abstract class BaseState
{

    abstract public void Input();

    abstract public void Update(BaseCell _cell);
}
 