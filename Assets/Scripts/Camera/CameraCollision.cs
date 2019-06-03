using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 5.0f;
    public float smooth;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public bool hitwall;
    public float distance;
    Camera cam;
    RaycastHit hit;
    RaycastHit hit2;
    Vector3 desiredCameraPos;
    Vector3 otherdesiredPos;
    private GameObject RayCastPoint;
    private GameObject NEO;
    public LayerMask layerMask;
    private PlayerInputController playerInputController;
    internal bool playingCutscene;
    private CameraBehaviour camBehaviour;
    void Awake()
    {
        playerInputController = GameObject.Find("Player").GetComponent<PlayerInputController>();
        RayCastPoint = GameObject.Find("RayCast");
        NEO = GameObject.Find("NEO");
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
        cam = GetComponent<Camera>();
        camBehaviour = GetComponentInParent<CameraBehaviour>();
    }

    private void OnDrawGizmos() => Gizmos.DrawLine(transform.parent.position, new Vector3(desiredCameraPos.x, desiredCameraPos.y - 1, desiredCameraPos.z));

    void LateUpdate()
    {
        desiredCameraPos = transform.parent.TransformPoint(dollyDir*15);
        otherdesiredPos = transform.parent.TransformPoint(new Vector3(0,2,0));
    
        if(playerInputController.bling == true && maxDistance <= 18)
        {
            maxDistance += playerInputController.distancetoadd/150f;
        }
        else if(maxDistance >= 8.3f)
        {
            maxDistance -= playerInputController.distancetoadd/100f;
        }
        else
        {
            maxDistance = 8;
        }

        if (Physics.Linecast(transform.parent.position, new Vector3(desiredCameraPos.x, desiredCameraPos.y - 1, desiredCameraPos.z), out hit ,layerMask) )
        {
            distance = Mathf.Clamp((hit.distance * 0.6f), 0f, maxDistance);
        }
        else
        {           
            distance = maxDistance;
            hitwall = false;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
            
}