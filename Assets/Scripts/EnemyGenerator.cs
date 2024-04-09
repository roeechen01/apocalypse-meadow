using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    public Enemy witchPrefab, spiderPrefab, zombiePrefab, skeletonPrefab, wolfPrefab;
    public Enemy witchVariant, spiderVariant, zombieVariant, skeletonVariant, wolfVariant;

    Player player;

    List<Enemy> enemySpawns;
    Enemy lastEnemy;
    List<int> spawnsAmount;
    List<int> spawnTimers;
    List<int> counterStart;
    List<bool> spawnsLoop;
    

    public bool activated = true;

    int aliveCounter = 0;

    float spawnDistance = 3f;
    float spawnRadius = 30f;

    public SpawnInfo[] activeSpawnInfos = new SpawnInfo[5];
    public SpawnInfo[] passiveSpawnInfos = new SpawnInfo[5];
    public SpawnInfo[] variantActiveSpawnChances = new SpawnInfo[5];
    public SpawnInfo[] variantPassiveSpawnChances = new SpawnInfo[5];

    void Start()
    {
        if (!activated)
            return;
        aliveCounter++;
        passiveSpawnInfos = new SpawnInfo[5];
        variantActiveSpawnChances = new SpawnInfo[5];
        variantPassiveSpawnChances = new SpawnInfo[5];

        //

        CallWavesEnd();
        InvokeRepeating("OneSec", 1f, 1f);
        player = FindObjectOfType<Player>();
        enemySpawns = new List<Enemy>();
        spawnsAmount = new List<int>();
        spawnTimers = new List<int>();
        counterStart = new List<int>();
        spawnsLoop = new List<bool>();
        Invoke("SpawnActiveEnemy", Random.Range(2, 6));
    }


    void SpawnActiveEnemy()
    {
        float[] chances = new float[5];
        for (int i = 0; i < chances.Length; i++)
        {
            chances[i] = activeSpawnInfos[i].chance;
        }

        int index = SpawnInfo.GetRandomIndex(chances);
        

        if(SpawnInfo.NotEverythingCapped(activeSpawnInfos) && activeSpawnInfos[index].onScreen >= activeSpawnInfos[index].cap) //If enemy is capped but others aren't
        {
            //print("cap! cant spawn " + spawnInfos[index].enemyPrefab.name);
            SpawnActiveEnemy();
            
        }
        else
        {
            Enemy enemy = activeSpawnInfos[index].enemyPrefab;
            if(Random.Range(0f, 1f) < variantActiveSpawnChances[index].chance)
            {
                enemy = variantActiveSpawnChances[index].enemyPrefab;
            }

            int amountToSpawn = Random.Range(1, 3);
            if (!SpawnInfo.NotEverythingCapped(activeSpawnInfos))//If all enemies are at their screen cap
            {
                ActiveRandomGeneraion(enemy, amountToSpawn);
                Invoke("SpawnActiveEnemy", Random.Range(4, 11));
            }
            else if(activeSpawnInfos[index].onScreen < activeSpawnInfos[index].cap)//If enemy is not at cap
            {
                ActiveRandomGeneraion(enemy, amountToSpawn);
                Invoke("SpawnActiveEnemy", Random.Range(2, 6));
            }
            
            
        }
            
    }

    bool waveEnded = true;
    int currentWave = 0;

    void ActiveSpawnsManager()
    {

        switch (currentWave)
        {
            case 0: currentWave = 1;
                waveEnded = true; break;
            case 1:
                if (waveEnded)
                {
                    
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0, 0);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 1f, 5);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f, 0);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0, 0);
                    ActiveRandomGeneraion(zombiePrefab, 5);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 1f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;
                   
                } break;

            case 2:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f, 0);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.7f, 6);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.3f, 4);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0, 0);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.5f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.5f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);


                    waveEnded = false;

                }
                break;

            case 3:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.2f, 5);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.5f, 3);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.3f, 3);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0, 0);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.6f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.2f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.2f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;



                }
                break;

            case 4:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.1f, 3);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.4f, 5);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.3f, 3);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.2f, 4);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0, 0);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.5f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.5f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.1f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);






                    waveEnded = false;

                }
                break;

            case 5:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.25f, 3);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.25f, 3);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0, 0);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.4f, 3);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.1f, 2);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.5f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.3f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.2f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.1f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;
            case 6:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.2f, 2);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.2f, 2);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0, 0);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.2f, 2);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.4f, 2);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.4f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 1f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.4f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.2f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.1f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;
            case 7:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0, 0);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0, 0);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.8f, 10);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.2f, 2);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 1f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;
            case 8:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.2f, 5);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.3f, 5);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.3f, 5);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.2f, 4);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.5f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.5f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.15f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0.1f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;

            case 9:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f, 0);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.5f, 6);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f, 0);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.5f, 3);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.9f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.1f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.2f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.2f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;

            case 10:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f, 0);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.4f, 7);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.3f, 5);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.2f, 5);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.1f, 2);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.4f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.4f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.2f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.2f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0.2f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.2f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0.15f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0.2f);


                    waveEnded = false;

                }
                break;

            case 11:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 1f, 15);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f, 0);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f, 0);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f, 0);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 1f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;

            case 12:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.8f, 13);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f, 0);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f, 0);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.2f, 4);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 1f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0.2f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0.2f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0.1f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);


                    waveEnded = false;

                }
                break;

            case 13:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.5f, 10);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f, 0);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.3f, 10);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.2f, 7);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f, 0);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.7f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.2f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.1f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.2f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0.2f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.2f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0.15f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0.3f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0.2f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0.2f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;

            case 14:
                if (waveEnded)
                {
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f, 0);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.9f, 15);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.1f, 5);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f, 0);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 1f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.15f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0.2f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;

            case 15:
                if (waveEnded)
                {
                    print("Last Wave!");
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0.2f, 10);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0.2f, 10);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0.2f, 10);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0.2f, 10);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0.2f, 5);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0.3f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0.4f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0.2f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0.2f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0.3f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;

            case 16:
                if (waveEnded)
                {
                    KillDistantEnemies();
                    print("No more spawns!");
                    activeSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0, 0);
                    activeSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0, 0);
                    activeSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f, 0);
                    activeSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0, 0);
                    activeSpawnInfos[4] = new SpawnInfo(witchPrefab, 0, 0);

                    passiveSpawnInfos[0] = new SpawnInfo(spiderPrefab, 0f);
                    passiveSpawnInfos[1] = new SpawnInfo(zombiePrefab, 0f);
                    passiveSpawnInfos[2] = new SpawnInfo(skeletonPrefab, 0f);
                    passiveSpawnInfos[3] = new SpawnInfo(wolfPrefab, 0f);
                    passiveSpawnInfos[4] = new SpawnInfo(witchPrefab, 0f);

                    variantActiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantActiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantActiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantActiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantActiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    variantPassiveSpawnChances[0] = new SpawnInfo(spiderVariant, 0f);
                    variantPassiveSpawnChances[1] = new SpawnInfo(zombieVariant, 0f);
                    variantPassiveSpawnChances[2] = new SpawnInfo(skeletonVariant, 0f);
                    variantPassiveSpawnChances[3] = new SpawnInfo(wolfVariant, 0f);
                    variantPassiveSpawnChances[4] = new SpawnInfo(witchVariant, 0f);

                    waveEnded = false;

                }
                break;

        }
    }

    void KillDistantEnemies()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach(GameObject enemy in allObjects)
        {
            if (enemy.GetComponent<Enemy>() && Vector2.Distance(player.transform.position, enemy.transform.position) > 35f )
            {
                Destroy(enemy);
                print("killed");
            }
            else
            {
                print("stayed");
                enemiesLeftCounter++;
            }
        }
        player.end = true;
        player.enemiesLeftCounter = enemiesLeftCounter;


    }

    int enemiesLeftCounter = 0;

    void SetWaveEnd()
    {
        currentWave++;
        waveEnded = true;
    }

    void CallWavesEnd()
    {
        Invoke("SetWaveEnd", 15f);
        Invoke("SetWaveEnd", 30f);
        Invoke("SetWaveEnd", 45f);
        Invoke("SetWaveEnd", 60f);
        Invoke("SetWaveEnd", 75f);
        Invoke("SetWaveEnd", 90f);
        Invoke("SetWaveEnd", 105f);
        Invoke("SetWaveEnd", 120f);
        Invoke("SetWaveEnd", 135);
        Invoke("SetWaveEnd", 150f);
        Invoke("SetWaveEnd", 165f);
        Invoke("SetWaveEnd", 180f);
        Invoke("SetWaveEnd", 195f);
        Invoke("SetWaveEnd", 230f);
        Invoke("SetWaveEnd", 300f);//No more spawns from second 300
    }




    
    void OneSec()
    {
        foreach (SpawnInfo spawnInfo in activeSpawnInfos)
            spawnInfo.onScreen = 0;
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the object is in the camera view
            if (obj.GetComponent<Enemy>() && IsObjectInCameraView(obj))
            {
                if (obj.GetComponent<Spider>())
                {
                    activeSpawnInfos[0].onScreen++;
                }
                else if (obj.GetComponent<Zombie>())
                {
                    activeSpawnInfos[1].onScreen++;
                }
                else if (obj.GetComponent<Skeleton>())
                {
                    activeSpawnInfos[2].onScreen++;
                }
                else if (obj.GetComponent<Wolf>())
                {
                    activeSpawnInfos[3].onScreen++;
                }
                else if (obj.GetComponent<Witch>())
                {
                    activeSpawnInfos[4].onScreen++;
                }
            }
        }

        //for (int i = 0; i < spawnInfos.Length; i++)
        //{
        //    if (spawnInfos[i].onScreen >= spawnInfos[i].cap)
        //        spawnInfos[i].spawnTimer -= 0.5f;
        //    else spawnInfos[i].spawnTimer--;
        //    if(spawnInfos[i].spawnTimer == 0)
        //    {
        //        ActiveRandomGeneraion(spawnInfos[i].enemyPrefab, spawnInfos[i].spawnAmount);
        //        spawnInfos[i].spawnTimer = spawnInfos[i].lastTimer;



        //    }
        //}

        
    }

   

    void ActiveRandomGeneraion(Enemy enemy, int amount)
    {
        if (!activated)
            return;
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {

            if (Random.Range(1, 6) == 5)
            {
                if (amount == 1)
                    amount = 2;
                else if (amount == 2)
                    amount = 3;
            }
            // Instantiate an enemy at the calculated spawn point

            for (int i = 0; i <amount; i++)
            {
                float randomAngle;

                bool foundSuitableSpawnPoint = false;
                Vector2 spawnPoint = Vector2.zero;
                while (!foundSuitableSpawnPoint)
                {
                    // Calculate the spawn point based on the random angle and spawnRadius
                    randomAngle = Random.Range(0f, 2f * Mathf.PI);
                    spawnPoint = new Vector2(
                        player.transform.position.x + Mathf.Cos(randomAngle) * spawnRadius,
                        player.transform.position.y + Mathf.Sin(randomAngle) * spawnRadius);

                    if (Game.IsInGameMap(spawnPoint))
                        foundSuitableSpawnPoint = true;
                }


               
                Instantiate(enemy, spawnPoint, Quaternion.identity);

            }

        }

    }


    public static bool IsObjectInCameraView(GameObject obj)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null || obj == null)
        {
            return false;
        }

        // Get the object's position in viewport coordinates
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(obj.transform.position);

        // Check if the object is within the camera's viewport (0 to 1 for both x and y)
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

     void Update()
    {
        ActiveSpawnsManager();

    }





}

public class SpawnInfo
{
    public Enemy enemyPrefab;
    public int spawnRate;
    public int spawnAmount;
    public float spawnTimer;
    public int lastTimer;
    public int cap;
    public int onScreen;
    public float chance;

    public SpawnInfo(Enemy enemy, int currentSpawnRate, int spawnAmount, int cap)
    {
        this.enemyPrefab = enemy;
        this.spawnRate = currentSpawnRate;
        this.spawnAmount = spawnAmount;
        this.spawnTimer = spawnRate + Random.Range(2, 5);
        lastTimer = spawnRate;
        this.cap = cap;
    }

    public SpawnInfo(Enemy enemy,float chance , int cap)
    {
        this.enemyPrefab = enemy;
        this.spawnAmount = 1;
        this.cap = cap;
        this.chance = chance;
    }

    public SpawnInfo(Enemy enemy, float chance)
    {
        this.enemyPrefab = enemy;
        this.spawnAmount = 1;
        this.chance = chance;
    }

    public static bool NotEverythingCapped(SpawnInfo[] spawnInfo)
    {

        for(int i = 0; i < spawnInfo.Length; i++)
        {
            bool noCap = spawnInfo[i].chance > 0 && spawnInfo[i].onScreen < spawnInfo[i].cap;
            if (noCap)
            {
                //Debug.Log(spawnInfo[i].onScreen + " / " + spawnInfo[i].cap);
                return true;
            }
        }

        return false;
    }

    public static int GetRandomIndex(float[] chances)
    {
        float sum = 0;
        float randomValue = Random.value; // Random value between 0 and 1
        float cumulativeProbability = 0f;

        for (int i = 0; i < chances.Length; i++)
        {
            sum += chances[i];
            if(i == chances.Length - 1 && sum < 1)
            {
                Debug.Log("Spawn chances sum are not 1");
                return -1;

            }
            cumulativeProbability += chances[i];

            if (randomValue <= cumulativeProbability)
            {
                return i; // Return the index when the cumulative probability exceeds the random value
            }
        }

        // This should not happen if the sum of chances is 1, but just in case
        return -1;
    }



}

