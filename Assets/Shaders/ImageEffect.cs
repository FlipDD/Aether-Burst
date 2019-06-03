using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    private float intensity = 1;
    private Material imgEffect;

    void Start() 
    {
        intensity = 1;
        imgEffect = new Material(Shader.Find("Hidden/BWDiffuse"));
        StartCoroutine(DecreaseIntensity(0, 2.5f));
    }

    private IEnumerator DecreaseIntensity(float newIntensity, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        while (intensity > newIntensity)
        {
            intensity -= Time.deltaTime * .4F;
            yield return null;
        }
        intensity = newIntensity;
    }

    internal IEnumerator IncreaseIntensity(float newIntensity, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        while (intensity < newIntensity)
        {
            intensity += Time.deltaTime * .4F;
            yield return null;
        }
        intensity = newIntensity;
    }

    void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (intensity == 0)
		{
			Graphics.Blit (source, destination);
			return;
		}

		imgEffect.SetFloat("_bwBlend", intensity);
		Graphics.Blit (source, destination, imgEffect);
	}

    void SetIntensity(float _intensity) => intensity = _intensity;
}
