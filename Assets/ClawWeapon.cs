using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawWeapon : MonoBehaviour
{
    [SerializeField] private EnemyBehaviour enemy;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player") && enemy.GetClawing())
        {
            enemy.SetClawing(0);
            col.gameObject.GetComponent<UIBar>().UpdateHp(-20);
            col.gameObject.GetComponent<Rigidbody>().AddForce((transform.position - col.transform.position).normalized * 5000);
        }
    }
}
