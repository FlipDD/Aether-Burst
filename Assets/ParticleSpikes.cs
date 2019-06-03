using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpikes : MonoBehaviour
{
    void OnParticleCollision(GameObject col) 
    {
        if (col.CompareTag("Player"))
            col.GetComponent<UIBar>().UpdateHp(-10);
    }
}
