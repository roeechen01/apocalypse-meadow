using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Archer : Ally
{
    Animator animator;
    public Arrow arrow;
    public Transform spawner;

    bool reloaded = true;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        speed = 3.5f;
        hp = 10000f;
        target = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", 5f, 5f);
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (stunned)
        {
            rb.velocity = Vector2.zero;
        }
        FaceTarget();
        if (target && Vector2.Distance(transform.position, target.position) < 18f && Vector2.Distance(transform.position, target.position) > 8f)
            rb.velocity = Vector2.zero;
        else
        {
            if (!stunned)
                MoveForward();
            else rb.velocity = Vector2.zero;
        }
        if (target && Vector2.Distance(transform.position, target.position) < 5f)
            rb.velocity = Vector2.zero;
    }

    public void ReloadAnimation()
    {
        Arrow singleArrow = Instantiate(arrow, spawner.transform.position, transform.rotation);
        singleArrow.shooter = transform;
        animator.SetTrigger("Reload");
    }

    public void Reloaded()
    {
        reloaded = true;
        animator.SetTrigger("Idle");
    }

    void Shoot()
    {
        reloaded = false;
        animator.SetTrigger("Shoot");
    }

    protected void FaceTarget(Vector3 angleToLook = default)
    {
        if (target != null || angleToLook != default)
        {
            if (angleToLook == default)
                angleToLook = target.position;
            Vector2 direction = angleToLook - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            rb.rotation = angle + 270;
        }
    }

    void MoveForward()
    {
        Vector2 forward = transform.up;
        rb.velocity = forward * speed;
    }

    override protected void SetTarget()
    {
        float prefferedDistance = 16f;
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
            if (IsCloserToDistnace(prefferedDistance, minDistance, distance))
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
}
