using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void SplitEvent();
    public static event SplitEvent OnSplit;

    public delegate void MoveEvent(Vector3 _dest);
    public static event MoveEvent OnMove;

    public void Split()
    {
        OnSplit();
    }

    public static void Move(Vector3 _dest)
    {
        OnMove(_dest);
    }
}
