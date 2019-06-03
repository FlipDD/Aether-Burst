using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float KnockBackStrenght;
    private Vector3 direction;
    private GameObject target;
    internal Rigidbody rigidbody;
    internal float arrowspeed;
    private Vector3 moveDirection;
    public float moveSpeed = 70f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        rigidbody = gameObject.GetComponent<Rigidbody>();
        KnockBackStrenght = 600f;
        arrowspeed = 15;
        direction = target.transform.position - transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {   
        rigidbody.velocity += transform.forward * arrowspeed * Time.deltaTime;
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




    }

}
