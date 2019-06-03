using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    // public float EnemyMoveSpeed = 10.0f;
    // private Transform target;
    // private Rigidbody rigidbody;
    // private ParticleSystem psWater;
    // private Vector3 goToPosition;
    // bool startedMoving;
    // private GameObject player;

    // public State state;
    // public GameObject hitParticle;
    // private bool Destroy;

    // public enum State
    // {
    //     Idle,
    //     Frozen,
        
    //     Chasing
    // } 

    // public void Start()
    // {
    //     player = GameObject.Find("Player");
    //     target = GameObject.Find("Player").transform;
    //     rigidbody = GetComponent<Rigidbody>();
    //     state = State.Chasing;
    // }

    // public void DummyBehaviour()
    // {
        
    // }   

    // void FixedUpdate()
    // {
    //     switch (state)
    //     {
    //         case State.Idle:
    //         break;

    //         case State.Chasing:
    //             Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
    //             transform.LookAt(targetPosition);
    //             float step = EnemyMoveSpeed * Time.deltaTime;
    //             transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    //         break;

    //         case State.Frozen:
    //             if (!startedMoving)
    //                 StartCoroutine(Freeze());
    //             else
    //                 transform.position = Vector3.Lerp(transform.position, goToPosition, .05f);
    //         break;
    //     }
    
    // }

    // IEnumerator Freeze ()
    // {
    //     startedMoving = true;
    //     goToPosition = transform.position + (Vector3.up * 6);
    //     Transform ps = Instantiate(GameAssets.i.waterPrison, transform.position, Quaternion.identity);
    //     ps.transform.parent = transform;
    //     rigidbody.isKinematic = true;
    //     yield return new WaitForSeconds(7);
    //     int index = transform.childCount - 1;
    //     Destroy(transform.GetChild(index).gameObject);
    //     rigidbody.isKinematic = false;
    //     startedMoving = false;
    //     state = State.Chasing;
    // }

    // public void Froze ()
    // {
    //     state = State.Frozen;
    // }

    // public void OnCollisionEnter(Collision collision)
    // {
    //     if (player.GetComponent<PlayerInputController>().noattack == false)
    //     {
    //         if (collision.collider.tag == "Sword")
    //         {
    //             ContactPoint contact = collision.contacts[0];
    //             Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //             Vector3 pos = contact.point;
    //             GameObject hitparticle = Instantiate(hitParticle, pos, rot);
    //             //Destroy(hitparticle);
    //         }
    //     }
    // }
}
