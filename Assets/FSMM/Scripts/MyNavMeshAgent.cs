using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyNavMeshAgent : EnemyBase
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform[] waypoints;
    private int currentWaypoint;
    public NavMeshAgent agent;
    public float enemyenergy;
    public bool recovered;
    public bool stayrecovering;
    public float firerate;
    float nextfire;
    public float firerate2;
    float nextfire2;
    public bool attacking;
    ObjectPooler objectPooler;
    private GameObject Player;
    public GameObject hitParticle;

    public State state;
    public bool startedMoving;
    public Vector3 goToPosition;
    public float EnemyMoveSpeed = 10.0f;
    private ParticleSystem psWater;
    private Rigidbody rigidbody;
    public int i;
    public int i2;
    private Vector3 targetPosition;
    public bool notattacking;
    public bool isWaiting;
    public bool chargeattack;
    private float chargetimer;
    public bool notattacking2;
    private Animator anim;
    
    public float positionToGox;
    public float positiontoGoy;
    public bool charging;
    public bool stagered;
    private float recovertimer;
     private HealthComponent health;
    private bool burnbaby;
    internal GameObject player;
    float firsttime;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Start()
    {
       
        health = GetComponent<HealthComponent>();
        recovertimer = 20f;
        stagered = false;
        agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = 5f;
        currentWaypoint = 0;
        enemyenergy = 50;
        recovered = false;
        stayrecovering = false;
        anim = gameObject.GetComponent<Animator>();
        objectPooler = ObjectPooler.Instance;
        //firerate = 2f;
        nextfire = Time.time;
        Player = GameObject.Find("Player");
    
        rigidbody = GetComponent<Rigidbody>();
     
        i = 0;
    }

    private void FixedUpdate()
    {

        if (burning && !takingDamage)
        {
           
            StartCoroutine(BurnDoT(health, 23));
            rigidbody.useGravity = true;

            stagered = true;
        }
        
       
        if (enemyenergy < 0)
        {
            recovered = true;
        }


        if (stagered == true)
        {
            recovertimer -= Time.deltaTime;
        }

        if(recovertimer < 1)
            agent.enabled = true;

        if (recovertimer < 0)
        {
            i = 0;
            charging = false;
            chargeattack = false;
            attacking = false;
            notattacking = false;
            notattacking2 = false;
            rigidbody.useGravity = false;
            stagered = false;
            
            recovertimer = 20f;
        }


    }

   

   public void CheckTimeofFire()
    {


        var dist = Vector3.Distance(Player.transform.position, transform.position);

        if (dist > 50)
        {
            
            //agent.enabled = true;
           

        }


        if (notattacking == false&&stagered==false)
        {
           
            if (dist < 200)
            {
               
                if (dist > 25)
                {
                    chargeattack = false;
                    attacking = true;
                }
                else
                {
                    attacking = false;
                    chargeattack = true;
                }
            }
            else
            {
                attacking = false;
                chargeattack = false;
                chargetimer = 4.0f;
            }
        }
        if (Time.time > nextfire && attacking == true && chargeattack == false)
        {
            agent.enabled = false;
            anim.SetTrigger("Small");
            ObjectPooler.Instance.SpawnFromPool("EnemyBullet", transform.position, Quaternion.identity);
            i++;
            nextfire2 = Time.time + firerate2;
            nextfire = Time.time + firerate;
           
        }

        if (Time.time > nextfire2 && chargeattack == true && attacking == false)
        {
            agent.enabled = false;
            anim.SetTrigger("Charging");
            if(firsttime > 1f)
            ObjectPooler.Instance.SpawnFromPool("ChargedBullet", transform.position, Quaternion.identity);
            firsttime = 2f;
            chargeattack = false;

            nextfire = Time.time + firerate;
            nextfire2 = Time.time + firerate2;
            i++;
           
            
            positionToGox = transform.position.x + Random.Range(-20f,20f);
            positiontoGoy = transform.position.y + Random.Range(1f, 4f);
        }
        if (i == 2)
        {
            notattacking = true;
        }
        if (i == 1)
        {
            notattacking2 = true;
        }
        if (i == 0)
            charging = false;


    }

    internal void GoToNextWaypoint()
    {
        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint++;
        if (currentWaypoint >= waypoints.Length)
            currentWaypoint = 0;
    }

    internal void GoToTarget()
    {
        agent.destination = target.position;
    }

    internal Transform GetTarget()
    {
        return target;
    }

    internal void StopAgent()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    internal void RecoverAgent()
    {
        
        agent.isStopped = true;
        agent.ResetPath();
        //GoToNextWaypoint();
        
    }

    internal bool IsAtDestination()
    {
        if (!agent.pathPending)
            if (agent.remainingDistance <= agent.stoppingDistance)
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
                    return true;

        return false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (Player.GetComponent<PlayerInputController>().noattack == false)
        {
            if (collision.collider.tag == "Sword")
            {
                Player.GetComponent<PlayerInputController>().playshaker = true;
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                GameObject hitparticle = Instantiate(hitParticle, pos, rot);
                //Destroy(hitparticle);
                rigidbody.useGravity = true;

                stagered = true;
                //agent.enabled = true;
                //enemyenergy = -1f;
            }
        }

        if (collision.collider.tag == "Boulder")
        {
            stagered = true;
            rigidbody.useGravity = true;
            gameObject.SetActive(false);
        }
        }
}
