using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShroom : MonoBehaviour
{
    public GameObject sunObject;
    public float cooldown;

    public float bigDuration = 90;

    public RuntimeAnimatorController normalAnimation;
    public RuntimeAnimatorController bigAnimation;
    public RuntimeAnimatorController sleepAnimation;

    private void Start()
    {
        InvokeRepeating("SpawnSun", cooldown, cooldown);
        Invoke("BeBig", bigDuration);
    }

    private void BeBig()
    {

       GetComponent<Animator>().runtimeAnimatorController = bigAnimation;
    }

    void SpawnSun()
    {
        GameObject mySun = Instantiate(sunObject, new Vector3(transform.position.x + .2f, transform.position.y + .2f, 0), Quaternion.identity);
        mySun.GetComponent<Sun>().dropToYPos = transform.position.y - .5f;
    }
}
