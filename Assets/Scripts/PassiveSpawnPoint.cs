using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSpawnPoint : MonoBehaviour
{
    EnemyGenerator generator;
    bool activated = false;
    SpriteRenderer sr;

    
    // Start is called before the first frame update
    void Start()
    {
        generator = FindObjectOfType<EnemyGenerator>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = null;
        Invoke("Activate", 5f);
    }

    // Update is called once per frame
    void Update()
    {
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CameraCollider") && activated)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if (!generator.activated)
            return;

        activated = false;
        Invoke("Activate", 10f);

        float[] chances = new float[5];
        for (int i = 0; i < chances.Length; i++)
        {
            chances[i] = generator.passiveSpawnInfos[i].chance;
        }

        int index = SpawnInfo.GetRandomIndex(chances);
        if(index >= 0)
        {
            Enemy enemy = generator.passiveSpawnInfos[index].enemyPrefab;
            if (Random.Range(0f, 1f) < generator.variantPassiveSpawnChances[index].chance)
            {
                enemy = generator.variantPassiveSpawnChances[index].enemyPrefab;
            }
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
    }

    void Activate()
    {
        activated = true;
    }

}
