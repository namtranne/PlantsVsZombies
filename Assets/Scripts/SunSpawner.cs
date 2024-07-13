using System.Collections;
using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunObject;
    public float initialSpawnDelay = 7f;
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 10f;
    public float spawnIntervalDecreaseRate = 0.1f;
    public float minSpawnY = -3.0f;
    public float maxSpawnY = 0.0f;

    private float currentMinInterval;
    private float currentMaxInterval;

    private void Start()
    {
        currentMinInterval = minSpawnInterval;
        currentMaxInterval = maxSpawnInterval;
        StartCoroutine(SpawnSunRoutine());
    }

    IEnumerator SpawnSunRoutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            SpawnSun();
            float interval = Random.Range(currentMinInterval, currentMaxInterval);
            yield return new WaitForSeconds(interval);

            // Giảm dần khoảng thời gian giữa các lần sinh sun
            currentMinInterval = Mathf.Max(currentMinInterval - spawnIntervalDecreaseRate, 3f);
            currentMaxInterval = Mathf.Max(currentMaxInterval - spawnIntervalDecreaseRate, 5f);
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
        // Tính toán giá trị của sun dựa trên thời gian game
        float gameTime = Time.timeSinceLevelLoad;
        return Mathf.Max(25, Mathf.RoundToInt(25 + gameTime / 60f * 5)); // Tăng 5 giá trị mỗi phút
    }
}