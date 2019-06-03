using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        fsm.GetNavMeshAgent().GoToTarget();
    }
}
