using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EnemyBase
{
    private enum State {Idle, Chasing, Attacking, Jumping, Waiting}
    private State state;

    private List<int> abilitiesIndex;

    public float startDistance = 30;
    [SerializeField]
    private Transform player;
    private PlayerInputController playerScript;
    private GameObject playerObj;
    private float dist;
    private bool jumping, attacking, starting;
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
    private SpriteRenderer exclamationMark;
    Vector3 targetPos;
    [SerializeField] private ParticleSystem breakGroundPs;
    private bool breaking;
    float yPos;
    bool waitToIdle;
    internal bool clawAttacking;
    private bool playingLaunchAttack;
    private bool playingVoice;
    private bool counted;

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
        playerScript = player.GetComponent<PlayerInputController>();
        if (cam == null)
        {
            camObject = GameObject.Find("CamBase");
            cam = camObject.transform;
        }

        abilitiesIndex = new List<int>();

        exclamationMark = GetComponentInChildren<SpriteRenderer>();
        exclamationMark.enabled = false;

        // breakGroundPs = GetComponent<ParticleSystem>();
        StartCoroutine(WaitForIdle());

    }

    IEnumerator WaitForIdle()
    {
        waitToIdle = true;
        yield return new WaitForSeconds(3f);
        waitToIdle = false;
        yPos = transform.position.y + 1;
        rgbd.useGravity = false;
    }
    

    internal bool GetJumpingState()
    {
        if (state == State.Jumping)
            return true;
        else
            return false;
    }

    IEnumerator PlayVoice()
    {
        playingVoice = true;
        yield return new WaitForSeconds(Random.Range(3f, 7f));
        int r = Random.Range(0, 3);
        if (r == 0) AudioManager.i.Play("GargoV1", transform.position);
        else if (r == 1) AudioManager.i.Play("GargoV2", transform.position);
        else if (r == 2) AudioManager.i.Play("GargoV3", transform.position);
        playingVoice = false;

    }

    void FixedUpdate()
    {
        if (!playingVoice && state != State.Idle) StartCoroutine(PlayVoice());
        dist = Vector3.Distance(transform.position, player.position);
        // if (scareAttack && dist < startPosition)
        // {
        //     StartCoroutine(RockAbility());
        //     scareAttack = false
        // }
        if (burning && !takingDamage) 
            StartCoroutine(BurnDoT(health, 3));
        if (baseState == BaseState.Frozen) 
            state = State.Waiting;

        switch(state)
        {
            case State.Idle:
                if (dist < startDistance && baseState != BaseState.Frozen)
                {
                    if (!counted)
                    {
                        playerScript.numberOfEnemies += 1;
                        counted = true;
                    }
                    if (scareAttack)  
                    {
                        StartCoroutine(RockAbility());
                        scareAttack = false;
                    } 
                    else
                        if (!starting) StartCoroutine(Starting());

                    rgbd.useGravity = true;
                }
                if (!waitToIdle) transform.position = new Vector3(transform.position.x, yPos + Mathf.Sin(Time.time * 3), transform.position.z);
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
                if (Vector3.Distance(transform.position, targetPos) > .5f && moving)
                {
                    if (!playingLaunchAttack)
                    {
                        animator.SetTrigger("Jump");
                        playingLaunchAttack = true;
                    }
                    transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref speed, Time.deltaTime * 25);
                }
                //     // transform.position = Vector3.Lerp(transform.position, targetPos, .1f);
                else 
                    moving = false;
            break;

            case State.Waiting:
                if (baseState != BaseState.Frozen)
                    state = State.Chasing;
            break;

        }
    }

    internal void PlaySFX(string name)
    {
        AudioManager.i.Play(name, transform.position);
    }

    IEnumerator Starting()
    {
        starting = true;
        exclamationMark.enabled = true;
        float t = 0;
        while (t < .5f)
        {
            t += Time.deltaTime;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            yield return new WaitForSeconds(.05f);
        }
        state = State.Chasing;
        exclamationMark.enabled = false;
        starting = false;
    }

    IEnumerator JumpAttack()
    {
        targetPos = transform.position + (Vector3.up * 7);

        rgbd.useGravity = false;
        jumping = true;
        state = State.Jumping;
        moving = true;
        yield return new WaitForSeconds(3f);
        playingLaunchAttack = false;
        attacking = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        transform.rotation *= Quaternion.Euler(90, 0, 0);
        rgbd.AddForce((player.transform.position - transform.position) * 320, ForceMode.Impulse);
        rgbd.useGravity = true;
        yield return new WaitForSeconds(2f);
        attacking = false;
        state = State.Chasing;
        // yield return new WaitForSeconds(Random.Range(1f, 2.5f));
        yield return new WaitForSeconds(6f);

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
        yield return new WaitForSeconds(4f);
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
        // else if (col.gameObject.CompareTag("Ground") && jumping)
        // {
        //     StartCoroutine(LookAtPlayer());
        //     // rgbd.velocity = Vector3.zero;
        // }
        else if (col.gameObject.CompareTag("Ground") && state == State.Jumping && !breaking)
        {
            StartCoroutine(Break(col.GetContact(0).point));
            // StartCoroutine(LookAtPlayer());

            // rgbd.velocity = Vector3.zero;
        }
        
        if (attacking && (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Player")))
        {
            // delete later ~
            rgbd.velocity = Vector3.zero;
            animator.SetTrigger("GetUp");
            AudioManager.i.Play("GargLand", transform.position);
            attacking = false;
        }

    }

    IEnumerator Break(Vector3 point)
    {
        breaking = true;
        breakGroundPs.transform.parent = null;
        breakGroundPs.transform.position = point;
        breakGroundPs.transform.rotation = Quaternion.identity;
        breakGroundPs.Play();
        yield return new WaitForSeconds(3.1f);
        breakGroundPs.transform.parent = transform;
        breaking = false;

    }

    internal bool GetClawing() => clawAttacking;
    internal void SetClawing(int number)
    {
        if (number == 0)
            clawAttacking = false;
        else if (number == 1)
            clawAttacking = true;
    }
}
