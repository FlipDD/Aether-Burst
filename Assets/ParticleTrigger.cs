using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    void OnParticleCollision(GameObject other) 
    {
        if (other.CompareTag("Enemy"))
            other.GetComponent<HealthComponent>().DamageEnemy(10);
        else if (other.CompareTag("Boss"))
        {
            FindObjectOfType<HealthComponentBoss>().DamageEnemy(10);
            ParticleSystem ps = GetComponent<ParticleSystem>();
            var col = ps.collision;
            col.enabled = false;
        }

        else if (other.CompareTag("Door"))
        {
            for (int i = 0; i < other.transform.childCount; i++)
                other.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;

            StartCoroutine(DisableCollider(other.GetComponent<BoxCollider>()));
        }
    }

    IEnumerator DisableCollider(BoxCollider col)
    {
        yield return new WaitForSeconds(.3f);
        col.isTrigger = true;
    }
}
