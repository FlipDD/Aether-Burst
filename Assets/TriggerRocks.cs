    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRocks : MonoBehaviour
{
    private List<Rigidbody> rbs;
    private Transform rocks;
    private bool isFading;
    private PlayerInputController pic;

    void Start()
    {
        rbs = new List<Rigidbody>();
        rocks = transform.GetChild(0);

        for (int i = 0; i < rocks.childCount; i++)
            rbs.Add(rocks.GetChild(i).GetComponent<Rigidbody>());

        pic = FindObjectOfType<PlayerInputController>();
    }

    void OnTriggerEnter(Collider col)
    {
        StartCoroutine(pic.CameraShaker2(0.5f, 1, 1));
        if (!isFading && col.gameObject.CompareTag("Player"))
        {
            isFading = true;
            foreach (Rigidbody rb in rbs)
            {
                rb.useGravity = true;
                rb.AddForce(new Vector3(0, Random.Range(0f, -1f), 0) , ForceMode.Impulse);
            }
        }
        else if (!isFading && col.gameObject.CompareTag("Boulder"))
        {
            col.GetComponent<BoulderKnock>().boulderscene = true;
            isFading = true;
            foreach (Rigidbody rb in rbs)
            {
                rb.useGravity = true;
                rb.AddForce(new Vector3(0, Random.Range(0f, -1f), 0), ForceMode.Impulse);
            }
        }
        else if (!isFading && col.gameObject.CompareTag("Bullet"))
        {
            col.gameObject.SetActive(false);
            
        }
    }
}
