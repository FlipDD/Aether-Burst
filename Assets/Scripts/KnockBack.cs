using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KnockBack : MonoBehaviour
{
    
    public bool AreaofEffect;
    public bool FinalChargeAttack;
    [SerializeField]
    private float KnockBackStrenght;
    public GameObject hitParticle;
    private GameObject player;
    private PlayerInputController playerScript;
    private HealthComponentBoss hpBoss;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerInputController>();

        if (SceneManager.GetActiveScene().name.Contains("Boss"))
            hpBoss = FindObjectOfType<HealthComponentBoss>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyWeapons") || collision.gameObject.CompareTag("Boss"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {

                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0;

                AnimatorStateInfo state = playerScript.animator.GetCurrentAnimatorStateInfo(0);
                if (playerScript.isAttacking)
                {
                    if (state.IsName("charge attack 1"))
                    {
                        player.GetComponent<PlayerInputController>().playshaker = true;
                        ContactPoint contact = collision.contacts[0];
                        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                        Vector3 pos = contact.point;
                        GameObject hitparticle = Instantiate(hitParticle, pos, rot);
                        GameObject collider = collision.gameObject;
                        rb.AddForce(direction.normalized * (KnockBackStrenght * 2), ForceMode.Impulse);
                        if (collision.gameObject.CompareTag("Enemy"))
                            collider.GetComponent<HealthComponent>().DamageEnemy(17);
                        else if (collision.gameObject.CompareTag("EnemyWeapons"))
                            collider.GetComponent<DamageEnemy>().DamageE(17);
                        else if (collision.gameObject.CompareTag("Boss"))
                            hpBoss.DamageEnemy(15);
                    }
                    else if (state.IsName("chargeattack"))
                    {
                        player.GetComponent<PlayerInputController>().playshaker = true;
                        ContactPoint contact = collision.contacts[0];
                        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                        Vector3 pos = contact.point;
                        GameObject hitparticle = Instantiate(hitParticle, pos, rot);
                        GameObject collider = collision.gameObject;
                        rb.AddForce(direction.normalized * (KnockBackStrenght * 3), ForceMode.Impulse);
                        if (collision.gameObject.CompareTag("Enemy"))
                            collider.GetComponent<HealthComponent>().DamageEnemy(25);
                        else if (collision.gameObject.CompareTag("EnemyWeapons"))
                            collider.GetComponent<DamageEnemy>().DamageE(25);
                        else if (collision.gameObject.CompareTag("Boss"))
                            hpBoss.DamageEnemy(25);
                    }
                    else if (state.IsName("combo1") || state.IsName("combo2") || state.IsName("combo3"))
                    {
                        player.GetComponent<PlayerInputController>().playshaker = true;
                        ContactPoint contact = collision.contacts[0];
                        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                        Vector3 pos = contact.point;
                        GameObject hitparticle = Instantiate(hitParticle, pos, rot);
                        GameObject collider = collision.gameObject;
                        rb.AddForce(direction.normalized * 100f, ForceMode.Impulse);
                        if (collision.gameObject.CompareTag("Enemy"))
                            collider.GetComponent<HealthComponent>().DamageEnemy(Random.Range(5, 10));
                        else if (collision.gameObject.CompareTag("EnemyWeapons"))
                            collider.GetComponent<DamageEnemy>().DamageE(Random.Range(5, 10));
                        else if (collision.gameObject.CompareTag("Boss"))
                            hpBoss.DamageEnemy(Random.Range(5, 11));
                    }

                }
            }
        }
        // else if (collision.gameObject.CompareTag("Boss") && playerScript.isAttacking)
        // {
        //     if (AreaofEffect)
        //         hpBoss.DamageEnemy(15);
        //     else if (FinalChargeAttack)
        //         hpBoss.DamageEnemy(25);
        //     else if (!AreaofEffect && !FinalChargeAttack)
        //         hpBoss.DamageEnemy(Random.Range(5, 10));
            
        // }
        else if (collision.gameObject.CompareTag("Rope"))
        {
            if (playerScript.selectedAbility == playerScript.fireEffect && collision.gameObject.CompareTag("Rope"))
            {
                Transform parent = collision.transform.parent;
                Instantiate(GameAssets.i.burningEffect, collision.transform.position + (Vector3.up*2.5f), Quaternion.identity);
                for (int w = 0; w < parent.childCount; w++)
                {
                    if (parent.GetChild(w).CompareTag("Ground"))
                        parent.GetChild(w).GetComponent<Rigidbody>().isKinematic = false;
                    else 
                        parent.GetChild(w).gameObject.SetActive(false);
                }
            }
        }
    }
}
