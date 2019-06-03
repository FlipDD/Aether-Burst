using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JorgeController : EnemyBase
{
    #region Variables
    private enum State {Walking, Attacking, ChoosingWall, Climbing, Eating, Summoning, Underground, Changing, Shooting}
    private State state;

    [SerializeField] private List<GameObject> skulls;

    [SerializeField]
    private Transform player;
    private PlayerInputController playerScript;
    [SerializeField]
    private Transform walls;
    [SerializeField]
    private List<Transform> crystals;
    private int currentCrystal;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;

    private Animator anim;
    private Animator childAnim;
    private Quaternion rot;
    private Vector3 dir;
    private Vector3 nDir;
    private Vector3 dirr;
    private Vector3 wallPos;
    private Vector3 ogPos;
    private float angle;
    private float dist;
    private float height;
    private bool walking, isClose, looking, goingDown, eating, turned, choosing, rotating, goingUp, shooting;

    [SerializeField] private Transform crystalPrefab;
    private Transform[] crystalsToDrop;
    private int count = 0;

    [SerializeField] private Transform defaultTerrain, northTerrain, southTerrain;
    private FadeInOut fade;
    [SerializeField] Transform tailAttackPoint;
    [SerializeField] Transform poisonObj;
    [SerializeField] Transform poisonCloud;
    private Transform[] poisonClouds;
    private ParticleSystem[] poisonCloudEffect;
    private int poisonCount = 0;
    private ParticleSystem jumpEffect;
    private Vector3 jumpEffectPosition;
    private ParticleSystem diggingEffect;
    private Vector3 diggingEffectPosition;

    internal bool firstPhase;
    internal bool landedNorth, crossedToNorth;
    internal bool landedSouth, crossedToSouth;

    [SerializeField] private GameObject northWall;
    [SerializeField] private GameObject southWall;    

    private Transform attackPoint;
    private Vector3 firstJumpPosition;

    [SerializeField] Transform tailPoint1, tailPoint2;
    [SerializeField] Transform bossPoint1, bossPoint2;

    private bool clawing;


    [SerializeField] Transform clawPs;
    [SerializeField] Transform clawPos;

    [SerializeField] Image logo;

    [SerializeField] Light light1, light2;

    [SerializeField] Transform crystalBroken;

    float timerMovement;
    #endregion

    void Awake()
    {
        crystalsToDrop = new Transform[400];
        for (int i = 0; i < crystalsToDrop.Length; i++)
        {
            crystalsToDrop[i] = Instantiate(crystalPrefab, transform.position, Quaternion.identity);
            crystalsToDrop[i].gameObject.AddComponent<Rigidbody>();
            crystalsToDrop[i].gameObject.GetComponent<Rigidbody>().drag = 1f;
            crystalsToDrop[i].gameObject.SetActive(false);
            crystalsToDrop[i].GetComponent<BoxCollider>().isTrigger = false;
        }
        poisonClouds = new Transform[10];
        poisonCloudEffect = new ParticleSystem[10];
        for (int i = 0; i < poisonClouds.Length; i++)
        {
            poisonClouds[i] = Instantiate(poisonCloud, transform.position, Quaternion.identity);
            poisonClouds[i].gameObject.SetActive(false);
            poisonCloudEffect[i] = poisonClouds[i].GetComponent<ParticleSystem>();
        }
        fade = FindObjectOfType<FadeInOut>();
    }

    void Start()
    {
        if (player == null)
            player = GameObject.Find("Player").transform;

        attackPoint = GameObject.Find("AtkPoint").transform;
        jumpEffect = GameObject.Find("BossJump").GetComponent<ParticleSystem>();
        diggingEffect = GameObject.Find("DiggingPs").GetComponent<ParticleSystem>();
        playerScript = player.GetComponent<PlayerInputController>();
        anim = GetComponent<Animator>();
        childAnim = GetComponentInChildren<Animator>();
        anim.SetTrigger("Walking");
        anim.speed = 1.2f;
        state = State.Eating;
        wallPos = walls.position;
        firstPhase = true;
        AudioManager.i.Play("Explore2", transform.position);
    }

    void Update() 
    {
        if (Input.GetKeyDown("c"))
        {
            anim.speed = 1;
            anim.SetTrigger("Claw");
        }
        // CHEATS
        if (Input.GetKeyDown("p") && firstPhase) 
        {
            StopAllCoroutines();
            StartCoroutine(StartSecondPhase());
        }

        if (firstPhase)
        {
            switch (state)
            {
                case State.Walking:
                    Movement();
                break;

                case State.ChoosingWall:
                    ChooseWall();
                break;

                case State.Climbing:
                    if (!goingDown) 
                        ClimbUpWall();
                    else
                        ClimbDownWall();
                break;

                case State.Summoning:
                    SummonWalls();
                break;

                case State.Eating:
                    //Do nothing
                break;
            }
        }
        else
        {
            switch (state)
            {
                case State.Underground:
                    if ((landedNorth && crossedToSouth) || (landedSouth && crossedToNorth))
                    {
                        northWall.SetActive(false);
                        southWall.SetActive(false);
                        StartCoroutine(LeaveUnderground());
                    }
                break;

                case State.Shooting:

                    if (!shooting)
                    {
                        if (poisonCount < 9)
                        {
                            StartCoroutine(SimulateProjectile(30));
                            poisonCount++;
                        }
                        else
                        {
                            poisonCount = 0;
                            landedNorth = false;
                            landedSouth = false;
                            StartCoroutine(JumpOver());
                        }
                    }
                break;

                case State.Changing:
                    // just changing
                break;

                default:
                break;
            }
        }
    }
    
    #region 2ndPhase
    internal IEnumerator StartSecondPhase()
    {
        firstPhase = false;
        state = State.Changing;
        anim.SetTrigger("Stopping");
        StartCoroutine(SimulateJump(false, 90, 0, 60, 2.25f));
        yield return new WaitForSeconds(1f);
        List<Transform> crystalsBroken = new List<Transform>();
        foreach (Transform crystal in crystals)
        {
            crystalsBroken.Add(Instantiate(crystalBroken, crystal.position, Quaternion.identity));
            Destroy(crystal.gameObject);
            // Rigidbody c = crystal.gameObject.GetComponent<Rigidbody>();
            // c.isKinematic = false;
            // c.AddForce(new Vector3(Random.Range(-1000, 1000f), -50, Random.Range(-1000, 1000f)));
        }
        yield return new WaitForSeconds(3);

        // foreach (Transform crystal in crystals)
        //     crystal.gameObject.SetActive(false);

        fade.FadeIn(false);
        yield return new WaitForSeconds(2.7f);
        foreach (Transform crystal in crystalsBroken)
        {
            int r = Random.Range(0, 5);
            if (r != 3)
                crystal.gameObject.SetActive(false);
        }
        defaultTerrain.gameObject.SetActive(false);
        northTerrain.gameObject.SetActive(true);
        southTerrain.gameObject.SetActive(true);
        northTerrain.position = new Vector3(northTerrain.position.x, 26, northTerrain.position.z);
        player.position = new Vector3(-50, 1.5f, 0);
        // transform.position = new Vector3(-131, 1.94f, transform.position.z);
        transform.position = new Vector3(tailPoint2.position.x - 20, tailPoint2.position.y + 26, tailPoint2.position.z);
        transform.LookAt(player);
        // transform.position += Vector3.up * 26;
        StartCoroutine(playerScript.CameraShaker2(1, .5f, .5f));
        fade.FadeOut();
        foreach (GameObject skull in skulls)
            Destroy(skull);
        yield return new WaitForSeconds(2);
        StartCoroutine(JumpOver()); 
    }

    IEnumerator JumpOver()
    {
        state = State.Changing;
        landedNorth = false;
        landedSouth = false;
        crossedToNorth = false;
        crossedToSouth = false;
        StartCoroutine(SimulateJump(false, 30, 0, 50, 2f));
        yield return new WaitForSeconds(3);
        // if (diggingEffect != null)
        // {
            diggingEffect.transform.parent = null;
            diggingEffect.Play();
        // }
        float t = 0;
        while (t < 5)
        {
            t += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime*2), transform.position.z);
            yield return null;
        }
        // Tail attack setup here
        if (Vector3.Distance(transform.position, tailPoint1.position) < Vector3.Distance(transform.position, tailPoint2.position))
        {
            bossPoint1.gameObject.SetActive(true);
            transform.position = tailPoint1.position - (Vector3.up * 30);
        }
        else
        {
            bossPoint2.gameObject.SetActive(true);
            transform.position = tailPoint2.position - (Vector3.up * 30);
            // transform.rotation = Quaternion.LookRotation(transform.position - tailPoint2.position);
        }

        yield return new WaitForSeconds(2);
        if (landedNorth)
            southWall.SetActive(true);
        else if (landedSouth)
            northWall.SetActive(true);
        diggingEffect.transform.parent = attackPoint;
        diggingEffect.transform.position = Vector3.zero;
        state = State.Underground;
    }

    IEnumerator LeaveUnderground()
    {
        bossPoint1.gameObject.SetActive(false);
        bossPoint2.gameObject.SetActive(false);
        state = State.Changing;
        diggingEffect.transform.parent = null;
        diggingEffect.transform.position = new Vector3(attackPoint.position.x, 0.5f, attackPoint.position.z);
        diggingEffect.Play();
        float t = 0;
        float height = transform.position.y;
        StartCoroutine(playerScript.CameraShaker2(8, 0.1f, 0.1f));
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + (Time.deltaTime*20), transform.position.z);
            yield return null;
        }
        yield return new WaitForSeconds(1); 
        float tt = 0;
        Vector3 northOriginalPos = northTerrain.position;
        Vector3 southOriginalPos = southTerrain.position;
        Vector3 jorgeOriginalPos = transform.position;
        while (tt < 5)
        {
            float random = Random.Range(-1f, 1f);
            tt += Time.deltaTime;
            if (landedNorth) northTerrain.position = new Vector3(northOriginalPos.x, tt * 6.5f, northOriginalPos.z + random);
            else if (landedSouth)  southTerrain.position = new Vector3(southOriginalPos.x, tt * 6.5f, southOriginalPos.z + random);
            transform.position = new Vector3(jorgeOriginalPos.x, tt * 6.5f, jorgeOriginalPos.z + random);
            yield return null;
        }
        transform.LookAt(new Vector3(player.position.x, 0, player.position.z));
        yield return new WaitForSeconds(1);
        if (landedNorth)
            StartCoroutine(SimulateJump(true, 45, 22, 80, 3f, true));
        else if (landedSouth)
            StartCoroutine(SimulateJump(false, 45, 22, 80, 3f, true));
        
    }

    IEnumerator SimulateProjectile(float firingAngle, float gravity = 120)
    {
        shooting = true;
        Transform poison = Instantiate(poisonObj, transform.position, Quaternion.identity);
        // random delay
        yield return new WaitForSeconds(Random.Range(.4f, .9f));
       
        poison.position = tailAttackPoint.position;
        Vector3 playerPosition = new Vector3(player.position.x, player.position.y - 2, player.position.z);
        float target_Distance = Vector3.Distance(poison.position, playerPosition);
        // hard formulaz
        float poisonVelocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / (gravity));
        float vx = Mathf.Sqrt(poisonVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float vy = Mathf.Sqrt(poisonVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // time flying
        float flightDuration = target_Distance / vx;
   
        poison.rotation = Quaternion.LookRotation(player.position - poison.position);
        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            poison.Translate(0, (vy - (gravity * elapse_time)) * Time.deltaTime, vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }
        
        poisonClouds[poisonCount].position = poison.position;
        poisonClouds[poisonCount].GetChild(1).GetComponent<SphereCollider>().enabled = true;
        poisonClouds[poisonCount].gameObject.SetActive(true);
        poisonCloudEffect[poisonCount].Play();

        poison.gameObject.SetActive(false);

        poisonCount++;
        shooting = false;
    }  

    IEnumerator SimulateJump(bool isNorthTerrain, float firingAngle, float height, float gravity, float velocityModifier, bool doubleJump = false)
    {
        if (doubleJump) 
        {
            Vector3 dir = (attackPoint.position - transform.position).normalized;
            firstJumpPosition = new Vector3(attackPoint.position.x - (dir.x*15), attackPoint.position.y, attackPoint.position.z);
        }
        yield return new WaitForSeconds(.8f);
        AudioManager.i.Play("BossJump", transform.position);
        yield return new WaitForSeconds(.2f);
        for (int i = 0 + count; i < 5 + count; i++)
        {
            crystalsToDrop[i].position = transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(30f, 35f), Random.Range(-5f, 5f));
            crystalsToDrop[i].gameObject.SetActive(true);
        }
        count += 5; 
        anim.SetTrigger("Stopping");
        jumpEffect.transform.parent = attackPoint;
        jumpEffect.transform.localRotation = Quaternion.identity;
        jumpEffect.transform.localPosition = Vector3.zero;
        Vector3 playerPosition = new Vector3(player.position.x, height, player.position.z);
        float target_Distance = Vector3.Distance(attackPoint.position, playerPosition);
        // hard formulaz
        float poisonVelocity = target_Distance / (Mathf.Sin(2.25f * firingAngle * Mathf.Deg2Rad) / (gravity));
        float vx = Mathf.Sqrt(poisonVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float vy = Mathf.Sqrt(poisonVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        float flightDuration = target_Distance / vx;

        transform.LookAt(new Vector3(playerPosition.x, playerPosition.y, playerPosition.z), Vector3.up);
        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (vy - (gravity * elapse_time)) * Time.deltaTime, vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            if (elapse_time > .5f)
            {
                if (Physics.Raycast(attackPoint.position, -attackPoint.transform.up, 1f, playerLayer)) 
                    elapse_time = flightDuration;

                Collider[] col = Physics.OverlapSphere(jumpEffect.transform.position, 1, playerLayer);
                if (col.Length > 0) 
                    elapse_time = flightDuration;
            }
            yield return null;
        }
        AudioManager.i.Play("BossLand", transform.position);
        Collider[] cols = Physics.OverlapSphere(jumpEffect.transform.position, 8, playerLayer);
        foreach (Collider cc in cols)
        {
            if (cc.CompareTag("Player"))
            {
                cc.GetComponent<UIBar>().UpdateHp(-33);
                cc.GetComponent<Rigidbody>().AddExplosionForce(100000, jumpEffect.transform.position, 15, 5f);
            }
            else if (cc.name.Contains("South"))
            {
                southTerrain.position = new Vector3(southTerrain.position.x, 0, southTerrain.position.z);
                landedSouth = true;
            }
            else if (cc.name.Contains("North"))
            {
                northTerrain.position = new Vector3(northTerrain.position.x, 0, northTerrain.position.z);
                landedNorth = true;
            }
        }
        if (light1 != null)
        {
            light1.intensity -= 2;
            light2.intensity -= 2;
        }
        else if (light1.intensity < .5f)
        {
            Destroy(light1.transform.parent.gameObject);
            Destroy(light2.transform.parent.gameObject);
            // light1.transform.parent.gameObject.AddComponent<Rigidbody>();
            // light2.transform.parent.gameObject.AddComponent<Rigidbody>();
        }
        StartCoroutine(playerScript.CameraShaker2(0.5f, 1, 1));
        jumpEffect.transform.parent = null;
        jumpEffect.transform.rotation = Quaternion.identity;
        jumpEffect.transform.position = attackPoint.position;
        jumpEffect.Play();
        yield return new WaitForSeconds(.1f);
        if (doubleJump)
            StartCoroutine(JumpBack());
    }  

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward); 
    }

    IEnumerator JumpBack()
    {
        AudioManager.i.Play("BossJump", transform.position);

        transform.LookAt(firstJumpPosition);
        for (int i = 0 + count; i < 5 + count; i++)
        {
            crystalsToDrop[i].position = transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(30f, 35f), Random.Range(-5f, 5f));
            crystalsToDrop[i].gameObject.SetActive(true);
        }
        count += 5; 
        anim.SetTrigger("Stopping");
        float distance = Vector3.Distance(attackPoint.position, firstJumpPosition);
        float poisonVelocity = distance / (Mathf.Sin(2f * 45 * Mathf.Deg2Rad) / (80));
        float vx = Mathf.Sqrt(poisonVelocity) * Mathf.Cos(45 * Mathf.Deg2Rad);
        float vy = Mathf.Sqrt(poisonVelocity) * Mathf.Sin(45 * Mathf.Deg2Rad);
        float flightDuration = distance / vx;
        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (vy - (80 * elapse_time)) * Time.deltaTime, vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }
        AudioManager.i.Play("BossLand", transform.position);
        if (light1 != null)
        {
            light1.intensity -= 2;
            light2.intensity -= 2;
        }
        Transform jumpTf = jumpEffect.transform;
        jumpTf.parent = attackPoint;
        jumpTf.localRotation = Quaternion.identity;
        jumpTf.localPosition = Vector3.zero;
        jumpTf.parent = null;
        jumpTf.rotation = Quaternion.identity;
        jumpEffect.Play();
        yield return new WaitForSeconds(4);
        poisonCount = 0;
        state = State.Shooting;
    }
    #endregion
        
    IEnumerator BossLoop()
    {
        choosing = true;
        yield return new WaitForSeconds(12);

        // PUT CLAW ATTACK HERE, MAYBE POISON TOO?
        anim.speed = 1;
        anim.SetTrigger("Walking");
        state = State.ChoosingWall;
        choosing = false;
    }


    #region Movement
    void Movement()
    {
        timerMovement += Time.deltaTime;
        bool isClawing = anim.GetCurrentAnimatorStateInfo(0).IsName("ClawAttackDouble");
        if (!isClawing) anim.SetTrigger("Walking");
        if (!choosing) StartCoroutine(BossLoop());
        if (!isClose) StartCoroutine(WaitToCheck());
        dir = (player.position - transform.position).normalized;
        nDir = new Vector3(dir.x, 0, dir.z);
        rot = Quaternion.LookRotation(nDir);
        angle = Vector3.Angle(transform.forward, new Vector3(dir.x, transform.forward.y, dir.z));
        
        if (angle < 13 && !isClawing)
        {
            if (dist > 20)
            {
                transform.position += transform.forward * 15 * Time.deltaTime;
                anim.speed = 1.2f;
            }
            else if (dist > 16)
            {
                transform.position += transform.forward * 9 * Time.deltaTime;
                anim.speed = .85f;
            }
            else if  (dist > 13)
            {
                transform.position += transform.forward * 5 * Time.deltaTime;
                anim.speed = .5f;
            }
            else if (dist > 11)
            {
                if (!clawing) StartCoroutine(ClawAttack());
            }

            if (dist > 30 && timerMovement > 5) 
            {
                if (!shooting)
                    {
                        if (poisonCount < 9)
                        {
                            StartCoroutine(SimulateProjectile(30));
                            poisonCount++;
                        }
                        else
                            poisonCount = 0;

                        
                    }
            }
            else{
                poisonCount = 0;
            }



            if (!looking && !isClawing) StartCoroutine(LookAtRotation(10));
            if (!walking) 
            {
                anim.speed = 1.2f;
                walking = true;
            }
        }
        else if(!isClawing)
        {
            if (!looking) StartCoroutine(LookAtRotation(5));
            if (walking) 
            {
                anim.speed = .4f;
                walking = false;
            }
        }
    }

    void ClawEffect()
    {
        Instantiate(clawPs, clawPos.position, Quaternion.identity);
    }

    IEnumerator ClawAttack()
    {
        clawing = true;
        anim.speed = 1;
        anim.SetTrigger("Claw");
        yield return new WaitForSeconds(1);
        AudioManager.i.Play("BossClaw", transform.position);
        yield return new WaitForSeconds(3);
        clawing = false;
    }

    IEnumerator LookAtRotation(float rate)
    {
        looking = true;
        transform.rotation = Quaternion.Lerp (transform.rotation, rot, Time.deltaTime * rate);
        yield return new WaitForFixedUpdate();
        looking = false;
    }

    IEnumerator WaitToCheck()
    {
        isClose = true;
        dist = Vector3.Distance(player.position, transform.position);
        yield return new WaitForSeconds(.2f);
        isClose = false;
    }
    #endregion

    #region WallClimbing
    void ChooseWall()
    {
        if (!rotating) 
        {
            currentCrystal = Random.Range(0, crystals.Count);
            rotating = true;
            anim.speed = .4f;
        }
        dir = (crystals[currentCrystal].position - transform.position).normalized;
        nDir = new Vector3(dir.x, 0, dir.z);
        rot = Quaternion.LookRotation(nDir);

        if (!looking) StartCoroutine(LookAtRotation(2));
        if (Mathf.Abs(Vector3.Angle(transform.forward, nDir)) < 1)
        {
            rotating = false;
            state = State.Climbing;
            anim.speed = 1.2f;
        }
    }

    void ClimbUpWall()
    {
        dirr = (crystals[currentCrystal].position - transform.position).normalized;

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(crystals[currentCrystal].position.x, crystals[currentCrystal].position.z)) < 9f)
        {
            anim.speed = 0.25f;
            rot = Quaternion.LookRotation(dirr);
            StartCoroutine(LookAtRotation(1));
            transform.position += new Vector3(dirr.x, 0f, dirr.z) * 20 * Time.deltaTime;
            if (Vector3.Angle(transform.forward, dirr) < 12f)
            {
                anim.speed = .5f;
                transform.position += dirr * 8 * Time.deltaTime;
            }
            
            if (Vector3.Distance(crystals[currentCrystal].position, transform.position) < 7)
                StartCoroutine(EatCrystal());
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(dirr.x, 0, dirr.z));
            transform.position += new Vector3(dirr.x, transform.forward.y, dirr.z) * 15 * Time.deltaTime;
        }
    }

    void ClimbDownWall()
    {
        dirr = (player.position - transform.position).normalized;
        rot = Quaternion.LookRotation(dirr);
        if (transform.position.y < -1.5f)
        {
            anim.speed = 1.2f;
            state = State.Walking;
            goingDown = false;
        }
        else
        {
            anim.speed = .5f;
            transform.position += Vector3.up * -8 * Time.deltaTime;
        }
    }
    
    IEnumerator EatCrystal() //lol
    {
        eating = true;
        state = State.Eating;
        anim.speed = 0.3f;
        yield return new WaitForSeconds(1);
        AudioManager.i.Play("BossEat", transform.position);
        yield return new WaitForSeconds(1.5f);
        for (int i = 0 + count; i < 5 + count; i++)
        {
            crystalsToDrop[i].position = crystals[currentCrystal].position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            crystalsToDrop[i].gameObject.SetActive(true);
        }
        count += 5;
        anim.speed = -1;
        HealthComponentBoss hp = GetComponent<HealthComponentBoss>();
        hp.health += (int) hp.slider.maxValue / 10;
        hp.slider.value = hp.health;    
        Destroy(crystals[currentCrystal].gameObject);
        crystals.RemoveAt(currentCrystal);
        goingDown = true;
        state = State.Climbing;
        eating = false;
    }

    IEnumerator TurnAround()
    {
        while (!turned)
        {
            transform.rotation = Quaternion.Lerp (transform.rotation, rot, Time.deltaTime * 15);
            if (Mathf.Abs(Quaternion.Angle(transform.rotation, rot)) < 1)
                turned = true;
            
            yield return new WaitForFixedUpdate();
        }
        turned = false;
    }
    #endregion

    #region SummonWalls
    void SummonWalls()
    {
        if (!goingUp)
        {
            if (walls.position.y < -3)
            {
                height += .3f;
                walls.position = new Vector3(ogPos.x + Random.Range(-.2f, .2f), 
                ogPos.y + height, ogPos.z);
            }
            else
            {
                walls.position = new Vector3(walls.position.x, -2, walls.position.z);
            }
        }
        else
        {
            if (walls.position.y > -75)
            {
                height += .3f;
                walls.position = new Vector3(ogPos.x + Random.Range(-.2f, .2f), ogPos.y - height, ogPos.z);
            }
            else
            {
                walls.position = new Vector3(walls.position.x, -76, walls.position.z);
            }
        }
    }
    #endregion

    internal IEnumerator WinScreen()
    {
        state = State.Changing;
        StartCoroutine(playerScript.CameraShaker3(1, 1, 1));
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        fade.FadeIn(false);
        yield return new WaitForSeconds(2);
        float t = 5f;
        while (t > 0)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, Mathf.Lerp(logo.color.a, 1, Time.deltaTime/2.5f));
            yield return null;
        }
        SceneManager.LoadSceneAsync("EntranceSection");
        yield return new WaitForSeconds(3);
        // float tt = 3f;
        // while (tt > 0)
        // {
        //     logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, Mathf.Lerp(logo.color.a, 0, Time.deltaTime ));
        //     yield return null;
        // }
        // yield return new WaitForSeconds(1);

    }

    internal void SetState() 
    {
        state = State.Walking;
        AudioManager.i.Play("Boss", transform.position);
        AudioManager.i.Stop("Explore2");
    }
}
