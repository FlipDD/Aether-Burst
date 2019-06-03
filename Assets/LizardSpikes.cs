using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardSpikes : MonoBehaviour
{
    private Rigidbody rgbd;
    private MeshCollider meshCollider;

    void Start()
    {
        transform.Rotate(Vector3.up, -100);
        meshCollider = GetComponent<MeshCollider>();
        rgbd = GetComponent<Rigidbody>();
        rgbd.AddForce(transform.parent.forward * 75, ForceMode.Impulse);
        StartCoroutine(MakeCollider());
    }

    void OnCollisionEnter(Collision col)
    {
        gameObject.SetActive(false);

        if (col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<UIBar>().UpdateHp(-10);
    }

    IEnumerator MakeCollider()
    {
        yield return new WaitForSeconds(.18f);
        meshCollider.isTrigger = false;
    }
}
