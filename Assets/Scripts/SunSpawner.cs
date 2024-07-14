using System.Collections;
using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunObject;
    public float initialSpawnDelay = 7f;
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 10f;
    public float spawnIntervalDecreaseRate = 0.05f;
    public float minSpawnY = -3.0f;
    public float maxSpawnY = 0.0f;
    public int baseSunValue = 25;
    public float sunValueIncreaseRate = 0.1f;

    private float currentMinInterval;
    private float currentMaxInterval;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentMinInterval = minSpawnInterval;
        currentMaxInterval = maxSpawnInterval;
        StartCoroutine(SpawnSunRoutine());
    }

    IEnumerator SpawnSunRoutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            if (!GameManager.isPaused)
            {
                SpawnSun();
                float interval = Random.Range(currentMinInterval, currentMaxInterval);
                yield return new WaitForSeconds(interval);

                currentMinInterval = Mathf.Max(currentMinInterval - spawnIntervalDecreaseRate * Time.deltaTime, 3f);
                currentMaxInterval = Mathf.Max(currentMaxInterval - spawnIntervalDecreaseRate * Time.deltaTime, 5f);
            }
            else
            {
                yield return null;
            }
        }
    }

    void SpawnSun()
    {
        float spawnX = Random.Range(-8f, 8f);
        float spawnY = 6f;
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        GameObject mySun = Instantiate(sunObject, spawnPosition, Quaternion.identity);
        Sun sunComponent = mySun.GetComponent<Sun>();
        sunComponent.dropToYPos = Random.Range(minSpawnY, maxSpawnY);
        sunComponent.SetValue(CalculateSunValue());
    }

    int CalculateSunValue()
    {
        float difficulty = gameManager.GetDifficulty();
        return Mathf.RoundToInt(baseSunValue + difficulty * sunValueIncreaseRate * baseSunValue);
    }
}