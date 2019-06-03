using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class GroundBoolean : MonoBehaviour
{
    private UIBar uiBar;
    private PlayerInputController playerInputController;
    public Transform camBase;
    private bool setTrigger;

    void Start()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        uiBar = GetComponentInParent<UIBar>();
    }

    void OnTriggerEnter (Collider col)
    {
        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Enemy")) && !playerInputController.isGrounded)
        {
            playerInputController.animator.SetTrigger("Fallen");
            StopAllCoroutines();
            if (playerInputController.verticalVelocity > 26)
                uiBar.UpdateHp(-(playerInputController.verticalVelocity*4.5f) + (26 * 4.5f));
        }
    }

    void OnTriggerStay (Collider col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Enemy"))
        {
            if (playerInputController.animator.GetCurrentAnimatorStateInfo(0).IsName("falling") && !setTrigger)
            {
                playerInputController.animator.SetTrigger("Fallen");
                setTrigger = true;
            }
            playerInputController.isGrounded = true;
            playerInputController.hasJumped = false;
            playerInputController.animator.SetBool("Grounded", true);
        } 
    }

    void OnTriggerExit (Collider col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Enemy"))
        {
            playerInputController.isGrounded = false;
            playerInputController.hasJumped = true;
            
            playerInputController.animator.SetTrigger("Falling");
            setTrigger = false;
            playerInputController.animator.SetBool("Grounded", false);
        }
    }

    IEnumerator CamReturn()
    {
        Vector3 pos = camBase.transform.localPosition;
        camBase.transform.localPosition = Vector3.Lerp(pos, pos-Vector3.up*.5f, .5f);
        yield return new WaitForSeconds(.05f);
        camBase.transform.localPosition = Vector3.Lerp(camBase.transform.localPosition, pos, .5f);
    }
}
