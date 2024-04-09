using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonVariant : Skeleton
{
    protected override void Awake()
    {
        base.Awake();
        isVariant = true;
    }


    protected override void VariantProperties()
    {
        this.hp = 2;
        this.speed = 5f;
    }
}
