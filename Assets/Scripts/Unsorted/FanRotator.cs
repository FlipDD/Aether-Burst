using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float x, y, z;

    void Update()
    {
        x +=  rotationSpeed * Time.deltaTime;
        if (x >= 360) x = 0;
        transform.rotation = Quaternion.Euler(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boulder"))
        {
            gameObject.SetActive(false);
        }
    }
}
