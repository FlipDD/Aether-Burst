using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerTail : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0;

            rb.AddForce(direction.normalized * 1000, ForceMode.Impulse);
            }


            collision.gameObject.GetComponent<UIBar>().UpdateHp(-20);
        }

    }
}
