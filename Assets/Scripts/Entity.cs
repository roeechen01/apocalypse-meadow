using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float hp;
    protected float speed;
    public bool goodEntity;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected bool stunned;
    protected bool poisned;
    //protected bool burned;
    protected bool invincibility;
    protected float effectDuration;
    bool affectedByBlueFire;

    protected AudioSource audioSource;
    public AudioClip gotHitClip, deathClip;

   

    protected bool dieAfterKnockback;
    protected bool variantPoison;

    protected float knockbackSpeed;
    public float knockbackDuration = 0.3f;
    protected float stunDuration;
    protected float parryStunDurarion;


    // Start is called before the first frame update
    virtual protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        parryStunDurarion = 3f;
        stunDuration = 1.5f;
        stunned = false;
        poisned = false;
        //burned = false;
        invincibility = false;
        variantPoison = false;

    }


    private void FixedUpdate()
    {
        if (knockedBackEffect)
            ApplyKnockback();
    }

  

    protected void EntityStart()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    Color colorBeforeFlash;
    void ApplyPoison()
    {

        colorBeforeFlash = sr.color;
        int poisonDemage = variantPoison ? 4 : 2;
        hp -= poisonDemage;
        if (hp <= 0)
            Death();
        sr.color = new Color(0.1f, 1f, 0.1f);
        Invoke("StopFlash", 0.1f);

    }

    void StopFlash()
    {
        sr.color = colorBeforeFlash;
    }

    void EndPoison()
    {
        CancelInvoke("ApplyPoison");
        CancelInvoke("EndPoison");
        poisned = false;
        variantPoison = false;
    }


    virtual public void GotHit(float demage, Transform demageSource, bool knockback = true, float knockbackSpeed = 1f, float knockbackDuration = 0.3f, bool burn = false, bool poison = false, float effectDuration = 0)
    {
        if (stunned && !GetComponent<Enemy>())
            return;
        Player player = GetComponent<Player>();
        Enemy enemy = demageSource.GetComponent<Enemy>();
        Projectile projectile = demageSource.GetComponent<Projectile>();
        if(player)
        {
            if(enemy && player.shield.shieldUp && player.shield.parrying && player.shield.IsEnemySheilded(enemy))
            {
                ParryHit(enemy, this.transform);

                return;
            }
            else if (enemy && player.shield.shieldUp && player.shield.IsEnemySheilded(enemy))
            {
                player.shield.GotHit(false);
                HitDefended(enemy, this.transform);
                
                return;


            }



            if (projectile && projectile.GetComponent<BoneArrow>() && player.shield.parrying && player.shield.IsArrowShielded(projectile))
            {
                projectile.GetComponent<BoneArrow>().Deflect();
                return;
            }
            else if (projectile && projectile.GetComponent<BoneArrow>() && player.shield.shieldUp && player.shield.IsArrowShielded(projectile))
            {
                Destroy(projectile.gameObject);
                player.shield.GotHit(false);
                return;
            }
            else if(projectile) Destroy(projectile.transform.gameObject);
        }
        this.effectDuration = effectDuration;

        if(invincibility && !GetComponent<Enemy>())
        {
            return;
        }

        audioSource.clip = gotHitClip;
        audioSource.Play();
      
         if (!GetComponent<Enemy>() && invincibility == false)
        {
            hp -= demage;
            if (knockback && !knockedBackEffect)
            {
                StartKnockback(demageSource, knockbackSpeed, knockbackDuration, false, false);
                if (hp <= 0)
                    dieAfterKnockback = true;

            }
            else
            {
                if (hp <= 0)
                    Death();
            }

            if(poison && demageSource.GetComponent<SpiderVariant>())
            {
                variantPoison = true;
            }
            Fireball fb = demageSource.gameObject.GetComponent<Fireball>();
            if (fb )
            {
                if (fb.isVariant)
                {
                    affectedByBlueFire = true;
                }
                else
                {
                    affectedByBlueFire = false;
                }
            }
            InvincibilityStart(effectDuration, burn, poison);
        }

        else
        {
            hp -= demage;
            if (knockback)
            {
                StartKnockback(demageSource, knockbackSpeed, knockbackDuration, true, false);
                if (hp <= 0)
                    dieAfterKnockback = true;

            }
            else
            {
                if (hp <= 0)
                    Death();
            }

        }







    }

    void StunStart(float duraion)
    {
        Invoke("StunEnd", duraion);
        stunned = true;
        print("start");
    }

    void StunEnd()
    {
        if (IsInvoking("StunEnd"))
            CancelInvoke("StunEnd");
        stunned = false;
        print("end");
    }

    void HitDefended(Enemy enemy, Transform demageSource)
    {
        enemy.GotDefended(demageSource);
    }

      void ParryHit(Enemy enemy, Transform demageSource)
    {
        enemy.GotParried(demageSource);
        Player player = demageSource.GetComponent<Player>();
        if (player)
        {
            player.JustParried();
        }
    }


    void GotDefended(Transform demageSource)
    {
        StartKnockback(demageSource, this.knockbackSpeed, this.knockbackDuration, false, false);
        
    }

    void GotParried(Transform demageSource)
    {
        StartKnockback(demageSource, this.knockbackSpeed, this.knockbackDuration, true, true);
    }

    virtual public void Death()
    {
        if (deathClip)
        {
            audioSource.clip = deathClip;
            audioSource.Play();
           
        }
        if(GetComponent<Enemy>())
            Destroy(gameObject, 0.3f);
        Player player = FindObjectOfType<Player>();
        if(player && player.end)
        {
            Invoke("CheckOver", 1.5f);

        }

       
    }

    void CheckOver()
    {
        Player player = FindObjectOfType<Player>();
        if (!CheckEnemies() && player)
        {

            player.Win();
        }

    }

    bool CheckEnemies()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject objects in allObjects)
        {
            if (objects.GetComponent<Enemy>())
            {
                return true;
            }
        }
        return false;
    }

    protected float invincibiltyWindow = 0;
    protected void InvincibilityStart(float effectDuration = 0, bool burn = false, bool poison = false)
    {
        float oldinvIncibiltyWindow = this.invincibiltyWindow;
        if (effectDuration > 0)
            this.invincibiltyWindow = effectDuration;

        if (effectDuration > 0 && (burn || poison))
        {
            invincibility = true;
           // burned = burn;
            if (poison && !poisned)
            {
                poisned = true;
                sr.color = new Color(0.1f, 1f, 0.1f);
                CancelInvoke("ApplyPoison");
                CancelInvoke("EndPoison");
                InvokeRepeating("ApplyPoison", 1f, 1f);
                Invoke("EndPoison", 3.5f);
            }
            if (burn)
            {
                if (affectedByBlueFire)
                {
                    sr.color = new Color(0.15f, 0.15f, 0.7f);
                } else
                sr.color = new Color(1f, 0.1f, 0.1f);
            }
            if (!IsInvoking("InvincibilityEnd"))
            {
                Invoke("InvincibilityEnd", effectDuration);
            }
        }

        else
        {
            if(invincibiltyWindow > 0)
            {
                invincibility = true;
                sr.color = new Color(1f, 1f, 1f, 0.65f);
                if (!IsInvoking("InvincibilityEnd"))
                {
                    Invoke("InvincibilityEnd", invincibiltyWindow);
                }
            }
           
        }

        this.invincibiltyWindow = oldinvIncibiltyWindow;



    }


    protected void InvincibilityEnd()
    {
        sr.color = new Color(1f, 1f, 1f);
        invincibility = false;
    }


    Transform demageSource;
    Vector3 demageSourcePos;
    protected bool knockedBackEffect = false;
    virtual protected void StartKnockback(Transform demageSource, float knockbackSpeed, float knockbackDuraion, bool stun, bool parry)
    {
        if (parry && parryStunDurarion > 0)
        {
            stunned = true;
            StunStart(parryStunDurarion);
        }
        else if (stun && stunDuration > 0)
        {
            stunned = true;
            StunStart(stunDuration);
        }
        else
        {
            stunned = true;
            StunStart(knockbackDuraion);
        }
            
        this.knockbackDuration = knockbackDuraion;
        knockbackForce = 250f * knockbackSpeed;
        knockedBackEffect = true;
        this.demageSource = demageSource;
        this.demageSourcePos = demageSource.position;
        counter = 1;


    }

   
    float knockbackForce = 250f;
    virtual protected void ApplyKnockback()
    {
        counter++;
        if (counter <= knockbackDuration / Time.fixedDeltaTime)
        {
            Vector2 difference = (transform.position - demageSourcePos).normalized;
            Vector2 force = difference * knockbackForce;
            rb.AddForce(force, ForceMode2D.Force);
        }
        else
        {
            knockedBackEffect = false;
            if (dieAfterKnockback)
                Death();
        }


    }

    int counter = 1;



 



}
