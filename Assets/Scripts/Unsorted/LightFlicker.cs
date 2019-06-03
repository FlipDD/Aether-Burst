using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light lightSource;
    [SerializeField]
    private float min;
    [SerializeField]
    private float max;
    [SerializeField]
    private float damping;
    [SerializeField]
    private float strength;
    private float initalIntensity;
    private bool flickering;

    public void Reset()
    {
        min = 0.2f;
        max = 0.2f;
        damping = 0.1f;
        strength = 300;
    }

    public void Start()
    {
        lightSource = GetComponent<Light>();
        initalIntensity = lightSource.intensity;
    }

    void Update()
    {
        if (!flickering)
            StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        flickering = true;
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, Random.Range(initalIntensity - max, initalIntensity + min), strength * Time.deltaTime);
        yield return new WaitForSeconds(Random.Range(damping-.02f, damping));
        flickering = false;
    }
}
