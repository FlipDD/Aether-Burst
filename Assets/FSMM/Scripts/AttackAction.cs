using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Finite State Machine/Actions/Attack")]
public class AttackAction : Action
{
    public GameObject shootPrefab;
    public float shootTimeInterval = 2;
    private float shootTime = 0;
    private GameObject agent;
    private Vector3 targetPosition;
    public Vector3 velocity = Vector3.zero;


    public override void Act(FiniteStateMachine fsm)
    {
        
        if (fsm.GetNavMeshAgent().stagered == false && fsm.GetNavMeshAgent().player.GetComponent<PlayerInputController>().enemystop == false)
        {
           
            fsm.GetNavMeshAgent().CheckTimeofFire();

            targetPosition = new Vector3(fsm.GetNavMeshAgent().GetTarget().position.x, fsm.GetNavMeshAgent().GetTarget().position.y, fsm.GetNavMeshAgent().GetTarget().position.z);

           
            var dist = Vector3.Distance(fsm.GetNavMeshAgent().GetTarget().position, fsm.transform.position);
  
            fsm.transform.LookAt(targetPosition);
            if (dist < 205)
            {
                if (fsm.GetNavMeshAgent().notattacking == true && fsm.GetNavMeshAgent().notattacking2 == false)
                {
                    fsm.GetNavMeshAgent().attacking = false;
                    fsm.GetNavMeshAgent().StartCoroutine(WaitAndAttack(3f));


                }
                if (fsm.GetNavMeshAgent().notattacking2 == true && fsm.GetNavMeshAgent().notattacking == false && fsm.GetNavMeshAgent().charging == false)
                {
                    fsm.GetNavMeshAgent().chargeattack = false;
                    fsm.GetNavMeshAgent().StartCoroutine(ChargeAndAttack(3f));
                }

            }



            //shootTime += Time.deltaTime;
            //if (shootTime > shootTimeInterval)
            //{
            //shootTime = 0;
            //fsm.GetNavMeshAgent().enemyenergy -= 5f;
            //GameObject bullet = Instantiate(shootPrefab, fsm.transform.position + fsm.transform.forward, Quaternion.identity);
            //bullet.GetComponent<Rigidbody>().velocity = (fsm.GetNavMeshAgent().GetTarget().position - fsm.transform.position).normalized * 10;
            //}

            IEnumerator WaitAndAttack(float waitTime)
            {


                float step = fsm.GetNavMeshAgent().EnemyMoveSpeed * Time.deltaTime;
                fsm.GetNavMeshAgent().transform.position = Vector3.Lerp(fsm.GetNavMeshAgent().transform.position,
                                new Vector3(targetPosition.x, targetPosition.y, targetPosition.z),
                                1f * Time.deltaTime);
                yield return new WaitForSeconds(waitTime);


                fsm.GetNavMeshAgent().notattacking = false;
                fsm.GetNavMeshAgent().i = 0;
                //yield return new WaitForSeconds(10f);




            }

            IEnumerator ChargeAndAttack(float waitTime)
            {
                fsm.GetNavMeshAgent().charging = true;
                float step = fsm.GetNavMeshAgent().EnemyMoveSpeed * Time.deltaTime;



                yield return new WaitForSeconds(waitTime);
                fsm.GetNavMeshAgent().transform.position = Vector3.SmoothDamp(fsm.GetNavMeshAgent().transform.position,
                                new Vector3(fsm.GetNavMeshAgent().positionToGox, fsm.GetNavMeshAgent().positiontoGoy, fsm.GetNavMeshAgent().transform.position.z),
                                ref velocity,
                                8.5f * Time.deltaTime);


                fsm.GetNavMeshAgent().notattacking2 = false;
                fsm.GetNavMeshAgent().i2 = 0;

                //yield return new WaitForSeconds(10f);




            }
        }

    }

}
