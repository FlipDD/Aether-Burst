using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardBehaviour : EnemyBase
{
    private enum State {Idle, Patrol, Rolling, Swiping, Jumping, Ball, Choosing}
    private State state;

    private List<int> abilitiesIndex;

    public float startDistance = 30;
    [SerializeField]
    private Transform player;
    private GameObject playerObj;
    float dist;
    private bool startedMoving;
    private bool rolling;
    private bool spinning;
    private bool charging;
    private bool attacking;
    private bool looking;
    private bool swiped;
    private bool sawPlayer;
    private bool ballin;
    private bool choosingAbility;
    Vector3 refVel = Vector3.zero;
    private HealthComponent health;
    public LayerMask whatIsPlayer;
    private RaycastHit hit;
    Vector3 pointA, pointB;
    private bool pointing;

    [SerializeField]
    private float patrolX = 10;
    [SerializeField]
    private float patrolZ = 2;

    void Start()
    {
        state = State.Patrol;
        rgbd = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthComponent>();
        if (player == null) 
        {
            playerObj = GameObject.Find("Player");
            player = playerObj.transform;
        }
        
        pointA = new Vector3(transform.position.x + patrolX, transform.position.y, transform.position.z + patrolZ);
        pointB = new Vector3(transform.position.x - patrolX, transform.position.y, transform.position.z - patrolZ);
        
        abilitiesIndex = new List<int>();
    }

    void Update()
    {
        dist = Vector3.Distance(transform.position, player.position);

        if (ballin)
            transform.LookAt(player);

        switch(baseState)
        {
            case BaseState.Frozen:
                if (!freezing)
                    StartCoroutine(Freeze());
                else
                    transform.position = Vector3.Lerp(transform.position, goToPosition, .05f);
            break;

            case BaseState.Burning:
                if (!burning)
                    StartCoroutine(Burn());
               
            break;
        }

        if (burning && !takingDamage) 
            StartCoroutine(BurnDoT(health));

        if (dist < startDistance && !sawPlayer && !ballin)
        {
            state = State.Rolling;
            sawPlayer = true;
        }

        if (baseState == BaseState.Frozen) state = State.Idle;
         
        switch(state)
        {
            case State.Idle:
            break;

            case State.Choosing:
                if (dist <= 7)
                    state = State.Swiping;
                else if (!choosingAbility)
                    StartCoroutine(ChangeAbilities());
            break;

            case State.Patrol:
                if (!pointing)
                {
                    Vector3 dir = (transform.position - pointA).normalized;
                    transform.LookAt(pointA);
                    rgbd.AddForce(transform.forward * 88000 * Time.deltaTime);
                    if (Vector3.Distance(transform.position, pointA) < 1f)
                        pointing = true;
                }
                else
                {
                    Vector3 dir = (transform.position - pointB).normalized;
                    transform.LookAt(pointB);
                    rgbd.AddForce(transform.forward * 88000 * Time.deltaTime);
                    if (Vector3.Distance(transform.position, pointB) < 1f)
                        pointing = false;
                }
            break;

            case State.Rolling:
                Vector3 tmpPlayer = player.position;
                if (!rolling) 
                    StartCoroutine(RollAttack());
                else if (rolling && charging && dist > 4)
                {
                    transform.LookAt(player);
                    transform.position += transform.forward * Time.deltaTime * 10;
                }
                if (looking) transform.LookAt(tmpPlayer, Vector3.up);

            break;

            case State.Swiping:
                if ((rolling && charging && !ballin)) //remove ||dist <6
                {
                    StartCoroutine(SwipeAttack());
                    charging = false;
                }
            break;

            case State.Ball:
                transform.LookAt(player);

                if (!ballin)
                    StartCoroutine(Ballin());
            break;
        } 
    }

    IEnumerator ChangeAbilities()
    {
        choosingAbility = true;
        int i = UnityEngine.Random.Range(0, 2);
        if (abilitiesIndex.Count > 10)
            abilitiesIndex.RemoveAt(0);
        if (abilitiesIndex.Count-2 == i)
        {
            if (i == 0)
                i = 1;
            else
                i = 0;
        }

        abilitiesIndex.Add(i);

        if (i == 0 && ((charging && rolling) || (!charging && !rolling))) state = State.Ball;
        if (i == 1 && !rolling) state = State.Rolling;   
         
        yield return new WaitForSeconds(2f);
        choosingAbility = false;
    }

    private IEnumerator Ballin()
    {
        ballin = true;
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), .05f);
        yield return new WaitForSeconds(2);
        Transform tf1 = Instantiate(GameAssets.i.lizardSpikes, transform.position, Quaternion.identity);
        tf1.parent = transform;
        yield return new WaitForSeconds(.9f);
        Transform tf2 = Instantiate(GameAssets.i.lizardSpikes, transform.position, Quaternion.identity);
        tf2.parent = transform;
        yield return new WaitForSeconds(.7f);
        Transform tf3 = Instantiate(GameAssets.i.lizardSpikes, transform.position, Quaternion.identity);
        tf3.parent = transform;
        yield return new WaitForSeconds(.5f);
        Transform tf4 = Instantiate(GameAssets.i.lizardSpikes, transform.position, Quaternion.identity);
        tf4.parent = transform;
        yield return new WaitForSeconds(1.5f);
        Transform tf5 = Instantiate(GameAssets.i.lizardSpikes, transform.position, Quaternion.identity);
        tf5.parent = transform;
        tf5.localScale *= 2.4f;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 2f));
        state = State.Choosing;
        ballin = false;
    }

    //!(boolean love) 
    IEnumerator RollAttack()
    {
        rolling = true;
        looking = true;
        transform.LookAt(player);
        yield return new WaitForSeconds(1.5f);
        transform.LookAt(player);
        animator.SetTrigger("Spinning");
        spinning = true;
        yield return new WaitForSeconds(1.5f);
        transform.LookAt(player);
        looking = false;
        transform.LookAt(player);    
        rgbd.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z) * 6000, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        spinning = false;
        animator.SetTrigger("Charging");
        yield return new WaitForSeconds(1.5f);
        charging = true;
        state = State.Choosing;
        yield return new WaitForSeconds(UnityEngine.Random.Range(4, 6.5f));
        charging = false;
        rolling = false;
    }

    IEnumerator SwipeAttack()
    {
        yield return new WaitForSeconds(.3f);
        animator.SetTrigger("Swiping");
        swiped = true;
        yield return new WaitForSeconds(2);
        swiped = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(2, 3.3f));
        state = State.Choosing;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && (swiped || spinning))
        {
            Vector3 dir = (player.position - transform.position).normalized;
            if (swiped)
            {
                col.gameObject.GetComponent<UIBar>().UpdateHp(-10);
                col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3() * 1250, ForceMode.Impulse);
            }
            else if (spinning)
            {
                col.gameObject.GetComponent<UIBar>().UpdateHp(-25);
                col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3() * 1550, ForceMode.Impulse);
                animator.SetTrigger("Charging");
                spinning = false;
            }
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            state = State.Swiping;
            Vector3 dir = (player.position - transform.position).normalized;
            col.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3() * 1250, ForceMode.Impulse);
        }
        else if (!col.gameObject.CompareTag("Ground"))
        {
            // Debug.Log("gotcha");
            // rgbd.velocity = Vector3.zero;
            // state = State.Idle;
            //Instantiate mesh particle system ground
        }
    }
}
