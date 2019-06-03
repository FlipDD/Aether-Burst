using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreIcon : MonoBehaviour
{
    private GameObject player;
    public SpriteRenderer sprite;
    public bool enablesprite;

    void Start()
    {
        player = GameObject.Find("Player");
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.TransformDirection(player.transform.position.x, 0, player.transform.position.z);
        //transform.LookAt(player.transform);
        if (player.GetComponent<PlayerInputController>().targetlock == false)
        {
            sprite.enabled = false;
        }
        if (enablesprite == true)
        {
            sprite.enabled = true;
            enablesprite = false;
        }
    }
}
