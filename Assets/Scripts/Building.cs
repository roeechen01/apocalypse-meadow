using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int hp = 200;
    public bool good;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    int lastCounterHit = 0;
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Friendly Weapon" && !good && FindObjectOfType<Player>().IsAnimating())
        {
            if (lastCounterHit < Player.swipeCounter)
                lastCounterHit = Player.swipeCounter;
            else return;
            hp -= 4;
           
            if (hp > 0)
                print("hit connect on Ship");
            else
            {
                print("destroyed Ship");
                Destroy(gameObject);
            }
        }

        
    }
}
