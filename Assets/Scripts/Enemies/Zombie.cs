using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    protected Animator animator;
    
    
    protected float demage = 5f;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        hp = 2;
        stunDuration = 1.5f;
        parryStunDurarion = 3f;
        knockbackSpeed = 2f;
        knockbackDuration = 0.4f;
        target = player.transform;
        speed = Random.Range(6, 11);
        print(audioSource);
        SetTarget();
        ZombieMovement();
        if (isVariant)
            VariantProperties();
    }


    void Update()
    {

        if (Game.IsDistanceInsignificant(transform.position, target.transform.position, 0.7f) || stunned)
        {
            rb.velocity = Vector2.zero;
        }

        //print(stunned);
        animator.speed = stunned ? 0 : 1;
        
            
    }

    int lastCounterHit = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Friendly Weapon" && FindObjectOfType<Player>().IsAnimating())
        {
            if (lastCounterHit < Player.swipeCounter)
                lastCounterHit = Player.swipeCounter;
            else return;
            
            GotHit(Player.currentDemage, player.transform, true, knockbackSpeed, knockbackDuration);

           
        }

       


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity && entity.goodEntity && !stunned)
        {
            entity.GotHit(demage, transform, true, knockbackSpeed, knockbackDuration);
            
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CancelInvoke("DecreasePlayerHealthByTouch");
        }
    }*/



    void ZombieMovement()
    {
        if (stunned)
        {
            rb.velocity = Vector2.zero;
            Invoke("ZombieMovement", 0.3f);
            return;
        }
        if (/*!onTarget &&*/  !knockedBackEffect )
        {
            bool right = transform.position.x < target.position.x;
            bool up = transform.position.y < target.position.y;
            float closeDistance = 1.5f;
            //float f = speed * speed;
            //float diognalSpeed = Mathf.Sqrt((speed * speed) / 2);

            if (right && up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, target.position.x, closeDistance))
                        rb.velocity = new Vector2(0, speed);
                    else
                        rb.velocity = new Vector2(speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, target.position.y, closeDistance))
                        rb.velocity = new Vector2(speed, 0);
                    else rb.velocity = new Vector2(0, speed);
                }

            }

            if (right && !up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, target.position.x, closeDistance))
                        rb.velocity = new Vector2(0, -speed);
                    else
                        rb.velocity = new Vector2(speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, target.position.y, closeDistance))
                        rb.velocity = new Vector2(speed, 0);
                    else rb.velocity = new Vector2(0, -speed);
                }
            }

            if (!right && up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, target.position.x, closeDistance))
                        rb.velocity = new Vector2(0, speed);
                    else
                        rb.velocity = new Vector2(-speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, target.position.y, closeDistance))
                        rb.velocity = new Vector2(-speed, 0);
                    else rb.velocity = new Vector2(0, speed);
                }
            }

            if (!right && !up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, target.position.x, closeDistance))
                        rb.velocity = new Vector2(0, -speed);
                    else
                        rb.velocity = new Vector2(-speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, target.position.y, closeDistance))
                        rb.velocity = new Vector2(-speed, 0);
                    else rb.velocity = new Vector2(0, -speed);
                }
            }

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;//Making the spider face where it's going
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        }
        else rb.velocity = Vector2.zero;

        Invoke("ZombieMovement", 0.1f);




    }

   



    /*void DecreasePlayerHealthByTouch()
    {
        player.GotHit(1, transform, false);
    }*/

    void MoveForward()
    {
        Vector2 forward = transform.right;
        rb.velocity = forward * speed;
    }
}

