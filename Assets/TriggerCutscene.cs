using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutscene : MonoBehaviour
{
    Animator anim;
    Vector3 originalPos;
    // Transform parent;
    [SerializeField] AudioSource music;

    void Start()
    {
        anim = FindObjectOfType<CameraCollision>().GetComponent<Animator>();
        originalPos = anim.transform.localPosition;
        // parent = anim.transform.parent;
    } 

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) 
        {
            // anim.transform.parent = null;
            anim.transform.parent.GetComponent<CameraBehaviour>().playingCutscene = true;
            // anim.transform.localposition = new Vector3(0, 0, 0);
            anim.GetComponentInParent<CameraBehaviour>().playingCutscene = true;
            // anim.transform.parent = null;  
            anim.GetComponent<CameraCollision>().playingCutscene = true;

            anim.SetTrigger("Cutscene");
            music.Play();
            GetComponent<BoxCollider>().enabled = false;
            anim.transform.parent.position = Vector3.zero;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SetParent();
    }

    void SetParent()
    {
        // anim.transform.parent = parent;
        anim.transform.localPosition = originalPos;
        anim.GetComponent<CameraCollision>().playingCutscene = false;
        anim.GetComponentInParent<CameraBehaviour>().playingCutscene = false;
        anim.SetTrigger("Cutscene");
    }
}
