using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform targetPosition;
    [SerializeField]
    private Transform gargoyleToSpawn;

    void Start()
    {
        if (targetPosition == null) transform.Find("Player");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Transform enemy = Instantiate(gargoyleToSpawn, targetPosition.position, Quaternion.identity);
            // enemy.localScale = new Vector3(1.5f, 3, 1.5f);
            Instantiate(GameAssets.i.rockShattered, targetPosition.position, Quaternion.identity);
            Destroy(targetPosition.gameObject);
        }
    }
}
