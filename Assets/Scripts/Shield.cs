using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{

    SpriteRenderer sr;
    Player player;
    SpriteRenderer weaponSR;
    public Image shieldsIndicator;
    public Sprite[] shieldsSprites = new Sprite[4];
    public bool shieldUp;
    public bool parrying;
    bool usedParry;
    int charges;
    List<Enemy> enemiesShielded;
    List<Projectile> projectilesShielded;

    AudioSource audioSource;
    public AudioClip shieldHit, shieldBreak, shieldBurned;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        usedParry = false;
        parrying = false;
        charges = 3;
        enemiesShielded = new List<Enemy>();
        projectilesShielded = new List<Projectile>();
        sr = GetComponent<SpriteRenderer>();
        player = transform.parent.GetComponent<Player>();
        weaponSR = player.weapon.GetComponentInChildren<SpriteRenderer>();

        UpdateShieldUI();
        ShieldDown();
        

    }

    void Update()
    {
        CheckShield();
        
        


    }

    void UpdateShieldUI(bool burned = false)
    {
        

        if (burned)
        {
            shieldsIndicator.color = new Color(0.616f, 0f, 0f);
            Invoke("NormalColor", 0.5f);
        }
        else shieldsIndicator.sprite = shieldsIndicator.sprite = shieldsSprites[charges];
    }

    void NormalColor()
    {
        shieldsIndicator.color = new Color(1, 1, 1);
        shieldsIndicator.sprite = shieldsIndicator.sprite = shieldsIndicator.sprite = shieldsSprites[0];


    }

    void AddCharge()
    {
        
        this.charges++;
        if (charges >= 3)
            charges = 3;
        UpdateShieldUI();
    }

    void CheckShield()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!player.IsAnimating() && !shieldUp && charges > 0)
            {
                ShieldUp();
                
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && shieldUp)
        {
            CallShieldDown(0.5f);
        }
       
    }

    public void CallShieldDown(float seconds)
    {
        Invoke("ShieldDown", seconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesShielded.Add(enemy);
        }

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile)
        {
            projectilesShielded.Add(projectile);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesShielded.Remove(enemy);
        }

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile)
        {
            projectilesShielded.Remove(projectile);
            if (projectile.GetComponent<Fireball>() && shieldUp)
            {
                GotHit(true);
            }
        }


    }

    public bool IsEnemySheilded(Enemy enemy)
    {
        return enemiesShielded.Contains(enemy);
    }

    public bool IsArrowShielded(Projectile projectile)
    {
        return projectilesShielded.Contains(projectile);
    }

    void StopParryWindow()
    {
        parrying = false;
    }



    void ShieldUp()
    {
        parrying = true;
        Invoke("StopParryWindow", 0.2f);
        gotHit = false;
        
        CancelInvoke("AddCharge");
        UpdateShieldUI();
        shieldUp = true;
        sr.color = Game.visible;
        weaponSR.color = Game.invisible;
        player.attackColldier.enabled = false;
        player.shieldColldier.enabled = true;
    }

    bool gotHit = false;

    public void GotHit(bool allCharges)
    {
        if (!gotHit)
        {
            CallShieldDown(1f);
            gotHit = true;

            if (!allCharges)
            {
                this.charges--;
                if (this.charges <= 0)
                {
                    this.charges = 0;
                    audioSource.clip = shieldBreak;
                    audioSource.Play();
                }
                else
                {
                    audioSource.clip = shieldHit;
                    audioSource.Play();
                }
                   
               
            }

            else
            {
                this.charges = 0;
                audioSource.clip = shieldBurned;
                audioSource.Play();
            }
        }


      



        UpdateShieldUI(allCharges);
    }
    public void ShieldDown()
    {
        parrying = false;
        gotHit = false;
        shieldUp = false;
        sr.color = Game.invisible;
        weaponSR.color = Game.visible;
        enemiesShielded.Clear();
        player.attackColldier.enabled = true;
        player.shieldColldier.enabled = false;
        if(!IsInvoking("AddCharge"))
            InvokeRepeating("AddCharge", 4f, 4f );
        UpdateShieldUI();
    }
}
