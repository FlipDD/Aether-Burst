using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCollision : MonoBehaviour
{
    private MeshCollider col;
    private Transform iceSpikes;
    internal Vector3 rot;

    void Start()
    {
        col = GetComponent<MeshCollider>();
        col.convex = true;
        col.isTrigger = true;
        StartCoroutine(SpawnCube());
    }

    public void SetRotation(Vector3 rotation) => rot = rotation;

    private IEnumerator SpawnCube()
    {
        yield return new WaitForSeconds(.5f);
        col.isTrigger = false;
        col.convex = false;
        yield return new WaitForSeconds(3.3f);
        iceSpikes = ObjectPooler.Instance.SpawnFromPool("ShatteredIce", transform.position, Quaternion.identity).transform;
        Quaternion rotation = Quaternion.LookRotation(rot);
        rotation *= Quaternion.Euler(-90, 0, 180);
        iceSpikes.rotation = rotation;
        Destroy(gameObject);
    }

    void OnDisable()
    {
        PlayerInputController pic = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
        pic.isGrounded = false;
    }
}
