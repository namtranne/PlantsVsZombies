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

    private List<ZombieTypes> probList = new List<ZombieTypes>();

    public int zombieMax;
    public int zombiesSpawned;
    public int zombiesKilled;
    public Slider progressBar;
    public float zombieDelay = 5;

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnZombie", 2, zombieDelay);

        foreach (ZombieTypeProb zom in zombieTypes)
            for (int i = 0; i < zom.probability; i++)
                probList.Add(zom.type);

        progressBar.maxValue = zombieMax;
    }

    private void Update()
    {
        // progressBar.value = zombiesSpawned;
    }



    void SpawnZombie()
    {
        if (zombiesSpawned >= zombieMax) return;
        zombiesSpawned++;
        int spawnPosition = Random.Range(0, spawnPoints.Length);
        GameObject myZombie = Instantiate(zombie, spawnPoints[spawnPosition].position, Quaternion.identity);

        myZombie.GetComponent<Zombie>().type = probList[Random.Range(0, probList.Count)];
        myZombie.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder++;
    }
}
[System.Serializable]
public class ZombieTypeProb
{
    public ZombieTypes type;
    public int probability;
}