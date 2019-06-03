
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets i;

    void Start() => i = this;

    //Particle Systems
    public Transform windEffect;
    public Transform windEffectShoot;
    public Transform fireEffect;
    public Transform fireDash;
    public Transform iceEffect;
    public Transform iceShatter;
    
    public Transform waterPrison;
    public Transform rock;
    public Transform rockShattered;
    public Transform burningEffect;
    public Transform lizardSpikes;

    //Others
    public Transform bullet;
    public Transform shatteredGargoyle;
}
