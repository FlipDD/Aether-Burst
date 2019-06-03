using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AetherPickable : MonoBehaviour
{
    private PlayerInputController playerInputController;
    private Light childLight;
    private Material mat;
    [SerializeField] private Transform aetherBall;
    private bool consumed;
    public int nOfAether = 1;

    void Start ()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        childLight = GetComponentInChildren<Light>();
        mat = GetComponent<Renderer>().material;
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.CompareTag("Player") && !consumed)
        {
            StartCoroutine(DimLights());
            for (int i = 1; i < nOfAether; i++)
            {
                StartCoroutine(ExtraAether());
            }
        }
    }

    IEnumerator DimLights()
    {
        consumed = true;
        Instantiate(aetherBall, transform.position, Quaternion.identity);
        float t = 0;
        float emissionTime = .2f;
        while (t < 2)
        {
            t += Time.deltaTime * 2;
            emissionTime -= Time.deltaTime / 5;
            childLight.intensity = 7 - (t * 3.5f); //7 is initial, 3.5f because it's over 2 seconds
            Color finalColor = Color.yellow * Mathf.LinearToGammaSpace(emissionTime);
            mat.SetColor("_EmissionColor", finalColor);
            yield return null;
        }
    }

    IEnumerator ExtraAether()
    {
        yield return new WaitForSeconds(Random.Range(.3f, 1.3f));
        Instantiate(aetherBall, transform.position, Quaternion.identity);
    }
}
