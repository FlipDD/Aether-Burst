﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardTail : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<UIBar>().UpdateHp(-10);
    }
}