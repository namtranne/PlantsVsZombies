using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject zombie;
    private int sortingOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnZombie", 10, 1);
    }

    void SpawnZombie() 
    {
        int spawnPosition = Random.Range(0, spawnPoints.Length);
        GameObject myZombie = Instantiate(zombie, spawnPoints[spawnPosition].position, Quaternion.identity);

        // Get the SpriteRenderer component and set the sortingOrder
        SpriteRenderer renderer = myZombie.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = sortingOrder;
            sortingOrder++; // Increment the sortingOrder for the next zombie
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
