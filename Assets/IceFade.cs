using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFade : MonoBehaviour
{
    private MeshRenderer rd;
    [SerializeField]
    private float time;
    [SerializeField]
    private bool isDoor;

    void Start()
    {
        rd = GetComponent<MeshRenderer>();
        if (!isDoor) StartCoroutine(Lifetime());
    }

    void OnCollisionEnter(Collision col)
    {   
        if (col.gameObject.CompareTag("Ground"))
            StartCoroutine(FadeOutIce(time));
    }

    IEnumerator FadeOutIce(float aTime)
    {
        for (float t = 0.0f; t < aTime; t += Time.deltaTime/aTime)
        {
            Color newColor = rd.material.color;
            newColor.a = Mathf.Lerp(aTime, 0, t);
            rd.material.color = newColor;  
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);    
    }
}
