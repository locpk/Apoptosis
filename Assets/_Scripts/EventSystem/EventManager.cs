using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void EventAction();

    public static event EventAction OnSplit;

    public void Split()
    {
        OnSplit();
    }
}
