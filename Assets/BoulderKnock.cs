using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderKnock : MonoBehaviour
{
    public bool boulderscene;

    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<UIBar>().UpdateHp(-100f);
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
            {
            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0;

            rb.AddForce(direction.normalized * (100000), ForceMode.Impulse);

            //collision.gameObject.GetComponent<HealthComponent>().DamageEnemy(15);

            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0;

                rb.AddForce(direction.normalized * (10000), ForceMode.Impulse);

                //collision.gameObject.GetComponent<HealthComponent>().DamageEnemy(15);

            }
        }

        if (collision.gameObject.CompareTag("SpecialBoulder"))
        {
            boulderscene = true;
        }

        if (collision.gameObject.CompareTag("SpecialPilar"))
        {
            collision.gameObject.SetActive(false);
        }

    }
}
