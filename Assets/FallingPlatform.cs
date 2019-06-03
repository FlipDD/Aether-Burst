using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Vector3 originalPos;
    private bool falling, fallen;
    private IEnumerator fallingPlatform, waitToFall;
    private List<Rigidbody> rgbs;
    private Transform[] rocksTf;
    private Vector3[] rocksPosition;
    private Quaternion[] rocksRotation;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        fallingPlatform = ReturnPlatform();
        waitToFall = WaitToFall();

        rgbs = new List<Rigidbody>();
        rocksTf = new Transform[69];
        rocksPosition = new Vector3[69];
        rocksRotation = new Quaternion[69];
        for (int i = 0; i < transform.childCount-1; i++)
        {
            rgbs.Add(transform.GetChild(i).GetComponent<Rigidbody>());
            rocksTf[i] = transform.GetChild(i);
            rocksPosition[i] = transform.GetChild(i).position;
            rocksRotation[i] = transform.GetChild(i).rotation;
        }
    }

    void OnEnable()
    {
        foreach (Rigidbody rb in rgbs)
            rb.isKinematic = true;

        for (int i = 0; i < rocksTf.Length; i++)
        {
            rocksTf[i].position = rocksPosition[i];
            rocksTf[i].rotation = rocksRotation[i];
        }
       
        boxCollider.isTrigger = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("IsGroundChecker") && !falling)
            StartCoroutine(WaitToFall());
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("IsGroundChecker") && !fallen)
        {
            StopCoroutine(WaitToFall());
            StartCoroutine(ReturnPlatform());
        }
    }

    IEnumerator WaitToFall()
    {
        AudioManager.i.Play("BreakPlat", transform.position);
        falling = true;
        float t = 0;
        while (t < 1.5f)
        {
            for (int i = 0; i < rocksTf.Length; i++)
            {
                rocksTf[i].Rotate(Random.Range(-1.3f, 1.4f), 0, Random.Range(-1.3f, 1.4f));
            }
            t += Time.deltaTime;
            yield return null;
        }
        if (!fallen) StartCoroutine(ReturnPlatform());
    }

    IEnumerator ReturnPlatform()
    {
        fallen = true;
        boxCollider.isTrigger = true;
        foreach (Rigidbody rb in rgbs)
            rb.isKinematic = false;

        yield return new WaitForSeconds(6f);

        foreach (Rigidbody rb in rgbs)
            rb.isKinematic = true;

        float t = 0;
        while(t < 3)
        {
            for (int i = 0; i < rocksTf.Length; i++)
            {
                rocksTf[i].position = Vector3.Lerp(rocksTf[i].position, rocksPosition[i], t/30);
                rocksTf[i].rotation = Quaternion.Lerp(rocksTf[i].rotation, rocksRotation[i], t/30);
            }
            t += Time.deltaTime;
            yield return null;
        }
       
        boxCollider.isTrigger = false;
        fallen = false;
        falling = false;
    }
}

    // private Rigidbody plat;
    // private BoxCollider boxCollider;
    // private PlayerInputController player;
    // private Vector3 originalPos;
    // private bool falling, fallen;
    // private IEnumerator fallingPlatform, waitToFall;

    // void Start()
    // {
    //     plat = GetComponent<Rigidbody>();
    //     boxCollider = GetComponent<BoxCollider>();
    //     GameObject playerOb = GameObject.Find("Player");
    //     player = playerOb.GetComponent<PlayerInputController>();
    //     fallingPlatform = ReturnPlatform();
    //     waitToFall = WaitToFall();
    // }

    // void OnTriggerEnter(Collider col)
    // {
    //     if (col.gameObject.CompareTag("IsGroundChecker") && !falling)
    //         StartCoroutine(waitToFall);
    // }

    // void OnTriggerExit(Collider col)
    // {
    //     if (col.gameObject.CompareTag("IsGroundChecker") && !fallen)
    //     {
    //         StopCoroutine(waitToFall);
    //         StartCoroutine(fallingPlatform);
    //     }
    // }

    // IEnumerator WaitToFall()
    // {
    //     falling = true;
    //     yield return new WaitForSeconds(1);
    //     StartCoroutine(fallingPlatform);
    // }

    // IEnumerator ReturnPlatform()
    // {
    //     fallen = true;
    //     originalPos = transform.position;
    //     plat.isKinematic = false;
    //     boxCollider.isTrigger = true;
    //     yield return new WaitForSeconds(5f);
    //     plat.transform.position = originalPos;
    //     plat.isKinematic = true;
    //     boxCollider.isTrigger = false;
    //     fallen = false;
    //     falling = false;
    // }
// }

