using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.GetNavMeshAgent().IsAtDestination())
            fsm.GetNavMeshAgent().GoToNextWaypoint();
    }
}
