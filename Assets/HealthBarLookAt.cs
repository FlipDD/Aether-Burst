using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLookAt : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform objToFollow;
    private GameObject camObject;
    private Transform selfTransform; 
    private Transform parentEnemy;
    private GameObject player;
    Quaternion quat;

    private Vector3 scale;
    void Awake()
    {
       quat = transform.rotation;
       parentEnemy = transform.parent;
       scale = transform.localScale;
    } 
    
    void Start()
    {
        selfTransform = GetComponent<Transform>();
        if (cam == null)
        {
            camObject = GameObject.Find("CamBase");
            cam = camObject.transform.GetChild(0).transform;
        }

        player = GameObject.Find("Player");

        // if (objToFollow == null) 
        //     objToFollow = GameObject.Find("CamBase").transform.GetChild(0).transform;
    }

    void LateUpdate()
    {   
        if (objToFollow != null) transform.position = objToFollow.position;
        transform.LookAt(cam, Vector3.up);
    }
}
