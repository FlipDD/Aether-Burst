using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseArrowTrigger : MonoBehaviour
{

    Vector3 initialpos;
    private Animator anim;
    public GameObject[] arrowspawner;
    internal bool down;
    private Vector3 endPosition = new Vector3(0, -10f, 0);
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        initialpos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "Player" && transform.position.y < initialpos.y + 0.2f)
        {
            anim.SetTrigger("Arrow");

            for (int i = 0; i < arrowspawner.Length; i++)
            {
                arrowspawner[i].GetComponent<ReverseArrowSpawner>().shootarrow = true;
            }





        }
    }
}


