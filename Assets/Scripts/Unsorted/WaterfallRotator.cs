using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallRotator : MonoBehaviour
{
    Vector3 newRotation;

    void Start()
    {
        newRotation = new Vector3(1f, 0, 1);
    }

    void Update()
    {
        newRotation += new Vector3(4, Random.Range(1, 3), Random.Range(1, 3)) * Time.deltaTime * 50;
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
