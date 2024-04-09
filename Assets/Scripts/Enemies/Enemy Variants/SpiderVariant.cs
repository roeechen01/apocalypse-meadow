using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderVariant : Spider
{
    protected override void Awake()
    {
        base.Awake();
        isVariant = true;
    }


    protected override void VariantProperties()
    {
        this.hp = 2;
        this.speed = Random.Range(11, 15);
        this.demage = 6f;
        this.parryStunDurarion = 1f;
        this.stunDuration = 0;
    }
}
