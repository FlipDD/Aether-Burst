using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(UIBar))]
public class PlayerInputController : MonoBehaviour 
{
    #region Variables
	private const string Horizontal = "Horizontal";
	private const string Vertical = "Vertical";
	private const string Jump = "Jump";
	private const string LeftClick = "Fire1";
	private const string RightClick = "Fire2";
    private const string Tab = "Tab";

	private Color fireColor = new Color(1, 0.7f, 0.25f);
	private Color waterColor = new Color(0.7f, 1, 0.97f);
	private Color airColor = new Color(.9f, 1, 1);
    Vector3 lookenemy;

    [HideInInspector]
	public LayerMask whatIsGround;
	public Camera cam;
	public TMPro.TMP_Text aetherText;

    #region Elements
    public GameObject fireEffect;
	public GameObject waterEffect;
	public GameObject airEffect;
	public GameObject groundEffect;
	public GameObject menu;
	public Image waterImg;
	public Image airImg;
	public Image fireImg;
	public Image groundImg;
	//AIR 0
	//WATER 1
	//FIRE 2
	//GROUND 3
	float airCd;
	float dashCd;

	internal GameObject currentAbility;
	internal GameObject selectedAbility;
	#endregion

	private UIBar uiBar;
    internal bool bling;
    internal float distancetoadd;
    private float combotimer;
    internal bool playshaker;
	private PlayerMovement playerMovement;
	private Fireball fireball;
	internal Vector3 movement;
	private Rigidbody rbdg;
    private bool calculated;
    internal float verticalVelocity;
	public bool isGrounded;
	public bool hasJumped;
    private float attacktimer;
    private bool aoeAttack;
    public bool ischarging;
    private bool buttonhelddown;
    private float normalattacktimer;
    public bool isfinalcharged;
    private bool finalchargeattack;

    private bool combodone;
    private bool combopossible;
    private float attackcooldown;
	private Vector3 tmpVelocity;
	Vector3 pos;
	float angleRadians;
	float angleDegrees;
    public Transform enemy;
    public bool targetlock;
	private int aetherEssence;
    public LayerMask whatIsEnemies;

    bool rightMouseUp;
	
    private State state;
    internal bool inWall;
    private bool targetanalyse;
    public bool enablesprite;
    float dist2;
    private GameObject sword;
	Vector3 tmpInput;
	internal bool pressedJump;
    private bool changingLight;

	public Transform camBase;
    public bool noattack;
    private GameObject boulder;
    private GameObject boulder2;
    private GameObject boulder3;
    private bool bouldershake;
    private int i;
    internal bool enemystop;
    public Transform bouldertrigger;

    [SerializeField]
    private ParticleSystem aetherBurst;
    private GameObject fireBurst;
    private GameObject airBurst;
    private GameObject waterBurst;

    public GameObject sphereCollider;
    float burstTime = -1;

    bool godMode;

    internal Animator animator;
    internal float magnitude;
    private bool stopping;
    private int nOfCombo;
    private bool canClick, stopMovement;
    private bool comboCooldown;
    internal bool isAttacking, playingFighting, canPlayFighting;
    private int musicNumber;
    internal int numberOfEnemies;
    Transform hips;
    ParticleSystem swordTrail;
    AudioManager audioManager;

    public enum State
	{
		Idle,
		Talking,
		ChangingElements,
		Moving,
		Charging,
		Attacking
	}
    #endregion

    #region Start

    // void Awake()
    // {
    //     audioManager = FindObjectOfType<AudioManager>();
    //     for (int i = 0; i < audioManager.sounds.Length; i++)
    //     {
    //         AudioManager.i.Stop(audioManager.sounds[i].name);
    //     }
    // }
	void Start () 
	{
        numberOfEnemies = 0;
        // Audio
        AudioManager.i.StopAll();
        if (SceneManager.GetActiveScene().name == "HoleSection")
        {
            AudioManager.i.PlayBackground("Explore2");
            musicNumber = 2;
        }
        else if (SceneManager.GetActiveScene().name == "BossSection")
        {
            AudioManager.i.PlayBackground("Explore1");
            musicNumber = 1;
        }
        else if (SceneManager.GetActiveScene().name == "OpenSpaceSection")
        {
            AudioManager.i.PlayBackground("Explore3");
            musicNumber = 3;

        }
        //TESTING PURPOSES//
        i = 0;
        aetherEssence = 10;
        sword = GameObject.Find("ari sword");
        
        ///////////////////
        normalattacktimer = 2.0f;
        targetlock = false;
		hasJumped = true;
        combopossible = false;
        combodone = false;
        boulder = GameObject.FindGameObjectWithTag("Boulder");
        boulder2 = GameObject.FindGameObjectWithTag("SecretBoulder");
        boulder3 = GameObject.FindGameObjectWithTag("SecretBoulder2");
        rbdg = GetComponent<Rigidbody>();
		playerMovement = GetComponent<PlayerMovement>();
        // combotimer = 0f;
        attacktimer = 3.0f;
        // attackcooldown = 1.0f;

		//ELEMENTS//
		fireball = fireEffect.GetComponent<Fireball>();
		// fireEffect.SetActive(false);
		fireEffect.SetActive(false);
		waterEffect.SetActive(false);
		groundEffect.SetActive(false);
		currentAbility = airEffect;
		//////////
		uiBar = GetComponent<UIBar>();

		aetherText.text = "" + aetherEssence;
		state = State.Moving;

		Time.timeScale = 1;
		Time.fixedDeltaTime = .02f;

		fireball.isCharging = false;
        if (aetherBurst == null)
        {
            aetherBurst = GameObject.Find("AetherBurst").GetComponent<ParticleSystem>();
        }
        airBurst = GameObject.Find("Air");
        fireBurst = GameObject.Find("Fire");
        waterBurst = GameObject.Find("Water");
        airBurst.SetActive(false);
        fireBurst.SetActive(false);
        waterBurst.SetActive(false);

        sphereCollider.SetActive(false);

        animator = GetComponent<Animator>();

        nOfCombo = 0;
        canClick = true;

        hips = GameObject.Find("mixamorig:Hips").transform;
        swordTrail = GameObject.Find("Trails").GetComponent<ParticleSystem>();

        audioManager = FindObjectOfType<AudioManager>();

	}
    #endregion

	void FixedUpdate ()
	{
        if (state == State.Moving)
			playerMovement.Move(movement.normalized);
        
        if (!uiBar.isWaiting)
        {
            if (!uiBar.isChanging)
                uiBar.DecreaseBreath(-.5f);
            else
                uiBar.DecreaseBreath(-.15f);
        }
	}

	void Update () 
	{
        HandleFightingTrack();
        Debug.Log(numberOfEnemies);
        //GODMODE
        if (Input.GetKeyDown("l") && !godMode)
            godMode = true;
        else if(Input.GetKeyDown("l") && godMode)
            godMode = false;

        if (godMode)
        {
            uiBar.UpdateHp(100);
            uiBar.DecreaseBreath(-10);
            UpdateAether(1);
        }

        if (!calculated)
            StartCoroutine(WaitToUpdate());

		//Cooldowns
		if (airCd > 0) airCd -= Time.deltaTime;
		if (dashCd > 0) dashCd -= Time.deltaTime;
		
		if (Input.GetButtonUp("MouseWheel") && menu.activeInHierarchy)
		{
			menu.SetActive(false);
			Time.timeScale = 1;
			Time.fixedDeltaTime = .02f;
			Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = true;
			SetElement(selectedAbility);
			SetState(State.Moving);	
			fireball.isCharging = false;
            aetherBurst.Play();
            StartCoroutine(IncreaseSphereCollider());
		}
		if (Input.GetButtonDown("MouseWheel") && burstTime == -1)
		{
			menu.SetActive(true);
			GetComponentInChildren<Projector>().enabled = false;
			Time.timeScale = .05f;
			Time.fixedDeltaTime = Time.timeScale * .05f;
			movement /= 10;
			pos = Input.mousePosition;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = false;
			SetState(State.ChangingElements);
		} 

		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();

		if (Input.GetMouseButtonUp(1))
			rightMouseUp = true;
		if (Input.GetMouseButtonDown(1))
			rightMouseUp = false;

        if(Input.GetButtonDown(Tab))
        {
            targetlock = !targetlock;
            targetanalyse = true;
        }

        HandleTarget();

        switch (state)
		{
			case State.Moving:
            
                if(Input.GetKey(KeyCode.Backspace) && bouldertrigger != null)
                    transform.position = bouldertrigger.position;

				movement = new Vector3(Input.GetAxis(Horizontal), 0f, Input.GetAxis(Vertical)).normalized;
                animator.SetFloat("Vx", movement.x);
                animator.SetFloat("Vy", movement.z);
                // magnitude = Mathf.Lerp(magnitude, movement.sqrMagnitude, Time.deltaTime * 2);
                Vector3 v = new Vector3(rbdg.velocity.x, 0f, rbdg.velocity.z);
                magnitude = Mathf.Lerp(magnitude, v.sqrMagnitude/70, Time.deltaTime * 7);
                if (magnitude > 1) magnitude = 1;
                    animator.SetFloat("Velocity", magnitude);
				
				if (Input.GetButtonDown(RightClick) && !uiBar.isChanging) 
					HandleAbilities();

				if (Input.GetButtonUp(RightClick) && currentAbility == waterEffect)
				{
					GetComponentInChildren<Projector>().enabled = false;
					StartCoroutine(waterEffect.GetComponent<WaterElement>().CastWaterPrison());
                    animator.SetTrigger("WaterStun");
					StartCoroutine(Cast(2.5f, 10f));
					UpdateAether(-1);
				}

				//No mouse wheel button?
				if (Input.GetKey("1"))
					SetElement(airEffect);
				if (Input.GetKey("2"))
					SetElement(fireEffect);
				if (Input.GetKey("3"))
					SetElement(waterEffect);    

				if (Input.GetKeyDown("n"))
			        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
				else if (Input.GetKeyDown("m"))
					SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);

                //MOBILITY
				if (Input.GetButtonDown(Jump) && !uiBar.isChanging)  
				{
                    if (isGrounded)
                    {
                        if (magnitude > 0.05)
                            animator.SetTrigger("RunningJump");
                        else
                            animator.SetTrigger("Jump");

                        hasJumped = true;
                    } 
                    else 
                    {
                        pressedJump = true;
                        StartCoroutine(WaitSomeFrames());
                    }
                }

				if (Input.GetButton("LeftShift") && !uiBar.isChanging)  
				{
					if (uiBar.GetBreathValue() > 17 && dashCd <= 0)
		            {
                        if (!targetlock || tmpInput.z < -.9f) 
                        {
                            if (isGrounded) animator.SetTrigger("RunningDash");
                            else animator.SetTrigger("FallingDash");
                        }   
                        else 
                        {
                            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
                            playerMovement.Dash((transform.forward + dirToEnemy).normalized); 
                        }

					    dashCd = 1.5f;
					}
				}
                tmpInput = movement;

                TargetsAndBoulders();

				movement.y = 0;

                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
				if (movement != Vector3.zero) 
                {
                    transform.rotation = Quaternion.LookRotation(movement);
                    if (isGrounded)
                        if (!currentState.IsName("AriRunning") && !targetlock) animator.SetBool("Running", true);
                        else if (targetlock && !currentState.IsName("TLTree")) animator.SetBool("TargetLocked", true);
                    else
                        animator.SetBool("Running", false);

                }
                else 
                   if (currentState.IsName("AriRunning") && !stopping)  animator.SetBool("Running", false);


                if (Input.GetButtonDown(Jump) && !isGrounded && hasJumped && uiBar.GetBreathValue() > 18 && !uiBar.isChanging && !playerMovement.isTouching)
                {
                    animator.SetTrigger("DoubleJump");
                    hasJumped = false;
                }
               
                break;

			case State.Idle:
                Time.fixedDeltaTime = .02f;
                Time.timeScale = 1f;
                animator.SetBool("Running", false);
                break;

			case State.Talking:
                // TargetsAndBoulders();
				break;
			case State.ChangingElements:
				ElementMenu();

				break;

			case State.Charging:
				movement = new Vector3(Input.GetAxisRaw(Horizontal), 0f, Input.GetAxisRaw(Vertical)).normalized/100;
                // transform.rotation = Quaternion.LookRotation(movement);
				break;

			default:
				break;
		}
	}

    internal void RunningDash() => playerMovement.Dash(transform.forward);

    internal void DoubleJ() => playerMovement.DoubleJump(new Vector3(movement.x, 1f, movement.z));

    void HandleFightingTrack()
    {
        if (!playingFighting)
        {
            if (numberOfEnemies > 0)
            {
                playingFighting = true;
                if (musicNumber == 1)
                {
                    AudioManager.i.PlayBackground("Fight1");
                    AudioManager.i.ChangeVolume("Explore1", .5f);
                }
                else if (musicNumber == 2)
                {
                    AudioManager.i.PlayBackground("Fight2");
                    AudioManager.i.ChangeVolume("Explore2", .5f);
                }
                else if (musicNumber == 3)
                {
                    AudioManager.i.PlayBackground("Fight3");
                    AudioManager.i.ChangeVolume("Explore3", .5f);

                }
            }
        }
        else
            if (numberOfEnemies < 1)
            {
                playingFighting = false;
                numberOfEnemies = 0;
                if (musicNumber == 1)
                {
                    AudioManager.i.Stop("Fight1");
                    AudioManager.i.ChangeVolume("Explore1", 2);
                }
                else if (musicNumber == 2)
                {
                    AudioManager.i.Stop("Fight2");
                    AudioManager.i.ChangeVolume("Explore2", 2);
                }
                else if (musicNumber == 3)
                {
                    AudioManager.i.Stop("Fight3");
                    AudioManager.i.ChangeVolume("Explore3", 2);
                }
            }
    }

    private void HandleChargeEffect(bool enabling)
    {
		if (enabling)
		{
			Light light = GetComponentInChildren<Light>();

			if (currentAbility == airEffect)
				sword.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
			else if (currentAbility == fireEffect)
				sword.transform.GetChild(4).GetComponent<ParticleSystem>().Play();
			else if (currentAbility == waterEffect)
				sword.transform.GetChild(5).GetComponent<ParticleSystem>().Play();
		}
		else
		{
			if (currentAbility == airEffect)
				StartCoroutine(WaitToStop(3));
			else if (currentAbility == fireEffect)
				StartCoroutine(WaitToStop(4));
			else if (currentAbility == waterEffect)
				StartCoroutine(WaitToStop(5));
		}
    }
	
	IEnumerator WaitToStop(int index)
	{
		uiBar.DecreaseBreath(15);
		UpdateAether(-1);
		yield return new WaitForSeconds(.4f);
		sword.transform.GetChild(index).GetComponent<ParticleSystem>().Stop();
	}

	internal void WindPush()
	{
		airCd = 1;
        UpdateAether(-1);
        rbdg.AddForce(transform.forward*-7500);
		Transform tf = Instantiate(GameAssets.i.windEffectShoot, transform.position, transform.rotation);
		// tf.GetComponent<ParticleSystem>().Play();
		Destroy(tf.gameObject, 6f);
		state = State.Moving;
        AudioManager.i.Play("AirPush", transform.position);
	}

	IEnumerator Cast (float castTime, float decreaseValue)
	{
		SetState(State.Charging);
		yield return new WaitForSeconds(castTime);
		uiBar.DecreaseBreath(decreaseValue);
		SetState(State.Moving);
        UpdateAether(-1);
	}

    // IEnumerator Cast (float castTime, float decreaseValue)
	// {
    //     float t = castTime;
    //     while(t > 0)
    //     {
    //         yield return new WaitForEndOfFrame();
    //         movement = Vector3.zero;
    //         t -= Time.deltaTime;
    //         yield return null;
    //     }
	// 	uiBar.DecreaseBreath(decreaseValue);
	// }

	void HandleAbilities ()
	{
		if (currentAbility == airEffect)
		{
			if (uiBar.GetBreathValue() > 20 && airCd <= 0 && aetherEssence > 0)
            {
				animator.SetTrigger("AirPush");
                StartCoroutine(Cast(1.5f, 10));
            }
			else if (!uiBar.isChanging)
				StartCoroutine(uiBar.ChangeColor(.1f, .15f, 20));
		} 
		else
		{
			if (aetherEssence > 0 && !uiBar.isChanging)
			{
				if (selectedAbility == fireEffect && fireball.readyToFire) 
				{
					animator.SetTrigger("FireBreath");
					StartCoroutine(Cast(1f, 10f));
				}
				else if (currentAbility == waterEffect)
				{
					transform.GetComponentInChildren<Projector>().enabled = true;
				}
			}	
		}
	}

    void FireBreath() => fireball.Shoot();

    IEnumerator WaitSomeFrames()
	{
		yield return new WaitForSeconds(.1f);
		pressedJump = false;
	}

    IEnumerator WaitToUpdate()
    {
        calculated = true;
        verticalVelocity = Mathf.Abs(rbdg.velocity.y);
        yield return new WaitForSeconds(.1f);
        calculated = false;
    }

	void SetElement(GameObject toActivate)
    {
		for (int i = 0; i < transform.GetChild(0).childCount; i++)
		{
			if (transform.GetChild(0).GetChild(i).gameObject.activeSelf == true)
				currentAbility = transform.GetChild(0).GetChild(i).gameObject;
		}
		HandleChargeEffect(false);
	   	if (currentAbility != null) currentAbility.SetActive(false);
		if (toActivate != null) toActivate.SetActive(true);
	   	currentAbility = toActivate;
    }
    #region ElementMenu
	void ElementMenu ()
	{
		if (uiBar.GetBreathValue() <= .5f)
		{
			menu.SetActive(false);
			Time.timeScale = 1;
			Time.fixedDeltaTime = .02f;
			Cursor.lockState = CursorLockMode.Locked;
			SetElement(selectedAbility);
			SetState(State.Moving);	
		}

		uiBar.DecreaseBreath(250f * Time.deltaTime);

		Vector3 angle = new Vector2(Input.GetAxisRaw("Horizontal2"), -Input.GetAxisRaw("Vertical2"));
		//Handle mouse or controller - in that order
		if (angle == Vector3.zero)
		{
			Vector2 dist = Input.mousePosition - pos;		
			angleRadians=Mathf.Atan2(dist.y, dist.x);
			angleDegrees = angleRadians * Mathf.Rad2Deg;
		} else
		{
			angleRadians=Mathf.Atan2(angle.y, angle.x);
		 	angleDegrees = angleRadians * Mathf.Rad2Deg;
		}

		if (angleDegrees<0) 
			angleDegrees+=360;

		if ((angleDegrees < 45 && angleDegrees > 0) || angleDegrees > 315) 
		{
			waterImg.color = new Color (waterImg.color.r, waterImg.color.g, waterImg.color.b, 1);
			selectedAbility = waterEffect;
            waterBurst.SetActive(true);
		}
		else
		{
			waterImg.color = new Color (waterImg.color.r, waterImg.color.g, waterImg.color.b, .7f);
            waterBurst.SetActive(false);

		}

		if (angleDegrees > 45 && angleDegrees <= 135)
		{
			airImg.color = new Color (airImg.color.r, airImg.color.g, airImg.color.b, 1);
			selectedAbility = airEffect;
            airBurst.SetActive(true);
		}
		else
        {
			airImg.color = new Color (airImg.color.r, airImg.color.g, airImg.color.b, .7f);
            airBurst.SetActive(false);
        }

		if (angleDegrees > 135 && angleDegrees <= 225)
		{
			fireImg.color = new Color (fireImg.color.r, fireImg.color.g, fireImg.color.b, 1);
			selectedAbility = fireEffect;
            fireBurst.SetActive(true);
		}
		else
        {
			fireImg.color = new Color (fireImg.color.r, fireImg.color.g, fireImg.color.b, .7f);
            fireBurst.SetActive(false);
        }

		if (angleDegrees > 225 && angleDegrees <= 315)
		{
			groundImg.color = new Color (groundImg.color.r, groundImg.color.g, groundImg.color.b, 1);
			selectedAbility = groundEffect;
		}
		else
			groundImg.color = new Color (groundImg.color.r, groundImg.color.g, groundImg.color.b, .7f);
	}
    #endregion

	internal void UpdateAether (int value)
	{
		aetherEssence += (value);
		if (aetherEssence < 0) aetherEssence = 0;
		aetherText.text = "" + aetherEssence;
	}

    internal void IncreaseBreath(float value) => uiBar.DecreaseBreath(-10);

    internal void IncreaseHealth(float value) => uiBar.UpdateHp(value);

    //setters
    internal State SetState(State newState)
	{
		state = newState;
		return state;
	}

	internal State SetIdle()
	{
		state = State.Idle;
		return state;
	}

    internal State SetCharging()
	{
		state = State.Charging;
		return state;
	}

	internal IEnumerator CameraShaker(float duration, float magnitudeX, float magnitudeY)
	{
        AudioManager.i.PlayBackground("Earthquake");
        bouldershake = false;
        state = State.Idle;
        Vector3 originalPos = camBase.transform.localPosition;
		float timer = 0;
		while (timer < duration)
		{
			float x = UnityEngine.Random.Range(-1f, 1f) * magnitudeX;
			float y = UnityEngine.Random.Range(1f, 1f) * magnitudeY;
			camBase.transform.localPosition	 = new Vector3(x, y, originalPos.z);
			timer += Time.deltaTime;
			yield return null;
		
		}
        state = State.Moving;
        camBase.transform.localPosition = originalPos;
    }

    internal IEnumerator CameraShaker2(float duration, float magnitudeX, float magnitudeY)
    {
        AudioManager.i.PlayBackground("Earthquake");
        bouldershake = false;
        state = State.Idle;
        camBase = transform.Find("CamFollow");
        Vector3 originalPos = cam.transform.localPosition;
        Vector3 ogPos = camBase.transform.localPosition;
        float timer = 0;
        while (timer < duration)
        {
            float x = UnityEngine.Random.Range(-.5f, .5f) * magnitudeX;
            float y = UnityEngine.Random.Range(-.5f, .5f) * magnitudeY;
            camBase.transform.localPosition = new Vector3(x, y, originalPos.z + 5);
            timer += Time.deltaTime;

            yield return null;
        }
        state = State.Moving;
        camBase.transform.position = transform.position;
    }

    internal IEnumerator CameraShaker3(float duration, float magnitudeX, float magnitudeY)
    {
        AudioManager.i.PlayBackground("Earthquake");
        bouldershake = false;
        state = State.Idle;
        camBase = transform.Find("CamFollow");
        Vector3 originalPos = cam.transform.localPosition;
        Vector3 ogPos = camBase.transform.localPosition;
        float timer = 0;
        while (timer < duration)
        {
            float x = UnityEngine.Random.Range(-.001f, .001f) * magnitudeX;
            float y = UnityEngine.Random.Range(-.001f, .001f) * magnitudeY;
            camBase.transform.localPosition = Vector3.Lerp(camBase.transform.localPosition, new Vector3(x, y, originalPos.z),1f*Time.deltaTime);
            timer += Time.deltaTime;

            yield return null;
        }
        state = State.Moving;
        camBase.transform.localPosition = ogPos;
    }

    IEnumerator IncreaseSphereCollider()
    {
        sphereCollider.SetActive(true);
        burstTime = 0;
        for (burstTime = 0; burstTime < 1; burstTime += Time.deltaTime*2)
        {
            float size = Mathf.Lerp(1, 13, burstTime);
            sphereCollider.transform.localScale = new Vector3(size, size, size);
            yield return null;
        }
        burstTime = -1;
        sphereCollider.SetActive(false);
    }

    private void HandleTarget()
    {
        if (targetlock == false)
        {
            Collider[] precolliders = Physics.OverlapSphere(transform.position, 20, whatIsEnemies);

            Transform tMin2 = null;
            float minDist2 = Mathf.Infinity;
            Vector3 currentPos2 = transform.position;

            foreach (Collider c in precolliders)
            {
                float dist = Vector3.Distance(c.transform.position, currentPos2);
                if (dist < minDist2)
                {
                    tMin2 = c.transform;
                    minDist2 = dist;
                }
            }
            foreach (Collider c in precolliders)
                if (c.transform == tMin2)
                    c.GetComponentInChildren<PreIcon>().enablesprite = true;
        }

        if (targetanalyse == true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 20, whatIsEnemies);
            System.Collections.Generic.List<Collider> list = new System.Collections.Generic.List<Collider>(colliders);
            
            foreach (Collider c in colliders)
            { 
                if(c.GetComponent<MeshRenderer>() != null)
                if(!c.GetComponent<MeshRenderer>().isVisible)
                    list.Remove(c);
                   
                if (c.GetComponent<SkinnedMeshRenderer>() != null)
                    if (!c.GetComponent<SkinnedMeshRenderer>().isVisible)
                        list.Remove(c);

                colliders = list.ToArray();
            }
           
            if (colliders.Length > 0)
            {

                Transform tMin = null;
                float minDist = Mathf.Infinity;
                Vector3 currentPos = transform.position;

                foreach (Collider c in colliders)
                {
                    float dist = Vector3.Distance(c.transform.position, currentPos);
                    if (dist < minDist)
                    {
                        tMin = c.transform;
                        minDist = dist;
                        
                    }
                }
                foreach (Collider c in colliders)
                {
                    if (c.transform == tMin)
                        c.GetComponentInChildren<Icon>().enablesprite = true;
                }
               
                enemy = tMin;
                cam.GetComponentInParent<CameraBehaviour>().target2 = tMin;

            }
            else
                targetlock = false;

            targetanalyse = false;

        }
        if (targetlock == true && enemy.gameObject.activeSelf)
        {
            Vector3 currentPosition = transform.position;
           
            dist2 = Vector3.Distance(enemy.transform.position, currentPosition);

            if (dist2 > 23f)
                targetlock = false;
        }
    }


    void CanAttackChecker(int canAttack)
    {
        if (canAttack == 0)
            canClick = false;
        else if (canAttack == 1)
            canClick = true;
    }
    void IsAttacking(int canAttack)
    {
        if (canAttack == 0)
            isAttacking = false;
        else if (canAttack == 1)
            isAttacking = true;
    }

    IEnumerator ComboCd()
    {
        comboCooldown = true;
        yield return new WaitForSeconds(1f);
        comboCooldown = false;
    }

    IEnumerator SwordTrail()
    {
        swordTrail.Play();
        yield return new WaitForSeconds(2);
        swordTrail.Stop();
    }

    void PlaySound(string name) => AudioManager.i.Play(name, transform.position);

    void PlayElementCharge() 
    {
        if (currentAbility == airEffect)
            AudioManager.i.Play("ChargeAir", transform.position);
        else if (currentAbility == fireEffect)
            AudioManager.i.Play("ChargeFire", transform.position);
        else if (currentAbility == waterEffect)
            AudioManager.i.Play("ChargeWater", transform.position);
    }

    void TargetsAndBoulders()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        
        if (Input.GetButtonUp(LeftClick))
        {
            if ((attacktimer > 2.5f && attacktimer < 2.99f) && !comboCooldown && !state.IsName("combo2") && !state.IsName("combo3"))
            {
                animator.SetTrigger("Combo1");
                StartCoroutine(ComboCd());
            }
            else if (attacktimer < 2.5f && attacktimer > 0)
            {
                animator.SetTrigger("Aoe");
                HandleChargeEffect(false);
            }
        }
        
        if (Input.GetButtonDown(LeftClick) && canClick)
        {
            if (state.IsName("combo1")) animator.SetTrigger("Combo2");
            else if (state.IsName("combo2") ) animator.SetTrigger("Combo3");
            swordTrail.Play();
            canClick = false;
        }
        
        if (Input.GetButton(LeftClick) && !canClick)
        {
            // isAttacking = false;    
            attacktimer -= Time.deltaTime;
            rbdg.velocity = new Vector3(0, rbdg.velocity.y, 0);
            // movement = Vector3.zero;
        }
        else
            attacktimer = 3;
        
        if (attacktimer < 2.5f && attacktimer > 0)
        {
            if (!state.IsName("chargingelement") && !state.IsName("chargeattack"))
            {
                animator.SetTrigger("ChargeSword");
                HandleChargeEffect(true);
            }
        }
        else if (attacktimer < 0)
        {
            animator.SetTrigger("FinalCharged");
            attacktimer = 3;
            HandleChargeEffect(false);
        }

        if (attacktimer < 3)
            rbdg.velocity = new Vector3(0, rbdg.velocity.y, 0);
            // movement = Vector3.zero;

        // if (state.IsName("combo1") || state.IsName("combo2") || state.IsName("combo3"))
        // {
        //     rbdg.velocity = new Vector3(rbdg.velocity.x, 0f, rbdg.velocity.z);
        //     rbdg.useGravity = false;
        //     playerMovement.acc = 0;
        // }
        // else 
        //     rbdg.useGravity = true;


        if (boulder != null)
        {
            if (boulder.GetComponent<BoulderKnock>().boulderscene == true)
            {
                bouldershake = true;
                enemystop = true;
                rbdg.velocity = Vector3.zero;
                rbdg.isKinematic = true;
            }
            else
            {
                rbdg.isKinematic = false;
            }
            if (bouldershake == true && i < 1)
            {
                
                movement /= 0;
                StartCoroutine(CameraShaker2(2f, 10f, 10f));
                i++;
            }
            else
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = .02f;
                enemystop = false;
            }
        }

        if (boulder2 != null)
        {
            if (boulder2.GetComponent<BoulderPlayer>().boulderscene == true && i < 1)
            {
                bouldershake = true;
                rbdg.velocity = Vector3.zero;
                rbdg.isKinematic = true;
            }
            else
            {
                rbdg.isKinematic = false;
            }
            if (bouldershake == true && i < 1)
            {

                movement /= 0;
                StartCoroutine(CameraShaker(2f, 10f, 10f));
                i++;
            }
            else
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = .02f;
                movement = movement;
            }
        }

        if (boulder3 != null)
        {
            if (boulder3.GetComponent<BoulderPlayer>().boulderscene == true && i < 1)
            {
                bouldershake = true;
                rbdg.velocity = Vector3.zero;
                rbdg.isKinematic = true;
            }
            else
            {
                rbdg.isKinematic = false;
            }
            if (bouldershake == true && i < 1)
            {
                movement /= 0;
                StartCoroutine(CameraShaker(2f, 10f, 10f));
                i++;
            }
            else
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = .02f;
            }
        }

        // if (Input.GetButton(LeftClick) && attacktimer > 0 && combopossible == false&&combodone==false && !animator.GetCurrentAnimatorStateInfo(0).IsName("swordcombo1"))
        // {
        //     attacktimer -= Time.deltaTime;
          
        //     if (attacktimer <= 1.87f  && combopossible==false && combodone == false &&  !animator.GetCurrentAnimatorStateInfo(0).IsName("swordcombo1"))
        //     {
        //         buttonhelddown = true;
        //         animator.Rebind();
        //     }
        // }

        // if (combopossible == true && combodone == false &&  !animator.GetCurrentAnimatorStateInfo(0).IsName("swordcombo1") && Input.GetButton(LeftClick))
        // {
        //     combotimer += Time.deltaTime;
        //     if (combotimer > 1.5f)
        //     {
        //         combopossible = false;
        //         combotimer = 0f;
        //     }
        // }

        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("SwordIdle"))
        //     noattack = true;
        // else
        //     noattack = false;
        // if (!animator.GetCurrentAnimatorStateInfo(0).IsName("swordcombo1"))
        //     noattack = true;
        // else
        //     noattack = false;

        // if (Input.GetButton(LeftClick) && attacktimer < 1.9f && attacktimer > 1.8f && combopossible == true)
        // if (buttonhelddown == true)

        // if (combopossible == true)
        // {
        //     normalattacktimer -= Time.deltaTime;
        //     if (normalattacktimer > 0.4f )
        //     {
        //         if (Input.GetButtonUp(LeftClick) )
        //         {
        //             aoeAttack = false;
        //             finalchargeattack = false;
        //             // animator.SetTrigger("Combo2");
        //         }
        //     }
        //     else
        //     {
        //         combodone = true;
        //     }
        // }

        // if (combodone == true)
        // {
        //     normalattacktimer = 2.0f;
        //     attacktimer = 2.0f;
        //     aoeAttack = false;
        //     finalchargeattack = false;
        //     attackcooldown -= Time.deltaTime;
        //     if (attackcooldown < 0)
        //     {
        //         // animator.ResetTrigger("Combo1");
        //         combopossible = false;
        //         combodone = false;
        //     }
        // }

        // if (attacktimer <= 2f && attacktimer > 1.8f && Input.GetButtonDown(LeftClick) && combopossible == false && combodone == false)
        //     animator.SetTrigger("Combo");

        // if (attacktimer <= 2f && attacktimer > 1.8f && Input.GetButtonUp(LeftClick) && combodone == false)
        //     combopossible = true;              

        // if (attacktimer <= 1.8f && attacktimer > 0f && Input.GetButtonUp(LeftClick) && combopossible == false && combodone == false)
        // {
        //     aoeAttack = true;
        //     buttonhelddown = false;
        //     normalattacktimer = 2.0f;
        //     attacktimer = 2.0f;
        // }
        // if (attacktimer <= 1.8f && attacktimer > 0f && combopossible == false && combodone == false)
        // {
        //     HandleChargeEffect(true);
        //     ischarging = true;
        // }
        // if (attacktimer < 0f && combopossible == false && combodone == false)
        //     isfinalcharged = true;

        // if (attacktimer <= 0f && Input.GetButtonUp(LeftClick))
        // {
        //     ischarging = false;
        //     finalchargeattack = true;
        //     buttonhelddown = false;
        //     normalattacktimer = 2.0f;
        //     attacktimer = 2.0f;
        // }

        // if (aoeAttack == true && combopossible == false && combodone == false)
        // {
        //     noattack = false;
        //     sword.GetComponent<KnockBack>().AreaofEffect = true;
        //     // anim.SetTrigger("AOEAttack");
        //     ischarging = false;
        //     aoeAttack = false;
        //     finalchargeattack = false;
        //     HandleChargeEffect(false);
        // }
        // if (finalchargeattack == true && combopossible == false && combodone == false)
        // {
        //     sword.GetComponent<KnockBack>().FinalChargeAttack = true;
        //     // anim.SetTrigger("FinalChargeAttack");
        //     animator.SetTrigger("Charge");
        //     isfinalcharged = false;
        //     aoeAttack = false;
        //     finalchargeattack = false;
        //     HandleChargeEffect(false);
        // }

        if(playshaker == true)
        {
            StartCoroutine(CameraShaker3(0.05f, 0.01f, 0.01f));
            playshaker = false;
        }
        if (combodone == false)
            attackcooldown = 0.5f;

        if (targetlock == true)
        {
            if (!enemy.gameObject.activeSelf)
                targetlock = false;
            if (enemy.gameObject.activeSelf == false)
                return;

            lookenemy = enemy.position - transform.position;

            transform.forward = (new Vector3(lookenemy.x, 0, lookenemy.z)).normalized;
            if (Input.GetKey(KeyCode.D))
                transform.right = Quaternion.Euler(0, -30, 0) * transform.right;
            if (Input.GetKey(KeyCode.A))
                transform.right = Quaternion.Euler(0, 30, 0) * transform.right;

            if(enemy.position.y > transform.position.y +1f && Vector3.Distance(enemy.position, transform.position) < 15f)
            {
                distancetoadd = enemy.position.y - transform.position.y;
                bling = true;
            }
            else
            {
                bling = false;
            }
            if (targetlock == false)
                bling = false;

            if (Vector3.Distance(enemy.position, transform.position) < 30f && enemy.position.y > transform.position.y && enemy.position.x + 2f > transform.position.x && enemy.position.x - 2f < transform.position.x && enemy.position.z + 2f > transform.position.z && enemy.position.z - 2f < transform.position.z && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
                movement = Vector3.zero;
            else
            {
                movement = transform.right * movement.x + transform.forward * movement.z;
                if (Input.GetKey(KeyCode.S))
                    hips.transform.rotation = Quaternion.LookRotation(new Vector3(-lookenemy.x, 0, -lookenemy.z));

            }
        }
                   
        if (targetlock == false)
        {
            bling = false;
            movement = cam.transform.TransformDirection(movement.x, 0, movement.z); 
            animator.SetBool("TargetLocked", false);

        }
    }

}
