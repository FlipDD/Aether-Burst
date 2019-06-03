using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private Vector3 speed = Vector3.zero;
    private bool orbiting, flying;
    private float t;
    private PlayerInputController p;
    private ParticleSystem ps;
    private bool selfDestroying;
    private Light selfLight;
    private Vector3 relativeDistance = Vector3.zero;
    private float timeSinceStart;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        p = player.GetComponent<PlayerInputController>();
        ps = GetComponent<ParticleSystem>();
        selfLight = GetComponent<Light>();
        relativeDistance = (transform.position - player.transform.position).normalized;
        flying = true;
        StartCoroutine(FlyUp());
        timeSinceStart = 0;
        AudioManager.i.Play("Aether", transform.position);
    }

    void Update() 
    {
        timeSinceStart += Time.deltaTime;
        if (!orbiting && !flying)
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * (timeSinceStart - 2)) ;
    }

    void LateUpdate()
    {
        if (orbiting)
        {
            if (t <= 0)
            {
                if (!selfDestroying)
                    StartCoroutine(DestroySelf());
            }
            else
            {
                t -= Time.deltaTime;
                transform.position = player.transform.position + relativeDistance;
                transform.RotateAround(player.transform.position, player.transform.up, Time.deltaTime * 600);
                relativeDistance = (transform.position - player.transform.position).normalized / 1.5f * t;
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !flying)
        {
            orbiting = true;
            t = 3;
            GetComponent<SphereCollider>().enabled = false; 
        }
    }

    IEnumerator DestroySelf()
    {
        selfDestroying = true;
        Destroy(transform.GetChild(0).gameObject);
        ps.Stop();
        p.UpdateAether(1);
        p.IncreaseBreath(15);
        float tt = 0;
        float intensity = selfLight.intensity;
        while (tt < 1)
        {
            tt += Time.deltaTime;
            selfLight.intensity = intensity - (t*intensity);
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator FlyUp()
    {
        float tt = 2;
        while (tt > 0)
        {
            tt -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + tt/25, transform.position.z);
            yield return null;
        }
        flying = false;
    }
}
