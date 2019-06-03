using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class UIBar : MonoBehaviour
{
    [SerializeField]
    private Slider breathSlider;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Image barImg;
    [SerializeField]
    private GameObject loseState;

    private TMPro.TMP_Text loseText;
    private Image loseImg;

    internal bool isChanging;
    internal bool isWaiting;
    private bool invincible;
    private Camera cam;
    private ImageEffect imgfx;

    private PlayerInputController playerScript;
    AudioManager audioManager;

    bool canPlay;

    void Start()
    {
        loseText = loseState.GetComponent<TMPro.TMP_Text>();
        loseImg = loseState.GetComponentInChildren<Image>();
        cam = Camera.main;
        imgfx = cam.GetComponent<ImageEffect>();
        playerScript = GetComponent<PlayerInputController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    internal void UpdateHp(float value)
    {
        if (!invincible)
        {
            healthSlider.value += value;
            if (value < 0)
                AudioManager.i.Play("Hurt", transform.position);
            // playerScript.CameraShaker3(1f, .01f, .01f);
            if (healthSlider.value <= 0)
                StartCoroutine(LoseState());
            StartCoroutine(InvincibilityWindow());
        }
    }

    public void DecreaseBreath(float value)
	{
        if (value > breathSlider.value && !isChanging)
        {
            StartCoroutine(ChangeColor(2f, .15f, 20));
            //TODO----------------
            //make it 0 for a bit?
        }
        else
        {
            float tmpValue = breathSlider.value;
		    breathSlider.value -= value;
            if (tmpValue > breathSlider.value)
            {
                StopCoroutine("WaitToUpdateBreath");
                StartCoroutine("WaitToUpdateBreath");
            }
        }
	}

    public IEnumerator ChangeColor(float duration, float blinkTime, float nextCost)
	{
		isChanging = true;

		while (duration > 0f) 
		{
            duration -= Time.deltaTime;
            barImg.color = new Color(1, .2f, .2f);
            yield return new WaitForSeconds(blinkTime);
         	barImg.color = new Color(.59f, 1, .32f);
		 	yield return new WaitForSeconds(blinkTime);
			if (breathSlider.value > nextCost) duration = 0;
         }

		isChanging = false;
	}

    public IEnumerator WaitToUpdateBreath()
	{
        isWaiting = true;
		yield return new WaitForSeconds(.75f);
        isWaiting = false;
	}


    private IEnumerator InvincibilityWindow()
    {
        invincible = true;
        yield return new WaitForSeconds(.5f);
        invincible = false;
    }

    public float GetBreathValue() => breathSlider.value;

    public float GetHealthValue() => healthSlider.value;

    internal IEnumerator LoseState()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Death");
        gameObject.GetComponent<PlayerInputController>().enabled = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        FindObjectOfType<GroundBoolean>().enabled = false;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        transform.RotateAround(transform.position, Vector3.right, 90);
        gameObject.GetComponent<Rigidbody>().mass = 1000000;
        
        Transform sword = FindObjectOfType<KnockBack>().transform;
        sword.parent = null;
        Rigidbody sr = sword.GetComponent<Rigidbody>();
        sr.useGravity = true;
        sr.constraints = RigidbodyConstraints.None;
        StartCoroutine(imgfx.IncreaseIntensity(1));
        float t = 0;
        for (t = 0; t < 6; t += Time.deltaTime)
        {
            loseText.color = new Color(1, .2f, .2f, Mathf.Lerp(0, .8f, t/2.5f));
            loseImg.color = new Color(.7f, .4f, .4f, Mathf.Lerp(0, .3f, t/3.5f));
            yield return null;
        }
        // yield return new WaitForSeconds(4);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
