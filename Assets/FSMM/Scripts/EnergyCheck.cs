using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/Conditions/EnergyCondition")]
public class EnergyCheck : Condition
{
    [SerializeField]
    private bool negation;

    public override bool Test(FiniteStateMachine fsm)
    {
        float energyLeft = fsm.GetNavMeshAgent().enemyenergy;

        if (fsm.GetNavMeshAgent().recovered == true)
        {
            // Debug.Log("OLA");
            return !negation;

        }
        else
        { 
         
            return negation;
        }
    }

}
