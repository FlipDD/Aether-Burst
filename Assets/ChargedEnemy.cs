using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedEnemy : MonoBehaviour
{
    public float moveSpeed = 70f;
    public bool hitplayer;
    public SphereCollider bulletcollider;

    private Rigidbody rb;
    private GameObject target;
    private Vector3 moveDirection;
    private Vector2 moveDirection2;
    private SpriteRenderer sprite;
    [SerializeField]
    private float KnockBackStrenght;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnObjectSpawn()
    {
        rb = GetComponent<Rigidbody>();

        target = GameObject.Find("Player");
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed - (target.transform.right) - (target.transform.forward);
        rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        bulletcollider = GetComponent<SphereCollider>();
        direction = target.transform.position - transform.position;
        transform.LookAt(target.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.GetComponent<PlayerInputController>().enemystop == true)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.collider.GetComponent<UIBar>().UpdateHp(-10f);
            Rigidbody rb = other.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                
                direction.y = 0;
                direction.x = 0;
                rb.velocity = Vector3.zero;
                rb.AddForce(direction.normalized * KnockBackStrenght, ForceMode.Impulse);
            }


            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);

        }


        if (other.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);

        }

        gameObject.SetActive(false);


    }
}
