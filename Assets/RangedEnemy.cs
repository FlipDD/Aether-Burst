using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    public float firerate;
    float nextfire;
    public float firerate2;
    float nextfire2;
    public bool attacking;
    ObjectPooler objectPooler;
    private GameObject Player;
    public GameObject hitParticle;
    private Transform target;
    public State state;
    bool startedMoving;
    private Vector3 goToPosition;
    public float EnemyMoveSpeed = 10.0f;
    private ParticleSystem psWater;
    private Rigidbody rigidbody;
    private int i;
    private int i2;
    private Vector3 targetPosition;
    public bool notattacking;
    public bool isWaiting;
    public bool chargeattack;
    private float chargetimer;
    private bool notattacking2;
    private Animator anim;
    Vector3 velocity = Vector3.zero;
    private float positionToGox;
    private float positiontoGoy;
    public bool charging;

    public enum State
    {
        Idle,
        Frozen,

        Chasing,
        Hit
    }


    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        objectPooler = ObjectPooler.Instance;
        //firerate = 2f;
        nextfire = Time.time;
        Player = GameObject.Find("Player");
        state = State.Chasing;
        rigidbody = GetComponent<Rigidbody>();
        target = GameObject.Find("Player").transform;
        i = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Hit:
                break;

            case State.Chasing:
               targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
                transform.LookAt(targetPosition);
                var dist = Vector3.Distance(Player.transform.position, transform.position);
                if (dist < 55)
                {
                    if (notattacking == true && notattacking2 == false)
                    {
                        attacking = false;
                        StartCoroutine(WaitAndAttack(3f));
                       

                    }
                    if(notattacking2 == true && notattacking == false && charging == false)
                    {
                        chargeattack = false;
                        StartCoroutine(ChargeAndAttack(3f));
                    }
                
                }
                break;

            case State.Frozen:
                if (!startedMoving)
                    StartCoroutine(Freeze());
                else
                    transform.position = Vector3.Lerp(transform.position, goToPosition, 2f);
                break;
        }

        CheckTimeofFire();


    }

    private IEnumerator WaitAndAttack(float waitTime)
    {
      
       
        float step = EnemyMoveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position,
                        new Vector3 (targetPosition.x, targetPosition.y, targetPosition.z),
                        1f * Time.deltaTime);
        yield return new WaitForSeconds(waitTime);
       
   
        notattacking = false;
        i = 0;
        //yield return new WaitForSeconds(10f);




    }

    private IEnumerator ChargeAndAttack(float waitTime)
    {
        charging = true;
        float step = EnemyMoveSpeed * Time.deltaTime;
        


        yield return new WaitForSeconds(waitTime);
        transform.position = Vector3.SmoothDamp(transform.position,
                        new Vector3(positionToGox, positiontoGoy, transform.position.z ),
                        ref velocity,
                        8.5f*Time.deltaTime);
       

        notattacking2 = false;
        i2 = 0;
       
        //yield return new WaitForSeconds(10f);




    }

    void CheckTimeofFire()
    {
 
        var dist = Vector3.Distance(Player.transform.position, transform.position);
        if (notattacking == false)
        {

            if (dist < 50)
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
        if (Time.time > nextfire && attacking == true &&chargeattack==false)
        {
          
            ObjectPooler.Instance.SpawnFromPool("EnemyBullet", transform.position, Quaternion.identity);
            i++;
            nextfire2 = Time.time + firerate2;
            nextfire = Time.time + firerate;
            anim.SetTrigger("Small");
        }

        if(Time.time > nextfire2 && chargeattack == true &&attacking==false)
        {
            
            ObjectPooler.Instance.SpawnFromPool("ChargedBullet", transform.position, Quaternion.identity);
            chargeattack = false;
           
            nextfire = Time.time + firerate;
            nextfire2 = Time.time + firerate2;
            i++;
            anim.SetTrigger("Charging");
            positionToGox = transform.position.x + 10f;
            positiontoGoy = transform.position.y + 5f;
        }
        if (i == 2)
        {
            notattacking = true;
        }
        if(i == 1)
        {
            notattacking2 = true;
        }
        if(i==0)
            charging = false;


    }

    IEnumerator Freeze()
    {
        startedMoving = true;
        goToPosition = transform.position + (Vector3.up * 6);
        Transform ps = Instantiate(GameAssets.i.waterPrison, transform.position, Quaternion.identity);
        ps.transform.parent = transform;
        rigidbody.isKinematic = true;
        yield return new WaitForSeconds(7);
        int index = transform.childCount - 1;
        Destroy(transform.GetChild(index).gameObject);
        rigidbody.isKinematic = false;
        startedMoving = false;
        state = State.Chasing;
    }

    public void Froze()
    {
        state = State.Frozen;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (Player.GetComponent<PlayerInputController>().noattack == false)
        {
            if (collision.collider.tag == "Sword")
            {
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                GameObject hitparticle = Instantiate(hitParticle, pos, rot);

                //Destroy(hitparticle);
            }
        }
    }
}
