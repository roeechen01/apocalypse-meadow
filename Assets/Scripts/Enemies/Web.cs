using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite webCage;
    float speed;
    Player player;
    float effectDuration;
    bool activated = false;

    void Start()
    {
        speed = 15f;
        player = FindObjectOfType<Player>();
        sr = GetComponent<SpriteRenderer>();
        effectDuration = 2f;
    }

    void Update()
    {
        if(!activated)
            transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!player.frozen)
            {
                transform.position = collision.gameObject.transform.position;
                sr.sprite = webCage;
                player.Freeze(effectDuration);
                activated = true;
                Destroy(gameObject, effectDuration);
            }
            
           
        }
    }
}
