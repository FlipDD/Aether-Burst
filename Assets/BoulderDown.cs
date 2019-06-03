using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderDown : MonoBehaviour
{
    private GameObject boulderholder;

    // Start is called before the first frame update
    void Start()
    {

        boulderholder = GameObject.Find("BoulderHolder");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            boulderholder.GetComponent<Collider>().enabled = false;
            boulderholder.GetComponent<MeshRenderer>().enabled = false;
            boulderholder.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

}
