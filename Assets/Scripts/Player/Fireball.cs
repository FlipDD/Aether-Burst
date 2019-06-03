using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private LayerMask whatIsEnemies;
    private Transform parent;
    private CapsuleCollider sphereCollider;
    private ParticleSystem fireEffectPs;
    private ParticleSystem fireBreathPs;
    private Animator anim;
    internal bool isCharging;
    public bool readyToFire;

    void Start()
    {
        anim = GetComponent<Animator>();
        sphereCollider = GetComponent<CapsuleCollider>();
        fireEffectPs = GetComponent<ParticleSystem>();
        fireBreathPs = GameObject.FindGameObjectWithTag("FireBreath").GetComponent<ParticleSystem>();
        parent = GameObject.Find("mixamorig:LeftHand").transform;
        readyToFire = true;
    }

    internal void Shoot ()
    {
        if (!isCharging)
            StartCoroutine(ChargeTime());
    }

    IEnumerator ChargeTime ()
    {
        isCharging = true;
        anim.SetTrigger("Casting");
        yield return new WaitForSeconds(.7f);
        fireBreathPs.transform.parent = parent;
        fireBreathPs.transform.localPosition = Vector3.zero;
        // fireBreathPs.transform.parent = transform.parent.transform.parent;
        // fireBreathPs.transform.position =  transform.parent.position + (transform.right * 1.25f) + (transform.forward * .5f);  
        fireBreathPs.transform.rotation = Quaternion.LookRotation(transform.parent.forward, Vector3.up);
        fireBreathPs.Play();
        Collider[] col = Physics.OverlapSphere(transform.position + (transform.forward * 2f) + (transform.right * 1.25f), 3, whatIsEnemies);
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.CompareTag("Enemy"))
            {
                GameObject tmp = col[i].gameObject;
                tmp.GetComponent<EnemyBase>().Burned();
                tmp.GetComponent<HealthComponent>().DamageEnemy(10);
            }
            else if (col[i].gameObject.CompareTag("Rope"))
            {
                Transform parent = col[i].transform.parent;
                Instantiate(GameAssets.i.burningEffect, col[i].transform.position + (Vector3.up*2.5f), Quaternion.identity);
                for (int w = 0; w < parent.childCount; w++)
                {
                    if (parent.GetChild(w).CompareTag("Ground"))
                        parent.GetChild(w).GetComponent<Rigidbody>().isKinematic = false;
                    else 
                        parent.GetChild(w).gameObject.SetActive(false);
                }
                //burn rope
            }
        }
        yield return new WaitForSeconds(.5f);
        fireBreathPs.transform.parent = null;
        fireBreathPs.Stop();
        fireEffectPs.Play();
        isCharging = false;
    }
    
    IEnumerator WaitForDamage(HealthComponent health)
    {
        yield return new WaitForSeconds(.5f);
        health.DamageEnemy(10); 
    }
}
