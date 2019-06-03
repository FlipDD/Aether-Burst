using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocks : MonoBehaviour
{
    [SerializeField]
    private Transform texture;
    private EnemyBehaviour eb;
    private bool breaking;

    [SerializeField] private ParticleSystem jumpPs;

    // void Start() => eb = GetComponentInParent<EnemyBehaviour>();

    // void OnCollisionEnter(Collision col)
    // {
    //     if (col.gameObject.CompareTag("Ground") && eb.GetJumpingState())
    //     {
    //         StartCoroutine(Break(col.GetContact(0).point));

    //         // Instantiate(GameAssets.i.rockShattered, transform.position, Quaternion.identity);
    //         // Instantiate(texture, new Vector3(transform.position.x, transform.position.y - .09f, transform.position.z), Quaternion.Euler(90, 0, 0));
    //         //ADD EXPLOSION FORCE?
    //     }
    // }

    // IEnumerator Break(Vector3 point)
    // {
    //     breaking = true;
    //     jumpPs.transform.parent = null;
    //     jumpPs.transform.position = point;
    //     jumpPs.transform.rotation = Quaternion.identity;
    //     jumpPs.Play();
    //     yield return new WaitForSeconds(3.1f);
    //     jumpPs.transform.parent = transform;
    //     breaking = false;

    // }
}
