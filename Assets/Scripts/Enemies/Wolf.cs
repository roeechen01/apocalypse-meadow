using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wolf : Enemy
{
    State currentState;
    protected float chaseDistance;
    protected float runningSpeed;
    float jumpSpeed;
    float jumpDistance;
    protected Animator animator;
    protected float biteDemage;
    protected float jumpDemage;
    protected float jumpParryStunDuration;
    protected float biteParryDuration;

    Vector2 chaseDirection;

    enum State
    {
        Search, Chase, Jump
    }

    protected override void Start()
    {
        base.Start();
        stunDuration = 1f;
        parryStunDurarion = 2.5f;
        biteParryDuration = parryStunDurarion;
        jumpParryStunDuration = 4f;
        animator = GetComponent<Animator>();
        biteDemage = 5;
        jumpDemage = 7;
        this.knockbackSpeed = 2.5f;
        this.knockbackDuration = 0.3f;
        hp = 2f;
        target = player.transform;
        speed = 6.5f;
        runningSpeed = 20f;
        jumpSpeed = 30f;
        chaseDistance = 25f;
        jumpDistance = 25f;
        currentState = State.Search;
        SetTarget();
        chaseDirection = Vector2.zero;
        if (isVariant)
            VariantProperties();
    }


    void Update()
    {

        if (stunned)
        {
            print("stunned");
            rb.velocity = Vector2.zero;
            animator.speed = 0;
            currentState = State.Search;
            return;
        }
        if (currentState == State.Search)
            animator.speed = 1;

        if (currentState == State.Search)
        {
            FaceTarget();
            if (!knockedBackEffect)
                MoveForward();
            else rb.velocity = Vector2.zero;
        }

        if(currentState == State.Chase && (chaseDirection.x != 0 && CloseEnough(transform.position.x, target.position.x, 0.5f)) || (chaseDirection.y != 0 && CloseEnough(transform.position.y, target.position.y, 0.5f)))
        {
            CheckIfJump();
        }

        if (target && DistanceFromTarget() < chaseDistance && CloseEnoughInOneAxis(0.5f) && currentState == State.Search && !chaseCooldown)
        {
            chaseCooldown = true;
            SetChaseDirection();
            StartChase();
        }

        if(currentState == State.Jump && IsInvoking("Jump"))
        {
            FaceTarget(target.position);
        }



    }

    void SetChaseDirection()
    {
        if (CloseEnough(transform.position.x, target.position.x, 0.5f))
        {
            if(transform.position.y > target.position.y)
            {
                chaseDirection = new Vector2(0, -1);
            }
            else
            {
                chaseDirection = new Vector2(0, 1);
            }
        }
        else if (CloseEnough(transform.position.y, target.position.y, 0.5f))
        {
            if (transform.position.x > target.position.x)
            {
                chaseDirection = new Vector2(-1, 0);
            }
            else
            {
                chaseDirection = new Vector2(1, 0);
            }
        }

    }

    bool CloseEnoughInOneAxis(float closeDistance)
    {
        bool close = false;

        close = CloseEnough(transform.position.x,target.position.x , closeDistance);
        if(!close)
            close = CloseEnough(transform.position.y, target.position.y, closeDistance);


        return close;
    }

    void CheckIfJump()
    {
        if(DistanceFromTarget() < jumpDistance && currentState != State.Jump)
        {
            rb.velocity = Vector2.zero;
            StartJump();
            
        }
        else
        {
            rb.velocity = Vector2.zero;
            if(!IsInvoking("EndJump"))
                EndChase();
        }
        
    }

    void StartJump()
    {
        currentState = State.Jump;
        FaceTarget(target.position);
        rb.velocity = Vector2.zero;
        animator.speed = 0;

        Invoke("Jump", 1f);

        Invoke("EndJump", 1.5f);
    }

    void Jump()
    {
        Vector2 direction = (target.position - transform.position).normalized;


        rb.velocity = direction * jumpSpeed;
    }


    void EndJump()
    {
        EndChase();
        animator.speed = 1;
    }


    bool chaseCooldown = false;

    void StartChase()
    {
        if(!knockedBackEffect){
            chaseEndPos = target.position;
            currentState = State.Chase;
            Invoke("ChaseTimer", 3.5f);
            WolfMovement();
        }

        
    }

    void ChaseTimer()
    {
        if (currentState == State.Chase && DistanceFromTarget() > 20f)
            EndChase();
    }

    protected float cooldownTimer;

    void EndChase()
    {
        cooldownTimer = 3f;
        if(IsInvoking("EndChase"))
            CancelInvoke("EndChase");
        chaseCooldown = true;
        Invoke("ResetChaseCooldown", cooldownTimer);
        currentState = State.Search;
        chaseDirection = Vector2.zero;

    }

    void ResetChaseCooldown()
    {
        chaseCooldown = false;
    }

    int lastCounterHit = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Friendly Weapon" && FindObjectOfType<Player>().IsAnimating())
        {

            if (lastCounterHit < Player.swipeCounter)
                lastCounterHit = Player.swipeCounter;
            else return;


            if(currentState == State.Jump && !IsInvoking("Jump") && !player.shield.shieldUp)
            {
                player.GotHit(jumpDemage, transform, true, 2.5f, 0.4f);
                GotHit(Player.currentDemage, collision.gameObject.transform, true, knockbackSpeed, knockbackDuration);

            }
            else
            {
                GotHit(Player.currentDemage, collision.gameObject.transform, true, knockbackSpeed, knockbackDuration);
            }
            


        }




    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity && entity.goodEntity)
        {
            if (currentState == State.Chase && entity.transform != target)
            {
                parryStunDurarion = biteParryDuration;
                entity.GotHit(biteDemage, transform, true, 1.5f, 0.35f);
            }

            if (currentState == State.Chase && entity.transform == target)
            {
                parryStunDurarion = biteParryDuration;
                entity.GotHit(biteDemage, transform, true, 1.5f, 0.3f);
                EndChase();
            }

            if(currentState == State.Jump && !IsInvoking("Jump") && entity.transform != target)
            {
                parryStunDurarion = jumpParryStunDuration;
                entity.GotHit(jumpDemage, transform, true, 2.5f, 0.4f);
            }

            if (currentState == State.Jump && !IsInvoking("Jump") && entity.transform == target)
            {
                parryStunDurarion = jumpParryStunDuration;
                entity.GotHit(jumpDemage, transform, true, 2.5f, 0.4f);
                CancelInvoke("EndJump");
                EndJump();
                EndChase();
            }
        }

        if (collision.tag == "EnemyWall")
            HandleWall();
    }

    void HandleWall()
    {
       if(currentState == State.Chase)
        {
            CheckIfJump();
        } else

        if (currentState == State.Jump)
        {
            rb.velocity = Vector2.zero;
            EndJump();
        }
    }

  
    void MoveForward()
    {
            Vector2 forward = transform.right;
            rb.velocity = forward * speed;
    }

    Vector2 chaseEndPos;
    Vector2 lastVelocity;
    int counter = 0;

    void WolfMovement()
    {
        if (!knockedBackEffect)
        {
            rb.velocity = chaseDirection * runningSpeed;

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else rb.velocity = Vector2.zero;
    }

    public override void GotHit(float demage, Transform demageSource, bool knockback = true, float knockbackSpeed = 1, float knockbackDuration = 0.3F, bool burn = false, bool poison = false, float effectDuration = 0)
    {
        if (isVariant)
        {
            cooldownTimer = 1.1f;
        }
        base.GotHit(demage, demageSource, knockback, knockbackSpeed, knockbackDuration, burn, poison, effectDuration);
        
        if (IsInvoking("Jump"))
            CancelInvoke("Jump");
        if (IsInvoking("EndJump"))
        {
            CancelInvoke("EndJump");
            EndJump();
        }
        cooldownTimer = 3f;
        currentState = State.Search;
    }

    protected override void SetTarget()
    {

        target = player.transform;
        //Scene scene = SceneManager.GetActiveScene();
        //GameObject[] allGameObjects = scene.GetRootGameObjects();
        //List<Entity> allies = new List<Entity>();
        //for (int i = 0; i < allGameObjects.Length; i++)
        //{
        //    Entity entity = allGameObjects[i].GetComponent<Entity>();


        //    if (entity && goodEntity != entity.goodEntity)
        //        allies.Add(entity);

        //}

        //float minDistance;
        //Transform minTransform;


        //minDistance = Vector2.Distance(transform.position, allies[0].transform.position);
        //minTransform = allies[0].transform;



        //for (int i = 0; i < allies.Count; i++)
        //{
        //    Entity ally = allies[i];
        //    float distance = Vector2.Distance(transform.position, ally.transform.position);
        //    if (distance < minDistance)
        //    {
        //        minDistance = distance;
        //        minTransform = allies[i].transform;
        //    }
        //}

        //if (/*Random.Range(1, 4) != 3*/true)
        //{
        //    target = minTransform;
        //}
        Invoke("SetTarget", Random.Range(4.2f, 8.4f));

    }


}

