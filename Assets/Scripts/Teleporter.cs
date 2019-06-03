using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    void OnTriggerEnter(Collider col) => col.gameObject.transform.position = new Vector3(-240f, 0f, -237f);
}
