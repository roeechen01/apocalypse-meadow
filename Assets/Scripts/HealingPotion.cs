using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        Destroy(transform.gameObject, 15f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>())
        {
            float hpAmount;
            float rnd = Random.Range(0f, 1f);
            if (rnd <= 0.5f)
            {
                hpAmount = 20;
            }
            else hpAmount = 30;

            player.AddHP(hpAmount);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
