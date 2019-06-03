using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouthTerrainBoss : MonoBehaviour
{
    JorgeController jorge;

    void Start() => jorge = FindObjectOfType<JorgeController>();

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("IsGroundChecker"))
            jorge.crossedToSouth = true;
    }
}
