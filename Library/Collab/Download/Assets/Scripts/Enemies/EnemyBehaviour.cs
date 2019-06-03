using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EnemyBase
{
    private enum State {Idle, Chasing, Attacking, Jumping}
    private State state;

    private List<int> abilitiesIndex;

    public float startDistance = 30;
    [SerializeField]
    private Transform player;
    private GameObject playerObj;
    private float dist;
    private bool jumping, attacking;
    private bool moving;
    private bool shooting;
    private bool clawing;
    private bool choosingAbility;
    private bool startedMoving;
    private Vector3 speed = Vector3.zero;
    Transform rock;
    private HealthComponent health;
    public bool scareAttack;

    private HealthBarLookAt healthBarLookAt;
    private GameObject healthBar;

    private Transform cam;
    private GameObject camObject;

    [SerializeField]
    private ParticleSystem digging;

    [SerializeField] private Transform attackPoint;


    void Start()
    {
        state = State.Idle;
        rgbd = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthComponent>();
        if (player == null) 
        {
            playerObj = GameObject.Find("Player");
            player = playerObj.transform;
        }
        if (cam == null)
        {
            camObject = GameObject.Find("CamBase");
            cam = camObject.transform;
        }

        abilitiesIndex = new List<int>();
    }

    void FixedUpdate()
    {
        dist = Vector3.Distance(transform.position, player.position);
        // if (scareAttack && dist < startPosition)
        // {
        //     StartCoroutine(RockAbility());
        //     scareAttack = false
        // }
        if (burning && !takingDamage) 
            StartCoroutine(BurnDoT(health, 3));
        if (baseState == BaseState.Frozen) 
            state = State.Idle;

        switch(state)
        {
            case State.Idle:
                if (dist < startDistance && baseState != BaseState.Frozen)
                {
                    if (scareAttack)  
                    {
                      StartCoroutine(RockAbility());
                      scareAttack = false;
                    } 
                    else
                        state = State.Chasing;
                }
            break;

            case State.Chasing:
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                transform.position = Vector3.Lerp(transform.position, player.position, Random.Range(.03f, .06f));
                if (!jumping && !shooting && !clawing) StartCoroutine(JumpAttack());
                if (dist < 3) state = State.Attacking;
            break;

            case State.Attacking:
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                if (dist >= 3 && !shooting && !clawing) state = State.Chasing;
                if (!choosingAbility) StartCoroutine(ChangeAbilities());
            break;

            case State.Jumping:
                Vector3 targetPos = transform.position + (Vector3.up * 3);
                if (Vector3.Distance(transform.position, targetPos) > .5f && moving)
                    transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref speed, .5f);
                else 
                    moving = false;
            break;

        }
    }

    IEnumerator JumpAttack()
    {
        rgbd.useGravity = false;
        jumping = true;
        state = State.Jumping;
        moving = true;
        yield return new WaitForSeconds(1.5f);
        attacking = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        transform.rotation *= Quaternion.Euler(90, 0, 0);
        rgbd.AddForce((player.transform.position - transform.position) * 320, ForceMode.Impulse);
        rgbd.useGravity = true;
        yield return new WaitForSeconds(2f);
        attacking = false;
        state = State.Chasing;
        yield return new WaitForSeconds(Random.Range(1f, 2.5f));
        jumping = false;
    }

    IEnumerator ChangeAbilities()
    {
        choosingAbility = true;
        int i = Random.Range(0, 2);
        if (abilitiesIndex.Count > 4)
            abilitiesIndex.RemoveAt(0);
        if (abilitiesIndex.Count-2 == i)
        {
            if (i == 0)
                i = 1;
            else
                i = 0;
        }

        abilitiesIndex.Add(i);

        if (!shooting && !clawing) 
        {
            if (i == 0) StartCoroutine(RockAbility());
            if (i == 1) StartCoroutine(ClawAbility());   
        }
         
        yield return new WaitForSeconds(2.8f);
        choosingAbility = false;
    }

    IEnumerator RockAbility()
    {
        shooting = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(2.8f);
        shooting = false;
    }

    IEnumerator ClawAbility()
    {
        clawing = true;
        animator.SetTrigger("ClawAttack");
        yield return new WaitForSeconds(3.5f);
        clawing = false;
    }

    IEnumerator LookAtPlayer()
    {
        yield return new WaitForSeconds(.8f);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void SpawnRock()
    {
        rock = Instantiate(GameAssets.i.rock, transform.position, Quaternion.identity);
        rock.transform.parent = attackPoint;
        rock.transform.position = attackPoint.position;
    }

    void PlayExplosion()
    {
        //play explosion ps here
    }

    void StartDigging() => digging.Play();

    void StopDigging() => digging.Stop();
    
    void ShootRock()
    {
        Vector3 dir = (player.transform.position - rock.transform.position).normalized;
        rock.transform.parent = null;
        rock.transform.rotation = Quaternion.identity;
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.velocity = dir * 80;
        rock.GetComponent<BoxCollider>().isTrigger = false;

        for (int i = 0; i < rock.childCount; i++)
            rock.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && attacking)
        {
            StartCoroutine(LookAtPlayer());
            rgbd.velocity = Vector3.zero;
            Vector3 dir = player.transform.position - transform.position;
            col.gameObject.GetComponent<Rigidbody>().AddForce(rgbd.velocity * 10, ForceMode.Impulse);
            col.gameObject.GetComponent<UIBar>().UpdateHp(-20);
        }
        else if (col.gameObject.CompareTag("Ground") && jumping)
        {
            StartCoroutine(LookAtPlayer());
            rgbd.velocity = Vector3.zero;
        }
    }
}
