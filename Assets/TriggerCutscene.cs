using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutscene : MonoBehaviour
{
    Animator anim;
    Vector3 originalPos;
    // Transform parent;

    void Start()
    {
        anim = FindObjectOfType<CameraBehaviour>().GetComponent<Animator>();
        originalPos = anim.transform.localPosition;
        // parent = anim.transform.parent;
    } 

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) 
        {
            // anim.transform.parent = null;
            // anim.transform.GetComponent<CameraBehaviour>().playingCutscene = true;
            // anim.transform.localposition = new Vector3(0, 0, 0);
            anim.GetComponent<CameraBehaviour>().playingCutscene = true;
            // anim.transform.parent = null;  
            anim.GetComponentInChildren<CameraCollision>().playingCutscene = true;

            anim.SetTrigger("Cutscene");
            AudioManager.i.PlayBackground("Explore3");  
            GetComponent<BoxCollider>().enabled = false;
            // anim.transform.parent.position = Vector3.zero;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SetParent();
    }

    internal void SetParent()
    {
        // anim.transform.parent = parent;
        // anim.transform.localPosition = originalPos;
        anim.GetComponent<CameraBehaviour>().playingCutscene = false;
        anim.GetComponentInChildren<CameraCollision>().playingCutscene = false;
        anim.SetTrigger("Cutsceneover");
        Destroy(gameObject);
    }
}
