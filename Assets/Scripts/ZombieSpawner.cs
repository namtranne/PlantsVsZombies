using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject zombie;
    private int sortingOrder = 2;
    public ZombieTypeProb[] zombieTypes;

    public int zombieMax;
    public int zombiesSpawned;
    public int zombiesKilled;
    public Slider progressBar;
    public float zombieDelay = 5;

    public int level;
    // private LevelManager levelManager;

    // Start is called before the first frame update
    private void Start()
    {
        StartLevel(level);
        InvokeRepeating("SpawnZombie", zombieDelay * 2, zombieDelay);

        progressBar.maxValue = zombieMax;
    }

    public void StartLevel(int level)
    {
        zombieMax = 60 + 30 * level;
        zombieDelay = 6f - .1f * level;
        progressBar.maxValue = zombieMax;

        float totalWeight = 0f;
        int baseProbability = 10; // Base probability
        float difficultyFactor = 1.2f; // Increasing difficulty with level


        for (int i = 0; i < zombieTypes.Length; i++)
        {
            float adjustedProbability = baseProbability + Mathf.FloorToInt(Mathf.Pow(difficultyFactor, i) * level);
            totalWeight += adjustedProbability;
            zombieTypes[i].probability = totalWeight;
        }

        // Normalize cumulative probabilities
        for (int i = 0; i < zombieTypes.Length; i++)
        {
            zombieTypes[i].probability /= totalWeight;
        }
    }

    void SpawnZombie()
    {
        int percentage = zombiesSpawned / zombieMax;
        if (percentage >= 1) return;
        if (zombiesSpawned <= 6)
        {
            InstantiateNewZombie();
        }
        else if (zombiesSpawned <= 12)
        {
            InstantiateNewZombie();
            InstantiateNewZombie();
        }
        else if (percentage < .3f)
        {
            InstantiateNewZombie();
            InstantiateNewZombie();
            InstantiateNewZombie();
        }
        else if (percentage < .6f)
        {
            InstantiateNewZombie();
            InstantiateNewZombie();
            InstantiateNewZombie();
            InstantiateNewZombie();
            InstantiateNewZombie();
        }
        else
        {
            CancelInvoke("SpawnZombie");
            for (int i = zombiesSpawned; i < zombieMax; i++)
            {
                InvokeRepeating("InstantiateNewZombie", 0, (i - zombiesSpawned) * .5f);
            }
        }
    }

    void InstantiateNewZombie()
    {
        if (zombiesSpawned > zombieMax) return;
        zombiesSpawned++;
        progressBar.value = zombiesSpawned;
        int spawnPosition = Random.Range(0, spawnPoints.Length);
        GameObject myZombie = Instantiate(zombie, spawnPoints[spawnPosition].position, Quaternion.identity);
        myZombie.GetComponent<Zombie>().type = SelectZombieType();
        myZombie.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder++;
    }
    private ZombieTypes SelectZombieType()
    {
        float randomValue = Random.value;

        foreach (ZombieTypeProb zom in zombieTypes)
        {
            if (randomValue <= zom.probability)
            {
                return zom.type;
            }
        }

        return zombieTypes[zombieTypes.Length - 1].type; // Fallback to the last type
    }
}
[System.Serializable]
public class ZombieTypeProb
{
    public ZombieTypes type;
    public float probability;
}