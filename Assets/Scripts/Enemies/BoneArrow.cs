using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneArrow : Projectile
{
    SpriteRenderer sr;
    

    public bool isVariant;
    public bool deflected;
    float deflectDemage;

    AudioSource audioSource;
    public AudioClip sound;


    // Start is called before the first frame update
    void Start()
    {
        deflected = false;
        Destroy(gameObject, 15f);
        deflectDemage = 1f;
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (isVariant)
        {
            demage = 4;
            speed = 60f;
            deflectDemage = 5f;

        }

        else
        {
            demage = 3;
            speed = 30f;
            deflectDemage = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!deflected)
        transform.Translate(speed * Time.deltaTime, 0, 0);
        else transform.Translate(-speed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (!deflected && entity && entity.goodEntity)
        {
            entity.GotHit(demage, transform, true, 0.75f, 0.25f);
            audioSource.Play();
        }
        else
        {
            if(deflected && entity && !entity.goodEntity)
            {
                entity.GotHit(deflectDemage, transform, true, 0.75f, 0.25f);
                Destroy(gameObject);

            }
        }
    }

    public void Deflect()
    {
        deflected = true;
        
    }
}
