using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AetherBurst : MonoBehaviour
{
    private Rigidbody rgbd;

    void Start() => rgbd = GetComponent<Rigidbody>();

    internal void Explode() => rgbd.AddExplosionForce(10, transform.position, 10);

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<HealthComponent>().DamageEnemy(10);
            col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(50000, transform.position, 20, .5f);
        }
        else if (col.gameObject.CompareTag("Skull"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(10000, transform.position, 20, .5f);

        }
    }
}
