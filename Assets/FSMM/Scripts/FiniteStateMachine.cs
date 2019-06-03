using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField]
    private State initialState;
    private State currentState;
    private MyNavMeshAgent navAgent;

    void Start ()
    {
        currentState = initialState;
        navAgent = GetComponent<MyNavMeshAgent>();
    }

    internal MyNavMeshAgent GetNavMeshAgent()
    {
        return navAgent;
    }

    void Update ()
    {
        Transition triggeredTransition = null;
        foreach (Transition t in currentState.GetTransitions())
        {
            if (t.IsTriggered(this))
            {
                triggeredTransition = t;
                break;
            }
        }

        List<Action> actions = new List<Action>();
        if (triggeredTransition)
        {
            State targetState = triggeredTransition.GetTargetState();
            Action tmpAction;
            tmpAction = currentState.GetExitAction();
            if (tmpAction)
                actions.Add(tmpAction);
            tmpAction = triggeredTransition.GetAction();
            if (tmpAction)
                actions.Add(tmpAction);
            tmpAction = targetState.GetEntryAction();
            if (tmpAction)
                actions.Add(tmpAction);

            currentState = targetState;
        }
        else
        {
            foreach (Action a in currentState.GetActions())
            {
                actions.Add(a);
            }
        }

        DoActions(actions);

    }

    void DoActions (List<Action> actions)
    {
        foreach (Action a in actions)
        {
            a.Act(this);
        }
    }
}
