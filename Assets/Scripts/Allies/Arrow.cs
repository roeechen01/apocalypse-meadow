using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        speed = 40f;
        demage = 0.6f;
        Destroy(gameObject, 10f);
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.GotHit(demage, transform, true, 0.7f, 0.25f);
            sr.sprite = null;
            Destroy(gameObject, 0.3f);
        }
    }
}
