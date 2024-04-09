using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    float maxRandomRotation = 7.5f;
    public bool isVariant;
    public bool partOfFireWall;
    float partOfWallSpeed;
    public float trucker;

    AudioSource audioSource;
    public AudioClip sound;



    // Start is called before the first frame update
    void Start()
    {
        speed = 27f;
        partOfWallSpeed = 9f;
        Destroy(gameObject, 30f);
        audioSource = GetComponent<AudioSource>();
        print(audioSource);
        audioSource.Play();
        transform.localScale *= 1.5f;
        if (isVariant)
        {
            demage = 15;
        }
        else demage = 10;
       
    }

    public void ChangeRotation()
    {
        Quaternion currentRotation = transform.rotation;

        // Generate a random rotation around the Z axis
        float randomRotation = Random.Range(-maxRandomRotation, maxRandomRotation);

        // Add the random rotation to the existing rotation
        Quaternion randomQuaternion = Quaternion.Euler(0f, 0f, randomRotation);
        Quaternion newRotation = currentRotation * randomQuaternion;

        // Apply the new rotation to the object
        transform.rotation = newRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!partOfFireWall)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(partOfWallSpeed * Time.deltaTime, 0, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.gameObject.GetComponent<Entity>();

        if (entity && entity.goodEntity)
        {
            entity.GotHit(demage, transform, true, 0.6f, 0.3f, true, false, 0.5f);
            //Destroy(gameObject);
        }
    }
}
