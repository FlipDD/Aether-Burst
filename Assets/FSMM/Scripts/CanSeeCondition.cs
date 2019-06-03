using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/Conditions/CanSeeCondition")]
public class CanSeeCondition : Condition
{
    [SerializeField]
    private bool negation;
    [SerializeField]
    private float viewAngle;
    [SerializeField]
    private float viewDistance;

    public override bool Test(FiniteStateMachine fsm)
    {
        Transform target = fsm.GetNavMeshAgent().GetTarget();
        Vector3 targetDir = target.position - fsm.transform.position;
        float angle = Vector3.Angle(targetDir.normalized, fsm.transform.forward);
        float distance = Vector3.Distance(target.position, fsm.transform.position);
        if ((angle < viewAngle) && (distance < viewDistance))
        {
            //cast ray check for collisions
            return !negation;
        }
        else
        {
            return negation;
        }
    }
    
}
