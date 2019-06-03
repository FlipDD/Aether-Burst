using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttack : MonoBehaviour
{
    private UIBar uIBar;
    private SphereCollider sphereCollider;

    void Start() 
    {
        StartCoroutine(SelfDeactivate());
        sphereCollider = GetComponent<SphereCollider>();
        AudioManager.i.Play("BossPoison", transform.position);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            uIBar = col.GetComponent<UIBar>();
            sphereCollider.enabled = false;
            StartCoroutine(PoisonDoT());
        }
    }

    IEnumerator SelfDeactivate()
    {
        yield return new WaitForSeconds(4);
        sphereCollider.enabled = false;
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }

    IEnumerator PoisonDoT()
    {
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        yield return new WaitForSeconds(.5f);
        uIBar.UpdateHp(-3);
        gameObject.SetActive(false);
    }
}
