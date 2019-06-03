using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    ObjectPooler objectPooler;
    public bool vamos;
    List<GameObject> enemylist;
    // private int numberofenemys;

    // Start is called before the first frame update
    void Start()
    {
        enemylist = new List<GameObject>();
        // numberofenemys = 0;
        vamos = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
       
               //ObjectPooler.Instance.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
               //ObjectPooler.Instance.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
      




        }
    }
}
