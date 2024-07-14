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
    public float baseZombieDelay = 5;

    public int level;
    public float difficultyMultiplier = 0.5f;

    private float currentZombieDelay;
    private float timeSinceLastSpawn;
    private int waveNumber = 0;
    private List<ZombieTypes> currentWave = new List<ZombieTypes>();

    private float eventCooldown = 30f;
    private float timeSinceLastEvent = 0f;

    public float initialPeacePeriod = 30f; // Time at the start for players to set up
    private float peacePeriodTimer;

    private void Start()
    {
        StartLevel(level);
        progressBar.maxValue = zombieMax;
        peacePeriodTimer = initialPeacePeriod;
    }

    private void Update()
    {
        if (peacePeriodTimer > 0)
        {
            peacePeriodTimer -= Time.deltaTime;
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= currentZombieDelay && zombiesSpawned < zombieMax)
        {
            SpawnZombie();
            timeSinceLastSpawn = 0;
        }

        timeSinceLastEvent += Time.deltaTime;
        if (timeSinceLastEvent >= eventCooldown)
        {
            TriggerRandomEvent();
            timeSinceLastEvent = 0f;
        }
    }

    public void StartLevel(int level)
    {
        zombieMax = 60 + 40 * level;
        baseZombieDelay = Mathf.Max(5f - 0.2f * level, 0.5f);
        currentZombieDelay = baseZombieDelay;
        progressBar.maxValue = zombieMax;
        zombiesSpawned = 0;
        zombiesKilled = 0;
        waveNumber = 0;

        UpdateZombieProbabilities(level);
        PrepareNextWave();
    }

    private void UpdateZombieProbabilities(int level)
    {
        float totalWeight = 0f;
        int baseProbability = 10;

        for (int i = 0; i < zombieTypes.Length; i++)
        {
            float adjustedProbability = baseProbability + Mathf.FloorToInt(Mathf.Pow(difficultyMultiplier, i) * level * 2);
            totalWeight += adjustedProbability;
            zombieTypes[i].probability = totalWeight;
        }

        for (int i = 0; i < zombieTypes.Length; i++)
        {
            zombieTypes[i].probability /= totalWeight;
        }
    }

    private void PrepareNextWave()
    {
        waveNumber++;
        currentWave.Clear();
        int waveSize = Mathf.Min(5 + waveNumber * 2, 20);

        for (int i = 0; i < waveSize; i++)
        {
            currentWave.Add(SelectZombieType());
        }

        if (waveNumber % 2 == 0 && waveNumber / 2 < zombieTypes.Length)
        {
            currentWave.Add(zombieTypes[waveNumber / 2].type);
        }

        for (int i = 0; i < currentWave.Count; i++)
        {
            ZombieTypes temp = currentWave[i];
            int randomIndex = Random.Range(i, currentWave.Count);
            currentWave[i] = currentWave[randomIndex];
            currentWave[randomIndex] = temp;
        }

        currentZombieDelay = baseZombieDelay * Mathf.Pow(0.9f, waveNumber - 1);
    }

    private void SpawnZombie()
    {
        if (currentWave.Count == 0)
        {
            PrepareNextWave();
        }

        ZombieTypes typeToSpawn = currentWave[0];
        currentWave.RemoveAt(0);

        int spawnPosition = SelectSpawnPoint();
        GameObject myZombie = Instantiate(zombie, spawnPoints[spawnPosition].position, Quaternion.identity);
        Zombie zombieComponent = myZombie.GetComponent<Zombie>();
        zombieComponent.type = typeToSpawn;
        zombieComponent.health = typeToSpawn.health * (1 + 0.1f * level);
        zombieComponent.speed = typeToSpawn.speed * (1 + 0.05f * level);
        myZombie.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder++;

        zombiesSpawned++;
        progressBar.value = zombiesSpawned;

        if (zombiesSpawned >= zombieMax)
        {
            TriggerFinalWave();
        }
    }

    private int SelectSpawnPoint()
    {
        return Random.Range(0, spawnPoints.Length);
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

        return zombieTypes[zombieTypes.Length - 1].type;
    }

    private void TriggerFinalWave()
    {
        int finalWaveSize = Mathf.Min(zombieMax - zombiesSpawned, 30);
        for (int i = 0; i < finalWaveSize; i++)
        {
            ZombieTypes finalZombieType = zombieTypes[Mathf.Min(i / 3, zombieTypes.Length - 1)].type;
            currentWave.Add(finalZombieType);
        }
        currentZombieDelay = 0.3f;
    }

    private void TriggerRandomEvent()
    {
        int eventType = Random.Range(0, 5);

        switch (eventType)
        {
            case 0:
                SpeedBoost();
                break;
            case 1:
                ZombieHorde();
                break;
            case 2:
                ZombieUpgrade();
                break;
            case 3:
                ZombieWeakness();
                break;
            case 4:
                ZombieFrenzy();
                break;
        }
    }

    private void SpeedBoost()
    {
        StartCoroutine(TemporarySpeedBoost());
    }

    private IEnumerator TemporarySpeedBoost()
    {
        Zombie[] zombies = FindObjectsOfType<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            zombie.speed *= 2.5f;
        }

        yield return new WaitForSeconds(15f);

        foreach (Zombie zombie in zombies)
        {
            if (zombie != null)
            {
                zombie.speed = zombie.type.speed * (1 + 0.05f * level);
            }
        }
    }

    private void ZombieHorde()
    {
        int extraZombies = Random.Range(10, 21);
        for (int i = 0; i < extraZombies; i++)
        {
            SpawnZombie();
        }
    }

    private void ZombieUpgrade()
    {
        Zombie[] zombies = FindObjectsOfType<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            zombie.health *= 2f;
            zombie.damage *= 1.5f;
        }
    }

    private void ZombieWeakness()
    {
        StartCoroutine(TemporaryWeakness());
    }

    private IEnumerator TemporaryWeakness()
    {
        Zombie[] zombies = FindObjectsOfType<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            zombie.health *= 0.3f;
        }

        yield return new WaitForSeconds(10f);

        foreach (Zombie zombie in zombies)
        {
            if (zombie != null)
            {
                zombie.health = zombie.type.health * (1 + 0.1f * level);
            }
        }
    }

    private void ZombieFrenzy()
    {
        StartCoroutine(TemporaryFrenzy());
    }

    private IEnumerator TemporaryFrenzy()
    {
        float originalDelay = currentZombieDelay;
        currentZombieDelay = 0.1f;

        yield return new WaitForSeconds(8f);

        currentZombieDelay = originalDelay;
    }
}

[System.Serializable]
public class ZombieTypeProb
{
    public ZombieTypes type;
    public float probability;
}