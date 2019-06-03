using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/State")]
public class State : ScriptableObject
{
    [SerializeField]
    private Action[] actions;
    [SerializeField]
    private Action entryAction;
    [SerializeField]
    private Action exitAction;
    [SerializeField]
    private Transition[] transitions;   

    public Action[] GetActions ()
    {
        return actions;
    }

    public Action GetEntryAction ()
    {
        return entryAction;
    }

    public Action GetExitAction ()
    {
        return exitAction;
    }

    public Transition[] GetTransitions ()
    {
        return transitions;
    }
}
