using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieVariant : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        isVariant = true;
    }


    protected override void VariantProperties()
    {
        this.hp = 4;
        this.speed = Random.Range(10, 15);
        this.knockbackSpeed = 3;
        this.stunDuration = 1f;
        this.parryStunDurarion = 2f;
        this.demage = 7f;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

}
