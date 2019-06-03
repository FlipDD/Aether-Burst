using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackPlayer : MonoBehaviour
{
    public Transform player;
    public Transform center;

    void FixedUpdate()
    {


        Vector3 direction = transform.parent.position - player.position;
        direction.Normalize();
        float distance = Vector3.Distance(transform.parent.position,player.position);
        if (distance <= 12f && distance >= 0)
        {
            if (player.position.x < transform.parent.position.x)
            {
               
                    transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x , player.position.y, player.position.z) + direction*15f, 2f * Time.deltaTime);
                

            }
            else
            {

                transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x , player.position.y, player.position.z) + direction * 15f, 2f * Time.deltaTime);

            }

        }
        else if (distance < 22f && distance > 12f)
        {   
            if (player.position.x < transform.parent.position.x)
            {

                transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y - 6f, player.position.z), 0.08f * Time.deltaTime);


            }
            else
            {

                transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y - 6f, player.position.z) , 0.5f * Time.deltaTime);

            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y +30f, player.position.z), 1f * Time.deltaTime);
        }

    }

    

   
}
