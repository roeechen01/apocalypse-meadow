using UnityEngine;

public class Witch : Enemy
{
    enum State
    {
        Stay, Move, Attack
    }

    public Sprite stayingSprite, movingSprite, attackingSprite;
    public Fireball fireball;
    public GameObject fireballSpawner;



    State currentState;

    

    // Start is called before the first frame update
    protected override void Start()
    {
        stunDuration = 1f;
        parryStunDurarion = 0f;
        base.Start();
        hp = 2;
        speed = 12;
        knockbackDuration = 0.35f;
        knockbackSpeed = 1.8f;
        target = player.transform;
        SetTarget();
        StartMove();
        InvokeRepeating("ShootFireball", 4, Random.Range(3, 5));
        if (isVariant)
            VariantProperties();


    }


    // Update is called once per frame
    void Update()
    {
        if (stunned)
        {
            rb.velocity = Vector2.zero;
            sr.sprite = attackingSprite;
            return;
        }
        switch (currentState)
        {
            case State.Stay: StayState(); break;
            case State.Move:
                MoveState(); break;
            case State.Attack: AttackState(); break;
        }




        if (currentState == State.Move && Game.IsDistanceInsignificant(transform.position, goal, 4f))
        {
            CancelInvoke("StartAttack");
            StartAttack();
        }







        UpdateSpawnerRotation();
    }

    void StartMove()
    {
        CancelInvoke("FlyToNextPos");
        currentState = State.Move;
        sr.sprite = movingSprite;
        Vector2 forward = transform.right;
        rb.velocity = forward * speed;

        FlyToNextPos();
    }

    void StartAttack()
    {
        currentState = State.Attack;
            stopping = true;
            sr.sprite = attackingSprite;
            rb.velocity = Vector2.zero;
        if(!IsInvoking("FlyToNextPos"))
            Invoke("FlyToNextPos", 10f);
    }

    void MoveState()
    {
        Vector2 forward = transform.right;
        rb.velocity = forward * speed;
        sr.sprite = movingSprite;
    }

    bool stopping = false;
    void AttackState()
    {
        sr.sprite = attackingSprite;
        rb.velocity = Vector2.zero;
        FaceTarget();
    }

    void StayState()
    {
        rb.velocity = Vector2.zero;

    }

    void ShootSingleFireball()
    {
            Fireball fb = Instantiate(fireball, new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y, 0), fireballSpawner.transform.rotation);
            fb.transform.parent = fireballSpawner.transform;
            fb.transform.localPosition = new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y, 0);
            fb.transform.parent = null;
            fb.shooter = transform;
            //fb.ChangeRotation();
    }

    void ShootFireball()
    {

        if (DistanceFromTarget() < 35 && !stunned)
        {
            int rnd = Random.Range(1, 11);
            if (!isVariant) //Normal Witch Fire
            {
                if (rnd < 5)//Shoot 1
                {
                    if (isVariant && Random.Range(1, 4) == 3)
                        return;
                    ShootSingleFireball();
                }

                else if (rnd <= 8)
                {
                    if (isVariant && Random.Range(1, 4) == 3)
                        return;
                    for (float i = 0; i <= 2; i++) //2 Rotated
                    {
                        if (i == 1)
                            continue;
                        Quaternion fireballRotation = Quaternion.Euler(fireballSpawner.transform.localRotation.x, fireballSpawner.transform.localRotation.y, fireballSpawner.transform.rotation.z - 10f + i * 10f);
                        Fireball fireballToSpawn = Instantiate(fireball, new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y - 1 + i, fireball.transform.position.z), fireballRotation);
                        fireballToSpawn.transform.parent = fireballSpawner.transform;
                        fireballToSpawn.transform.localPosition = new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y - 1 + i, fireball.transform.position.z);
                        fireballToSpawn.transform.localRotation = fireballRotation;
                        fireballToSpawn.transform.parent = null;
                    }
                }

                else
                {
                    if(isVariant && Random.Range(1, 4) == 3) 
                    return;

                    for (float i = 0; i <= 2; i++) //3 Rotated
                    {

                        Quaternion fireballRotation = Quaternion.Euler(fireballSpawner.transform.localRotation.x, fireballSpawner.transform.localRotation.y, fireballSpawner.transform.rotation.z - 15f + i * 15f);
                        Fireball fireballToSpawn = Instantiate(fireball, new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y -1 + i, fireball.transform.position.z), fireballRotation);
                        fireballToSpawn.transform.parent = fireballSpawner.transform;
                        fireballToSpawn.transform.localPosition = new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y -1 + i, fireball.transform.position.z);
                        fireballToSpawn.transform.localRotation = fireballRotation;
                        fireballToSpawn.transform.parent = null;
                    }

                }
            }
            else //Variant shoot types
            {
                if (rnd <= 2) 
                {
                    if (isVariant && Random.Range(1, 4) == 3) //Shoot 3 Crossed
                        return;
                    for (int i = 0; i <= 2; i++)
                    {
                        if (i == 1)
                            ShootSingleFireball();
                        else
                        {
                            Quaternion fireballRotation = Quaternion.Euler(fireballSpawner.transform.localRotation.x, fireballSpawner.transform.localRotation.y, fireballSpawner.transform.localRotation.z + 15f + i * -15f);
                            Fireball fireballToSpawn = Instantiate(fireball, new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y - 1 + i, fireball.transform.position.z), fireballRotation);
                            fireballToSpawn.transform.parent = fireballSpawner.transform;
                            fireballToSpawn.transform.localPosition = new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y - 1 + i, fireball.transform.position.z);
                            fireballToSpawn.transform.localRotation = fireballRotation;
                            fireballToSpawn.transform.parent = null;
                        }
                        
                    }
                }

                else if (rnd <= 8)
                {
                    if (isVariant && Random.Range(1, 4) == 3)
                        return;

                    for (float i = 0; i <= 2; i++) //2 Rotated
                    {
                        if (i == 1)
                            continue;
                        Quaternion fireballRotation = Quaternion.Euler(fireballSpawner.transform.localRotation.x, fireballSpawner.transform.localRotation.y, fireballSpawner.transform.rotation.z - 12f + i * 12f);
                        Fireball fireballToSpawn = Instantiate(fireball, new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y - 1 + i, fireball.transform.position.z), fireballRotation);
                        fireballToSpawn.transform.parent = fireballSpawner.transform;
                        fireballToSpawn.transform.localPosition = new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y - 1 + i, fireball.transform.position.z);
                        fireballToSpawn.transform.localRotation = fireballRotation;
                        fireballToSpawn.transform.parent = null;
                    }

                    //for (int i = 0; i <= 2; i++)//Shoot 2 crossed
                    //{
                    //    if (i == 1)
                    //        continue;
                    //    Quaternion fireballRotation = Quaternion.Euler(fireballSpawner.transform.localRotation.x, fireballSpawner.transform.localRotation.y, fireballSpawner.transform.localRotation.z + 15f + i * -15f);
                    //    Fireball fireballToSpawn = Instantiate(fireball, new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y + i - (i / 3), fireball.transform.position.z), fireballRotation);
                    //    fireballToSpawn.transform.parent = fireballSpawner.transform;
                    //    fireballToSpawn.transform.localPosition = new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y + i - (i / 3), fireball.transform.position.z);
                    //    fireballToSpawn.transform.localRotation = fireballRotation;
                    //    fireballToSpawn.transform.parent = null;
                    //}
                }
                else
                {
                    if (isVariant && Random.Range(1, 4) == 3)
                        return;
                    for (int i = 0; i <= 3; i++)//Shoot 4 wall
                    {
                        Fireball fb = Instantiate(fireball, new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y + 1.5f - i, -0.5f), fireballSpawner.transform.rotation);
                        fb.transform.parent = fireballSpawner.transform;
                        fb.transform.localPosition = new Vector3(fireballSpawner.transform.localPosition.x, fireballSpawner.transform.localPosition.y + 1.5f - i, -0.5f);
                        fb.transform.parent = null;
                        fb.shooter = transform;
                        print("dsfv");
                        fb.partOfFireWall = true;
                        
                    }
                }
            }



        }
    }



    int lastCounterHit = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Friendly Weapon" && FindObjectOfType<Player>().IsAnimating())
        {
            if (lastCounterHit < Player.swipeCounter)
                lastCounterHit = Player.swipeCounter;
            else return;
            GotHit(Player.currentDemage, collision.gameObject.transform, true, knockbackSpeed, knockbackDuration);

           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWall") && currentState == State.Move)
        {
            HandleWall();
        }
    }

    void HandleWall()
    {
        if (IsInvoking("StartMove"))
            CancelInvoke("StartMove");
        StartMove();
    }

    void UpdateSpawnerRotation()
    {

        if (target != null)
        {
            // Get the direction from this GameObject to the target
            Vector3 direction = target.position - transform.position;
             
            // Calculate the angle in radians
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Create a rotation quaternion
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Apply the rotation to this GameObject
            fireballSpawner.transform.rotation = rotation;
        }

    }

    protected override void TargetChanged()
    {
       
    }

    Vector3 FindNextPos()
    {
        
        bool validPos = false;
        Vector3 pos = new Vector3(0, 0, 0);
        int tries = 0;
        while (!validPos)
        {
            pos = new Vector3(Random.Range(Game.minX, Game.maxX), Random.Range(Game.minY, Game.maxY), transform.position.z);
            if (target)
            {
                float distanceFromTarget = Vector2.Distance(pos, target.position);
                float distanceFromItself = Vector2.Distance(pos, transform.position);
                if (distanceFromTarget <= 25 && distanceFromTarget > 18f && distanceFromItself > 12f)
                {
                    validPos = true;
                }
                else
                {
                    tries++;
                    if (tries > 25000) //In case of unexpected exception, lower the requirements
                    {
                        print("weird..");
                        if (distanceFromTarget < 35 && distanceFromTarget > 15f && distanceFromItself > 5f)
                        {
                            validPos = true;
                        }
                    }
                }
            }
           
            
        }
        return pos;
    }

    Vector2 goal;
    void FlyToNextPos()
    {

            Invoke("StartAttack", 8f);
           
            currentState = State.Move;
            Vector3 nextPos = FindNextPos();
            FaceTarget(nextPos);
            goal = nextPos;
        if(stopping)
            stopping = false;


    }

    void FlyToNextPos(bool force)
    {
        if (stopping || force)
        {
            stopping = false;
            currentState = State.Move;
            Vector3 nextPos = FindNextPos();
            FaceTarget(nextPos);
            goal = nextPos;
        }


    }

   

    public override void GotHit(float demage, Transform demageSource, bool knockback = true, float knockbackForce = 1f, float knockbackDuration = 0.3f, bool burn = false, bool poison = false, float effectDuration = 0)
    {
        if (currentState == State.Move)
        {
            rb.velocity = Vector2.zero;
            currentState = State.Stay;
            base.GotHit(demage, demageSource, knockback, knockbackForce, knockbackDuration);
            Invoke("StartMove", knockbackDuration);
        }

        else
        {
            rb.velocity = Vector2.zero;
            base.GotHit(demage, demageSource, knockback, knockbackForce, knockbackDuration);
            currentState = State.Stay;
            Invoke("StartMove", knockbackDuration);
        }
           

        Projectile projectile = demageSource.GetComponent<Arrow>();
        if (projectile && projectile.shooter)
            SetTarget(projectile.shooter);
        else
            SetTarget(demageSource);
    }

    protected override void SetTarget()
    {
        if(currentState == State.Attack)
            base.SetTarget();
    }


}
