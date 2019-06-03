using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    // public Transform camTransform;
	// public float shakeDuration = 0.5f;
	// public float shakeAmount = 0.5f;
	
	// Vector3 originalPos;
	
	// void Awake()
	// {
	// 	if (camTransform == null)
	// 	{
	// 		camTransform = GetComponent(typeof(Transform)) as Transform;
	// 	}
	// }

    // void OnEnable() => originalPos = camTransform.localPosition;

    // void Update()
	// {
	// 	if (shakeDuration > 0)
	// 	{
	// 		camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
		
	// 		shakeDuration -= Time.deltaTime;
	// 	}
	// 	else
	// 	{
	// 		shakeDuration = 0f;
	// 		camTransform.localPosition = originalPos;
	// 		enabled = false;
	// 	}
	// }
}
