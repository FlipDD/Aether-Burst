using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponentBoss : MonoBehaviour
{
    [SerializeField]
    internal int health;
    [SerializeField]
    internal Slider slider;
    private EnemyBase enemyBase;
    private JorgeController jorge;

    void Start()
    {
        slider.maxValue = health;
        enemyBase = GetComponent<EnemyBase>();
        jorge = GetComponent<JorgeController>();
    }

    internal void DamageEnemy(int dmg)
    {
        if(!enemyBase.isInvincible || enemyBase.takingDamage)
        {
            health -= dmg;
            slider.value = health;
            if (health <= 0) 
            {
                // gameObject.SetActive(false);
                jorge.StopAllCoroutines();
                StartCoroutine(jorge.WinScreen());
            }
            else if (jorge.firstPhase && health <= slider.maxValue / 3)
            {
                jorge.StopAllCoroutines();
                StartCoroutine(jorge.StartSecondPhase());
            }


            if (gameObject.activeInHierarchy && !enemyBase.takingDamage) StartCoroutine(enemyBase.Invicibility());
        }

    }
}
