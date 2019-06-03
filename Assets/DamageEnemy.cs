using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField]
    HealthComponent health;

    internal void DamageE(int value)
    {
        health.DamageEnemy(value);
    }
}
