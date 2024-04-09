using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    protected Animator animator;
    public Web web;
    public GameObject webSpawner;
    bool onTarget = false;
    bool biting = false;
    protected float demage;

    protected override void Start()
    {
        base.Start();
        this.demage = 3f;
        stunDuration = 0.5f;
        parryStunDurarion = 1.5f;
        knockbackDuration = 0.3f;
        knockbackSpeed = 1.75f;
        animator = GetComponent<Animator>();
        this.speed = Random.Range(8, 12);
        hp = 1;
        target = player.transform;
        //SpeedDown();
        SpiderMovement();
        SetTarget();
        //InvokeRepeating("Shoot", 5f, 3f);
        if (isVariant)
            VariantProperties();


    }

    void Update()
    {
        if (biting || stunned)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (DistanceFromTarget() < 5f)
            {
                onTarget = true;
                //FaceTarget();
                //if (!knockedBackEffect)
                    //MoveForward();
                //else rb.velocity = Vector2.zero;
            }
            else onTarget = false;
        }
       
    }

    void Shoot()
    {

        for (float i = 0; i <= 2; i++)
        {
            Quaternion webRotation = Quaternion.Euler(webSpawner.transform.rotation.x, webSpawner.transform.rotation.y, webSpawner.transform.rotation.z - 12.5f + i * 12.5f);
            Web webToSpawn = Instantiate(web, new Vector3(webSpawner.transform.localPosition.x, webSpawner.transform.localPosition.y + 0.5f - i / 3, -0.5f), webRotation);
            webToSpawn.transform.parent = webSpawner.transform;
            webToSpawn.transform.localPosition = new Vector3(webSpawner.transform.localPosition.x, webSpawner.transform.localPosition.y + 0.5f - i / 3, -0.5f);
            webToSpawn.transform.localRotation = webRotation;
            webToSpawn.transform.parent = null;
        }
    }


    void MoveForward()
    {
        Vector2 forward = transform.right;
        rb.velocity = forward * base.speed;
    }

    void SpiderMovement()
    {
        if (/*!onTarget &&*/ !biting && !knockedBackEffect)
        {
            bool right = transform.position.x < target.position.x;
            bool up = transform.position.y < target.position.y;
            //float f = speed * speed;
            float diognalSpeed = Mathf.Sqrt((base.speed * base.speed) / 2);

            if (right && up)
            {
                rb.velocity = new Vector2(diognalSpeed, diognalSpeed);
            }

            if (right && !up)
            {
                rb.velocity = new Vector2(diognalSpeed, -diognalSpeed);
            }

            if (!right && up)
            {
                rb.velocity = new Vector2(-diognalSpeed, diognalSpeed);
            }

            if (!right && !up)
            {
                rb.velocity = new Vector2(-diognalSpeed, -diognalSpeed);
            }

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;//Making the spider face where it's going
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        }
        else rb.velocity = Vector2.zero;

         Invoke("SpiderMovement", 0.2f);
    }

    int lastCounterHit = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Friendly Weapon" && FindObjectOfType<Player>().IsAnimating())
        {

            if (lastCounterHit < Player.swipeCounter)
                lastCounterHit = Player.swipeCounter;
            else return;

            GotHit(Player.currentDemage, player.weapon.transform, true, 1.75f, 0.25f);

            
        }




    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity && entity.goodEntity)
        {
            entity.GotHit(this.demage, transform, true, knockbackSpeed, knockbackDuration, false, true, 0.5f);
            
            biting = true;
            Invoke("StopBiting", 0.5f);
        }
    }

    void StopBiting()
    {
        biting = false;
    }

    public override void GotHit(float demage, Transform demageSource, bool knockback = true, float knockbackForce = 1f, float knockbackDuration = 0.3f, bool burn = false, bool poison = false, float effectDuration = 0)
        
    {

        base.GotHit(demage, demageSource, knockback, knockbackForce, knockbackDuration, burn, poison, effectDuration);

        Projectile projectile = demageSource.GetComponent<Projectile>();
        if (!projectile)
            SetTarget(demageSource);
        
    }
}
