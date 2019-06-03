using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUp : MonoBehaviour
{
    internal bool up;
    bool doorup;
    Vector3 initialpos;

    private void Start()
    {
        doorup = true;
        initialpos = transform.position;
    }

    private void FixedUpdate()
    {
        if(up == true && transform.position.y < initialpos.y +50)
        {
            transform.position = transform.position + Vector3.up*3;

          
        }

    }
}
