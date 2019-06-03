using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/Actions/EnergyRecoveryAction")]
public class EnergyRecoveryAction : Action
{
    
    private GameObject agent;

    public override void Act(FiniteStateMachine fsm)
    {

        if (fsm.GetNavMeshAgent().enemyenergy < 50)
        {
            fsm.GetNavMeshAgent().recovered = true;
            fsm.GetNavMeshAgent().enemyenergy += 0.002f;
        }
        else
        {
            fsm.GetNavMeshAgent().recovered = false;
        }
    }
}
