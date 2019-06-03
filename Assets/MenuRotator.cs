using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotator : MonoBehaviour
{
    public bool left;
    void Update()
    {
        if (left)
            transform.Rotate(Vector3.forward, 30 * Time.deltaTime);
        else
            transform.Rotate(Vector3.forward, -30 * Time.deltaTime); 
    }
}
