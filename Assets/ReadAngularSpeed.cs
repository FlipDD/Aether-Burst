using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadAngularSpeed : MonoBehaviour
{
    private Rigidbody rgbd;
    [SerializeField]
    private Transform door;
    private static bool activated;
    PlayerInputController playerInputController;
    GameObject pc;

    void Start()
    {
        rgbd = GetComponent<Rigidbody>();     
        pc = GameObject.FindGameObjectWithTag("Player");
        playerInputController = pc.GetComponent<PlayerInputController>();   
    }

    void Update()
    {
        if (!activated)
        {
            if (rgbd.angularVelocity.y > 6.9f)
            {
                rgbd.angularVelocity = new Vector3(0, 30, 0);
                rgbd.mass = 10000;
                rgbd.angularDrag = 0;
                rgbd.drag = 0;
                activated = true;
                door.gameObject.SetActive(false);
                StartCoroutine(playerInputController.CameraShaker(1.2f, 2.5f, 2.5f));
                
            }
            else if (rgbd.angularVelocity.y < -6.9f)
            {
                rgbd.angularVelocity = new Vector3(0, -30, 0);
                rgbd.mass = 10000;
                rgbd.angularDrag = 0;
                rgbd.drag = 0;
                activated = true;
                door.gameObject.SetActive(false);
                StartCoroutine(playerInputController.CameraShaker(1.2f, 2.5f, 2.5f));

            }
        }
    }
}
