using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofKillEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.name.Contains("Gargoyle"))          
                other.transform.parent.gameObject.SetActive(false);
            else
                other.gameObject.SetActive(false);
        }
    }
}
