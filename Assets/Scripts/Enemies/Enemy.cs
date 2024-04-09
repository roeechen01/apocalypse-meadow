using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : AiEntity
{

    protected bool isVariant = false;
    public HealingPotion hpDrop;
    

    // Start is called before the first frame update
    virtual protected void Awake()
    {
        
        transform.localScale *= 1.3f;
        EntityStart();
        goodEntity = false;
        dieAfterKnockback = false;
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
        target = player.transform;
    }

    protected virtual void VariantProperties()
    {

    }



    public override void Death()
    {
        if (GetComponent<Zombie>())
        {
            if(Random.Range(0f, 1f) <= 0.1f)
            {
                Instantiate(hpDrop, transform.position, Quaternion.identity);
            }
        } else 

        if (GetComponent<Spider>())
        {
            if (Random.Range(0f, 1f) <= 0.15f)
            {
                Instantiate(hpDrop, transform.position, Quaternion.identity);
            }
        }
        if (GetComponent<Wolf>())
        {
            if (Random.Range(0f, 1f) <= 0.15f)
            {
                Instantiate(hpDrop, transform.position, Quaternion.identity);
            }
        }

        if (GetComponent<Witch>())
        {
            if (Random.Range(0f, 1f) <= 0.3f)
            {
                Instantiate(hpDrop, transform.position, Quaternion.identity);
            }
        }

        base.Death();
    }






    protected void FaceTarget(Vector3 angleToLook = default)
    {
        if (target != null || angleToLook != default)
        {
            if (angleToLook == default)
                angleToLook = target.position;
            Vector2 direction = angleToLook - transform.position;
            // Calculate the rotation angle using Vector2.SignedAngle
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            rb.rotation = angle;
        }
    }
}
