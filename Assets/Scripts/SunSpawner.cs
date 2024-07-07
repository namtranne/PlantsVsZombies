using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunObject;

    private void Start()
    {
        SpawnSun();
    }
    
    void SpawnSun()
    {
        GameObject mySun = Instantiate(sunObject, new Vector3(Random.Range(-8f, 8f), 6, 0), Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYPos = Random.Range(0.0f, -3.0f);
        Invoke("SpawnSun", Random.Range(6, 12));
    }
}
