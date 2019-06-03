using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InverseKinematicsCC : MonoBehaviour
{
    #region Variables
    [Header("Options")]
    [Range(0,2)]
    public float elbowPower;

    [Header("Joint References")]
    public Transform endEffector;
    public Transform centroidRef;
    public Transform elbowRef;
    public Transform upperRef;
    public Transform lowerRef;
    public Transform handRef;

    [HideInInspector]
    public Vector3 centroidPos;

    private Vector3 tempCentroid;
    private Vector3 tempUpper;
    private Vector3 tempLower;
    private Vector3 tempHand;

    private float centroidToUpperDist;
    private float upperToLowerDist;
    private float lowerHandDist;

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        if(handRef != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(upperRef.position, centroidRef.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(upperRef.position, lowerRef.position);
            Gizmos.DrawLine(lowerRef.position, handRef.position);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(lowerRef.position, elbowRef.position);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endEffector.position, 0.15f);


            Gizmos.color = Color.green;
            Gizmos.DrawLine(handRef.position, endEffector.position);
        }
    }
    #endregion



    private void Awake()
    {
        SetupVariables();
    }

    private void SetupVariables()
    {
        centroidToUpperDist = Vector3.Distance(upperRef.position, centroidRef.position);
        upperToLowerDist = Vector3.Distance(upperRef.position, centroidRef.position);
        lowerHandDist = Vector3.Distance(lowerRef.position, centroidRef.position);

        centroidPos = centroidRef.position;
        tempCentroid = centroidPos;
    }


    // Update is called once per frame
    private void LateUpdate()
    {
        SolveBackwards(endEffector.position);
        SolveForward(centroidPos);
        SetTempPos();   
    }

    public Vector3 SolveBackwards(Vector3 endEffetor)
    {
        tempCentroid = centroidPos;

        tempHand = endEffetor;

        Vector3 handtoLower = (lowerRef.position - tempHand).normalized * lowerHandDist;
        tempLower = tempHand + handtoLower + (elbowRef.position - lowerRef.position).normalized*elbowPower;

        Vector3 lowertoupper = (upperRef.position - tempLower).normalized * upperToLowerDist;
        tempUpper = tempLower + lowertoupper;

        Vector3 uppertoCentroid = (centroidRef.position - tempUpper).normalized * centroidToUpperDist;
        tempCentroid = tempUpper + uppertoCentroid;

        return tempCentroid;
    }

    public void SolveForward( Vector3 rootPoint)
    {
        tempCentroid = rootPoint;

        Vector3 centroidtoUpper = (tempUpper - tempCentroid).normalized * centroidToUpperDist;
        tempUpper = tempCentroid + centroidtoUpper;
        Vector3 upperToLower = (tempLower - tempUpper).normalized * upperToLowerDist;
        tempLower = tempUpper + upperToLower;
        Vector3 lowerHand = (tempHand - tempLower).normalized * centroidToUpperDist;
        tempHand = tempLower + lowerHand;
    }

    public void SetTempPos()
    {
        upperRef.position = tempUpper;
        lowerRef.position = tempLower;
        handRef.position = tempHand;

        centroidPos = centroidRef.position;
    }
}
    