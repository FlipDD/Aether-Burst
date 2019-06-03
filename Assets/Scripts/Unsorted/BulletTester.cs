using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTester : MonoBehaviour
{
    public float speed = 6;
    private PlayerMovement playerMovement;
    void Start ()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.Impulse);
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void OnTriggerStay (Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody rigidbody = col.gameObject.GetComponent<Rigidbody>();
            col.GetComponent<PlayerInputController>().hasJumped = true;
            playerMovement.acc = 0;
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            rigidbody.AddForce(Vector3.up * 900, ForceMode.Impulse);
            gameObject.SetActive(false);
        }
    }
}