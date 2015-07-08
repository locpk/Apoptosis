using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void SplitEvent();
    public static event SplitEvent OnSplit;
    public static void Split()
    {
        OnSplit();
    }

    public delegate void MoveEvent(Vector3 _dest);
    public static event MoveEvent OnMove;

    public static void Move(Vector3 _dest)
    {
        OnMove(_dest);
    }

    public delegate void AttackMoveEvent(Vector3 _dest);
    public static event AttackMoveEvent OnAttackMove;

    public static void AttackMove(Vector3 _dest)
    {
        OnAttackMove(_dest);
    }


    public delegate void AttackEvent(GameObject _target);
    public static event AttackEvent OnAttack;

    public static void Attack(GameObject _target)
    {
        OnAttack(_target);
    }

    public delegate void EvolveEvent(CellType _newType);
    public static event EvolveEvent OnEvolve;

    public static void Evolve(CellType _newType)
    {
        OnEvolve(_newType);
    }

    public delegate void ConsumeEvent(GameObject _target);
    public static event ConsumeEvent OnConsume;

    public static void Consume(GameObject _target)
    {
        OnConsume(_target);
    }
}
