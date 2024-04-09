using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchVariant : Witch
{
    protected override void Awake()
    {
        base.Awake();
        isVariant = true;


        
    }


    protected override void VariantProperties()
    {
        this.hp = 3;
        this.speed = 14f;
    }
}
