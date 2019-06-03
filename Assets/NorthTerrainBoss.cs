using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthTerrainBoss : MonoBehaviour
{
    JorgeController jorge;

    void Start() => jorge = FindObjectOfType<JorgeController>();

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("IsGroundChecker"))
            jorge.crossedToNorth = true;
    }
}