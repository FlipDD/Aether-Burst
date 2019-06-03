using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnCollision : MonoBehaviour
{
    private List<Rigidbody> smallrocksRb;
    private List<Transform> smallRocksTf;
    private List<MeshCollider> smallRocksMc;
    private Rigidbody bigRock;
    private bool hasCollided;

    void Start()
    {
        smallrocksRb = new List<Rigidbody>();
        smallRocksTf = new List<Transform>();
        smallRocksMc = new List<MeshCollider>();

        bigRock = GetComponent<Rigidbody>();

        for (int i = 0; i < transform.childCount; i++)
        {
            smallrocksRb.Add(transform.GetChild(i).GetComponent<Rigidbody>());
            smallRocksTf.Add(transform.GetChild(i));
            smallRocksMc.Add(transform.GetChild(i).GetComponent<MeshCollider>());
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("SmallRocks") && !col.gameObject.CompareTag("Enemy") && !hasCollided)
        {
            hasCollided = true;
            foreach (Rigidbody rg in smallrocksRb)
            {
                rg.isKinematic = false;
                rg.transform.parent = null;
            }
            foreach (MeshCollider mc in smallRocksMc)
                mc.isTrigger = false;

        }

        if (col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<UIBar>().UpdateHp(-20);
    }
}
