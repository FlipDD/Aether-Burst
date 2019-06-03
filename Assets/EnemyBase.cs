using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    internal enum BaseState {Idle, Frozen, Burning}
    internal BaseState baseState;
    internal Rigidbody rgbd;
    internal Animator animator;
    internal bool isInvincible;

    internal Vector3 goToPosition;
    internal bool freezing;
    internal bool burning;
    internal bool takingDamage;
    private Coroutine burn;
    private Transform burningEffect;

    void Start()
    {
        baseState = BaseState.Idle;
        burning = false;
    }

    void Update()
    {
        switch(baseState)
        {
            case BaseState.Frozen:
                if (!freezing)
                {
                    StartCoroutine(Freeze());
                }
                else
                    transform.position = Vector3.Lerp(transform.position, goToPosition, .05f);
            break;

            case BaseState.Burning:
                if (!burning)
                    StartCoroutine(Burn());
               
            break;
        }
    }

    internal IEnumerator Invicibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(.5f);
        isInvincible = false;
    }

    internal IEnumerator Freeze ()
    {
        freezing = true;
        goToPosition = transform.position + (Vector3.up * 6);
        Transform ps = Instantiate(GameAssets.i.waterPrison, transform.position, Quaternion.identity);
        ps.transform.parent = transform;
        rgbd.isKinematic = true;
        yield return new WaitForSeconds(7);
        Destroy(transform.GetChild(0).gameObject);
        rgbd.isKinematic = false;
        freezing = false;
        baseState = BaseState.Idle;
    }

    internal IEnumerator Burn ()
    {
        burning = true;
        burningEffect = Instantiate(GameAssets.i.burningEffect, transform.position, Quaternion.identity);
        burningEffect.transform.parent = transform;
        yield return new WaitForSeconds(4);
        // Destroy(burningEffect.gameObject);
        baseState = BaseState.Idle;
        burning = false;
    }

    internal IEnumerator BurnDoT(HealthComponent healthComponent, int healthPerSecond = 1)
    {
        takingDamage = true;
        healthComponent.DamageEnemy(healthPerSecond);
        yield return new WaitForSeconds(1);
        takingDamage = false;
    }

    internal void Froze ()
    {
        baseState = BaseState.Frozen;
    }

    internal void Burned ()
    {
        if (burningEffect != null) Destroy(burningEffect.gameObject);
        // StopCoroutine(burn);
        burning = false;
        baseState = BaseState.Burning;
    }
}
