using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfVariant : Wolf
{
    protected override void Awake()
    {
        base.Awake();
        isVariant = true;
    }


    protected override void VariantProperties()
    {
        this.biteDemage = 8;
        this.jumpDemage = 12;
        this.chaseDistance = 22f;
        this.speed = 8f;
        this.runningSpeed = 22.5f;
        this.stunDuration = 0.5f;
        this.parryStunDurarion = 1.5f;
        this.biteParryDuration = 1.5f;
        this.jumpParryStunDuration = 3f;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

}
