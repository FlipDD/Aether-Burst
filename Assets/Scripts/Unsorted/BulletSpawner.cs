using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    float timeMax = 20;
    float currentTime;

    void FixedUpdate()
    {
        if (currentTime < timeMax)
        {
            currentTime += .1f;
        }
        else 
        {
            currentTime = 0;
            Instantiate(GameAssets.i.bullet, transform.position, Quaternion.identity);
        }
    }
}
