using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneActivator : MonoBehaviour
{
    private FadeInOut fadeInOut;
    private PlayerInputController playerInputController;

    void Start()
    { 
        fadeInOut = FindObjectOfType<FadeInOut>();
        playerInputController = FindObjectOfType<PlayerInputController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            fadeInOut.FadeIn();
            // playerInputController.SetIdle();
        }
    }
}
