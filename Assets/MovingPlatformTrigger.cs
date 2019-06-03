using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTrigger : MonoBehaviour
{
    void OnTriggerEnter()
    {
        transform.GetChild(0).GetComponent<MovingPlatform>().SetIsMoving();
        // Destroy(gameObject);
    }
}
