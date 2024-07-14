using System.Collections;
using UnityEngine;

public class SunFlower : MonoBehaviour
{
    public GameObject sunObject;
    public float baseCooldown = 24f;
    public float cooldownVariation = 3f;
    public int sunValue = 25;
    public float spawnRadius = 0.3f;

    private float nextSpawnTime;
    private bool isSpawning = false;

    private void Start()
    {
        SetNextSpawnTime();
        StartCoroutine(SpawnSunRoutine());
    }

    private void SetNextSpawnTime()
    {
        float randomCooldown = baseCooldown + Random.Range(-cooldownVariation, cooldownVariation);
        nextSpawnTime = Time.time + randomCooldown;
    }

    private IEnumerator SpawnSunRoutine()
    {
        while (true)
        {
            if (Time.time >= nextSpawnTime && !isSpawning && !GameManager.isPaused)
            {
                isSpawning = true;
                yield return StartCoroutine(SpawnSunAnimation());
                SpawnSun();
                isSpawning = false;
                SetNextSpawnTime();
            }
            yield return null;
        }
    }

    private IEnumerator SpawnSunAnimation()
    {
  
        yield return new WaitForSeconds(0.5f);
    }

    private void SpawnSun()
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        GameObject mySun = Instantiate(sunObject, spawnPosition, Quaternion.identity);
        Sun sunComponent = mySun.GetComponent<Sun>();
        if (sunComponent != null)
        {
            sunComponent.SetValue(sunValue);
            sunComponent.dropToYPos = transform.position.y - 0.5f;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}