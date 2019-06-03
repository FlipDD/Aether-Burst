using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotator : MonoBehaviour
{
    [SerializeField]
    private Transform cam;

    void Update() => transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));
}
