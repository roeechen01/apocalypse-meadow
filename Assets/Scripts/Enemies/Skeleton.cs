using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skeleton : Enemy
{
    public BoneArrow boneArrow;
    public Transform spawner;
    float shootingDistance;

    // Start is called before the first frame update
    protected override void Start()
    {
        stunDuration = 0f;
        parryStunDurarion = 0f;
        base.Start();
        player = FindObjectOfType<Player>();
        this.knockbackDuration = 0.4f;
        this.knockbackSpeed = 3.5f;
        speed = 3.5f;
        hp = 1f;
        shootingDistance = 22f;
        target = FindObjectOfType<Player>().transform;
        InvokeRepeating("Shoot", 3f, 1.2f);
        SetTarget();
        if (isVariant)
            VariantProperties();
    }


    // Update is called once per frame
    void Update()
    {
        if (stunned)
        {
            rb.velocity = Vector2.zero;
            if (IsInvoking("ZombieMovement"))
            {
                CancelInvoke("ZombieMovement");
            }
        }
        if (target && (Vector2.Distance(transform.position, target.position) < shootingDistance))
        {
            rb.velocity = Vector2.zero;
            if (IsInvoking("ZombieMovement"))
            {
                CancelInvoke("ZombieMovement");
            }

        }

        else
        {
            if (!stunned)
            {
                if (!IsInvoking("ZombieMovement"))
                {
                    ZombieMovement();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
                if (IsInvoking("ZombieMovement"))
                {
                    CancelInvoke("ZombieMovement");
                }
            }
        }

        if (!IsInvoking("ZombieMovement"))
            FaceTarget();
    }

    void Shoot()
    {
        if (target && Vector2.Distance(transform.position, target.position) <= shootingDistance)
        {
            BoneArrow singleArrow = Instantiate(boneArrow, spawner.transform.position, transform.rotation);
            singleArrow.transform.rotation = transform.rotation;
            singleArrow.shooter = transform;
        }

    }

    void MoveForward()
    {
        Vector2 forward = transform.right;
        rb.velocity = forward * speed;
    }

    override protected void SetTarget()
    {
        
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] allGameObjects = scene.GetRootGameObjects();
        List<Entity> availableTargets = new List<Entity>();
        for (int i = 0; i < allGameObjects.Length; i++)
        {
            Entity entity = allGameObjects[i].GetComponent<Entity>();


            if (entity && goodEntity != entity.goodEntity)
                availableTargets.Add(entity);

        }

        float minDistance;
        Transform minTransform;

        if (availableTargets.Count == 0)
            return;
        minDistance = Vector2.Distance(transform.position, availableTargets[0].transform.position);
        minTransform = availableTargets[0].transform;



        for (int i = 0; i < availableTargets.Count; i++)
        {
            Entity ally = availableTargets[i];
            float distance = Vector2.Distance(transform.position, ally.transform.position);
            if (IsCloserToDistnace(shootingDistance, minDistance, distance))
            {
                minDistance = distance;
                minTransform = availableTargets[i].transform;
            }
        }

        if (/*Random.Range(1, 4) != 3*/true)
        {
            target = minTransform;
        }
        Invoke("SetTarget", Random.Range(4.2f, 8.4f));

    }

    bool IsCloserToDistnace(float target, float oldNum, float newNum)
    {
        float oldDifference = Mathf.Abs(target - oldNum);
        float newDifference = Mathf.Abs(target - newNum);
        return oldDifference > newDifference;
    }

    public override void GotHit(float demage, Transform demageSource, bool knockback = true, float knockbackForce = 1f, float knockbackDuration = 0.3f, bool burn = false, bool poison = false, float effectDuration = 0)
    {
        base.GotHit(demage, demageSource, knockback, knockbackForce, knockbackDuration);

        Projectile projectile = demageSource.GetComponent<Projectile>();
        if (projectile && projectile.shooter)
            SetTarget(projectile.shooter);
    }

    int lastCounterHit = 1;
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

    void ZombieMovement()
    {
        if (/*!onTarget &&*/  !knockedBackEffect)
        {
            bool right = transform.position.x < target.position.x;
            bool up = transform.position.y < target.position.y;
            //float f = speed * speed;
            //float diognalSpeed = Mathf.Sqrt((speed * speed) / 2);

            if (right && up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, transform.position.x, 0.15f))
                        rb.velocity = new Vector2(0, speed);
                    else
                        rb.velocity = new Vector2(speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, transform.position.y, 0.15f))
                        rb.velocity = new Vector2(speed, 0);
                    else rb.velocity = new Vector2(0, speed);
                }

            }

            if (right && !up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, transform.position.x, 0.15f))
                        rb.velocity = new Vector2(0, -speed);
                    else
                        rb.velocity = new Vector2(speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, transform.position.y, 0.15f))
                        rb.velocity = new Vector2(speed, 0);
                    else rb.velocity = new Vector2(0, -speed);
                }
            }

            if (!right && up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, transform.position.x, 0.15f))
                        rb.velocity = new Vector2(0, speed);
                    else
                        rb.velocity = new Vector2(-speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, transform.position.y, 0.15f))
                        rb.velocity = new Vector2(-speed, 0);
                    else rb.velocity = new Vector2(0, speed);
                }
            }

            if (!right && !up)
            {
                if (Mathf.Abs(transform.position.x - target.position.x) < Mathf.Abs(transform.position.y - target.position.y))
                {
                    if (CloseEnough(transform.position.x, transform.position.x, 0.15f))
                        rb.velocity = new Vector2(0, -speed);
                    else
                        rb.velocity = new Vector2(-speed, 0);
                }
                else
                {
                    if (CloseEnough(transform.position.y, transform.position.y, 0.15f))
                        rb.velocity = new Vector2(-speed, 0);
                    else rb.velocity = new Vector2(0, -speed);
                }
            }

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;//Making the spider face where it's going
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        }
        else rb.velocity = Vector2.zero;

        Invoke("ZombieMovement", 0.2f);




    }
}
