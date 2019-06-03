using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 direction = collision.transform.position - transform.position;
                GameObject collider = collision.gameObject;
                direction.y = 0;


                // rb.AddForce(direction.normalized * (600f * 2), ForceMode.Impulse);




            }
        }

    }
}
