using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAttackPoint : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    void Update()
    {
        target.position = transform.position;
    }
}
