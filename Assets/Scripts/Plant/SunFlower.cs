using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : MonoBehaviour
{
    public GameObject sunObject;
    public float cooldown;

    private void Start()
    {
        InvokeRepeating("SpawnSun", cooldown, cooldown);
    }

    void SpawnSun()
    {
        GameObject mySun = Instantiate(sunObject, new Vector3(transform.position.x + .2f, transform.position.y + .2f, 0), Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYPos = transform.position.y - .5f;
    }
}
