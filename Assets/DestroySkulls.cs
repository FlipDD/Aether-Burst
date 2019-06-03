using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySkulls : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Skull"))
        {
            col.gameObject.SetActive(false);
            Destroy(this, 3);
        }
    }
}
