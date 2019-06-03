    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private Slider slider;
    private EnemyBase enemyBase;
    private GameObject playerOb;
    private PlayerInputController player;

    void Start()
    {
        slider.maxValue = health;
        enemyBase = GetComponent<EnemyBase>();
        playerOb = GameObject.Find("Player");
        player = playerOb.GetComponent<PlayerInputController>();
    }

    internal void DamageEnemy(int dmg)
    {
        if(!enemyBase.isInvincible || enemyBase.takingDamage)
        {
            health -= dmg;
            slider.value = health;
            if (health <= 0) 
            {
                player.UpdateAether(Random.Range(1, 4));
                if (gameObject.name.Contains("Gargoyle"))
                {
                    // gameObject.transform.parent.GetChild(0).gameObject.SetActive(false);
                    Transform tf = Instantiate(GameAssets.i.shatteredGargoyle, transform.position, Quaternion.identity);
                    GameObject parent = this.transform.parent.gameObject;
                    tf.rotation = Quaternion.LookRotation(transform.forward);
                    parent.SetActive(false);
                    gameObject.SetActive(false);
                    player.numberOfEnemies--;

                }
                else
                    gameObject.SetActive(false);


            }
            if (gameObject.activeInHierarchy && !enemyBase.takingDamage) StartCoroutine(enemyBase.Invicibility());
        }

    }
}
