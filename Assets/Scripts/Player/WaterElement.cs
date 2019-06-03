using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElement : MonoBehaviour
{
	private Transform attackIndicator;
    [SerializeField]
    private LayerMask whatIsEnemies;
    private Animator animator;

    void Start ()
    {
        attackIndicator = transform.parent.transform;
        animator = GetComponent<Animator>();
    }

    public IEnumerator CastWaterPrison ()
    {
        animator.SetTrigger("Cast");
        yield return new WaitForSeconds(1.8f);
        Collider[] colliders = Physics.OverlapSphere(attackIndicator.position, 7.5f, whatIsEnemies);
        int i = 0;
        while (i < colliders.Length)
        {
            colliders[i].GetComponent<EnemyBase>().Froze();
            i++;
        }
    }
}
