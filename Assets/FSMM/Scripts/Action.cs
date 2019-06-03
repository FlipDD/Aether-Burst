using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract void Act (FiniteStateMachine fsm);
}
