using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/Actions/Stop")]
public class StopAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        fsm.GetNavMeshAgent().StopAgent();
    }
}
