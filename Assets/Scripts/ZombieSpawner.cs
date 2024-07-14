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


    private bool flagStart = false;
    // private LevelManager levelManager;

    // Start is called before the first frame update
    private void Update()
    {
        if (GameManager.isSelecting || flagStart) return;

        flagStart = true;
        Debug.Log("Start level");
        StartLevel(GameManager.level);
        InvokeRepeating("SpawnZombie", (int)(zombieDelay * 2.5), zombieDelay);

        progressBar.maxValue = zombieMax;
    }

    public void StartLevel(int level)
    {
        zombieMax = 60 + 30*level;
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
        if (zombiesSpawned >= zombieMax) return;
        if (GameManager.isPaused) return;
        float percentage = 1f * zombiesSpawned / zombieMax;
        if (percentage >= 1) return;
        if (zombiesSpawned <= 6)
        {
            Debug.Log("Tren 6");
            InstantiateNewZombie(1);
        }
        else if (zombiesSpawned <= 12)
        {
            Debug.Log("Tren 12");
            InstantiateNewZombie(1);
            InstantiateNewZombie(1);
        }
        else if (percentage < .2f)
        {
            Debug.Log("Tren 0.2");
            InstantiateNewZombie(2);
            InstantiateNewZombie(2);
        }
        else if (percentage < .4f)
        {
            Debug.Log("Tren 0.4");
            InstantiateNewZombie(2);
            InstantiateNewZombie(2);
            InstantiateNewZombie(2);
        }
        else if (percentage < .6f)
        {
            Debug.Log("Tren 0.6");
            InstantiateNewZombie(3);
            InstantiateNewZombie(3);
            InstantiateNewZombie(3);
        }
        else if (percentage < .8f)
        {
            Debug.Log("Tren 0.8");
            InstantiateNewZombie(4);
            InstantiateNewZombie(4);
            InstantiateNewZombie(4);
        }
        else
        {
            Debug.Log("Stopp");
            CancelInvoke("SpawnZombie");
            for (int i = zombiesSpawned; i < zombieMax; i++)
            {
                InstantiateNewZombie(4);
            }
        }
    }

    void InstantiateNewZombie(int xBoost = 3)
    {
        if (zombiesSpawned >= zombieMax) return;
        
        int spawnPosition = Random.Range(0, spawnPoints.Length);
        GameObject myZombie = Instantiate(zombie, spawnPoints[spawnPosition].position, Quaternion.identity);
        zombiesSpawned++;
        progressBar.value = zombiesSpawned;
        myZombie.GetComponent<Zombie>().type = SelectZombieType();
        myZombie.GetComponent<Zombie>().xBoost = xBoost;
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
