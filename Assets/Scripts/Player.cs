using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Entity
{
    Camera mainCamera;
    public bool frozen = false;
    static public int swipeCounter = 0;
    float runningSpeed;
    public Collider2D attackColldier, shieldColldier;

    public Slider staminaBar, hpBar;

    public AudioClip counterClip, parryClip, attackClip;


    Vector3 lastPos; bool first = false;

    public Transform swordHelper;
    Vector3 playerPosAnimation, swordPosAnimation;

    public bool end = false;
    public float enemiesLeftCounter;

    bool strongAttack;
    public static float currentDemage = 1.4f;





    public PlayerWeapon weapon;
    public Shield shield;
    Animator weaponAnimator;

    public float fix = -7f;

    private void Awake()
    {
        goodEntity = true;
        weapon = GetComponentInChildren<PlayerWeapon>();
    }

    override protected void Start()
    {
        base.Start();
        EntityStart();
        strongAttack = false;
        hp = 100;
        speed = 17f;
        runningSpeed = 34f;
        invincibiltyWindow = 0.6f;
        sr = GetComponent<SpriteRenderer>();
        mainCamera = FindObjectOfType<Camera>();
        
        weaponAnimator = weapon.GetComponent<Animator>();
    }

    bool duringAttack = false;
    public void JustParried()
    {
        audioSource.clip = parryClip;
        audioSource.Play();
        strongAttack = true;
        Invoke("EndJustParried", 1f);
    }

    void EndJustParried()
    {
        strongAttack = false;
    }

    public bool IsAnimating()
    {
        return !weapon.CurrentAnimationClip().Contains("Ready");
    }


    public void Freeze(float duration)
    {
        frozen = true;
        Invoke("Unfreeze", duration);
    }

    void Unfreeze()
    {
        frozen = false;
    }

    //override public void GotHit(int demage, Transform demageSourcePos, bool knockback = true, float knockbackForce = 1f, float knockbackDuration = 0.3f)
    //{
    //    hp -= demage;
    //    if (hp <= 0)
    //        Death();
    //    else
    //    {
    //        if (knockback)
    //        {
    //            StartKnockback(demageSourcePos, knockbackForce, knockbackDuration);

    //        }

    //    }

    //}






    void Movement()
    {
        if (duringAttack)
            return;
        float currentSpeed;
        if (isSprinting)
            currentSpeed = runningSpeed;
        else currentSpeed = speed;

        if (stunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (frozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        float horizontalInput = 0.0f;
        float verticalInput = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            verticalInput -= 1.0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1.0f;
        }

        Vector2 moveDirection = Vector2.zero;
        if (verticalInput == 0 && horizontalInput == 1)
        {
            if (transform.position.x > Game.maxX)
                horizontalInput = 0;

        }

        else if (verticalInput == 1 && horizontalInput == 0)
        {
            if (transform.position.y > Game.maxY)
                verticalInput = 0;
        }

        else if (verticalInput == 0 && horizontalInput == -1)
        {
            if (transform.position.x < Game.minX)
                horizontalInput = 0;

        }

        else if (verticalInput == -1 && horizontalInput == 0)
        {
            if (transform.position.y < Game.minY)
                verticalInput = 0;
        }

        else if (verticalInput == 1 && horizontalInput == 1)
        {
            if (transform.position.x > Game.maxX)
            {
                horizontalInput = 0;
            }
            if (transform.position.y > Game.maxY)
            {
                verticalInput = 0;
            }
            
        }
        else if (verticalInput == -1 && horizontalInput == -1)
        {
            if (transform.position.x < Game.minX)
            {
                horizontalInput = 0;
            }
            if (transform.position.y < Game.minY)
            {
                verticalInput = 0;
            }
           
        }
        else if (verticalInput == 1 && horizontalInput == -1)
        {
            if (transform.position.x < Game.minX)
            {
                horizontalInput = 0;
            }
            if (transform.position.y > Game.maxY)
            {
                verticalInput = 0;
            }
           
        }
        else if (verticalInput == -1 && horizontalInput == 1)
        {
            if (transform.position.x > Game.maxX)
            {
                horizontalInput = 0;
            }
            if (transform.position.y < Game.minY)
            {
                verticalInput = 0;
            }
           
        }
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        rb.velocity = currentSpeed * moveDirection;

    }

    void Rotate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDirection = mousePosition - transform.position;
        lookDirection.z = 0;

        if (lookDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private Vector2 minBounds = new Vector2(Game.minX, Game.minY);
    private Vector2 maxBounds = new Vector2(Game.maxX, Game.maxY);

    void CameraPosition()
    {
        float screenWidth = CalculateVisibleDistance(Camera.main);
        float screenHeight = CalculateVisibleDistance(Camera.main);
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, minBounds.x +( screenWidth / 2 + fix), maxBounds.x - (screenWidth / 2 + fix));
        clampedPosition.y = Mathf.Clamp(transform.position.y, minBounds.y + (screenHeight / 2 + fix) , maxBounds.y - (screenHeight / 2 + fix));
        clampedPosition.z = mainCamera.transform.position.z;
        mainCamera.transform.position = clampedPosition;
    }

    float CalculateVisibleDistance(Camera camera)
    {
        float orthographicSize = camera.orthographicSize;
        float aspectRatio = camera.aspect;

        float visibleDistance = 2f * orthographicSize * aspectRatio;

        return visibleDistance;
    }




    void Attack(bool enableInstantAttack = false)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!IsAnimating())
                LaunchAttack();
            //else
            //{
            //    if (instantAttacksAvailable > 0 && enableInstantAttack)
            //    {
            //        weaponAnimator.speed = 4.2f;
            //        instantAttack = true;
            //        instantAttacksAvailable--;
            //        Invoke("ResetInstantAttacks", 3f);
            //    }
            //}

        }
    }

    public void HandleSword()
    {
        if (strongAttack)
            CounterAttack();
        if (!duringAttack)
            audioSource.clip = attackClip;
        audioSource.Play();
        strongAttack = false;
        shield.ShieldDown();
        weapon.transform.parent = swordHelper;
        first = true;
        weapon.transform.localPosition = Vector3.zero;
        swordPosAnimation = swordHelper.localPosition;
        playerPosAnimation = transform.localPosition;


    }

    void CounterAttack()
    {
        duringAttack = true;
        Player.currentDemage = 10;
        Vector2 forwardVelocity = transform.right;
        audioSource.clip = counterClip;
        audioSource.Play();
        rb.velocity = forwardVelocity * 30;

        Invoke("EndCounterAttack", 0.3f);
    }

    void EndCounterAttack()
    {
        
        rb.velocity = Vector2.zero;
        duringAttack = false;
        Player.currentDemage = 1.4f;
    }




    void FixSword()
    {
        if (IsAnimating() /*|| instantAttack*/)
        {
            if (weapon.transform.parent != swordHelper)
            {
                weapon.transform.parent = swordHelper;
            }
            swordHelper.position = swordPosAnimation + (transform.position - playerPosAnimation);
            lastPos = transform.position;
            //weaponTransform.localPosition = Vector3.zero;
        }
        else
        {
            weapon.transform.parent = transform;
            first = false;
            swordHelper.rotation = transform.rotation;
            swordHelper.position = weapon.transform.position;

            weapon.transform.localPosition = new Vector2(0.9f, -0.05f);
        }
    }

    


    void LaunchAttack()
    {
        if (shield.shieldUp && !strongAttack)
            return;

        if (weapon.CurrentAnimationClip() == "Ready Up")
        {
            HandleSword();
            weaponAnimator.SetTrigger("Swipe");
            weaponAnimator.speed = 1;
            swipeCounter++;
            instantAttack = false;



        }
        else if (weapon.CurrentAnimationClip() == "Ready Down")
        {
            HandleSword();
            weaponAnimator.SetTrigger("Reverse Swipe");
            weaponAnimator.speed = 1;
            swipeCounter++;
            instantAttack = false;

        }

    }

    bool instantAttack = true;
    int instantAttacksAvailable = 3;

    void ResetInstantAttacks()
    {
        if (!IsInvoking("ResetInstantAttacks"))
        {
            instantAttacksAvailable = Random.Range(2, 4);
        }
    }




    public void AnimationEnd()
    {
        if (instantAttack)
        {
            shouldWait = true;
        }
    }

    private int frameCount = 0;
    private bool shouldWait = false;

    void InstantAttackDelay()
    {
        if (shouldWait)
        {
            frameCount++;
            if (frameCount >= 2)
            {
                frameCount = 0;
                shouldWait = false;
                LaunchAttack();
            }
        }
    }

   



    void Update()
    {
        CameraPosition();
        Movement();
        Run();
        Rotate();
        Attack();
        FixSword();
        InstantAttackDelay();
        CheckLose();
        if (gameObject != null)
        {
            staminaBar.value = currentStamina;
            hpBar.value = this.hp;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.Tab))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            
        

    }

    public void Win()
    {
        SceneManager.LoadScene("Win Screen");
        print('W');

    }

    void CheckLose()
    {
        if(hp <= 0)
        {
            SceneManager.LoadScene("Lose Screen");
            print('L');
        }
    }

    bool isSprinting = false;
    float maxStamina = 100f;
    float currentStamina = 100f;
    float staminaConsumptionRate = 70;
    float staminaRecoveryRate = 35f;

    public void AddHP(float hp)
    {
        this.hp += hp;
        if (this.hp > 100)
            this.hp = 100;
    }

    void Run()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(currentStamina < 1)
            {
                isSprinting = false;
                currentStamina = 0;

            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 1)
        {
            isSprinting = true;
            
        }

      
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
           
        }

        if (isSprinting)
        {
            currentStamina -= staminaConsumptionRate * Time.deltaTime;
            if (currentStamina <= 1)
                currentStamina = 0;
        }

        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }

        // Clamp stamina within the range [0, maxStamina]
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Update player movement based on sprinting state
        //this.speed = isSprinting ? runningSpeed : speed;


        // Update UI
        //UpdateStaminaBar();
    }


}


