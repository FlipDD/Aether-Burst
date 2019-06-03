using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public static bool isMoving;
    private Vector3 ogPos;
    private Vector3 goToPos;
    private Vector3 refVel = Vector3.zero;
    private bool goingUp;
    private Rigidbody rgbd;

    void Start()
    {
        ogPos = transform.position;
        goToPos = new Vector3(ogPos.x, ogPos.y - 45, ogPos.z);
        rgbd = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            if (!goingUp) 
            {
                rgbd.MovePosition(Vector3.SmoothDamp(transform.position, goToPos, ref refVel, 3f));
                if (Vector3.Distance(goToPos, transform.position) < .1f) goingUp = true;
            }
            else
            {
                rgbd.MovePosition(Vector3.SmoothDamp(transform.position, ogPos, ref refVel, 1.5f));
                if (Vector3.Distance(ogPos, transform.position) < .1f) goingUp = false;
            }
        }
    }

    internal bool SetIsMoving()
    {
        isMoving = true;
        return isMoving;
    }
}
