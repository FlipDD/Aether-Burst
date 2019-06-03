using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseArrowSpawner : MonoBehaviour
{
    public bool shootarrow;
    internal bool arrowspawned;
    ObjectPooler objectPooler;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootarrow == true && arrowspawned == false)
        {
            ObjectPooler.Instance.SpawnFromPool("ReverseArrow", transform.position, Quaternion.identity);
            arrowspawned = true;
        }
        if (gameObject.GetComponentInChildren<DestroyedByBoulder>().destroyed == true)
        {
            gameObject.SetActive(false);
        }
    }


}