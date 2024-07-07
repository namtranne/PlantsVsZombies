using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShroom : MonoBehaviour
{
    public int frozenTime = 5; 
    public GameObject freezeEffectPrefab;
    private GameObject freezeEffect;

    private void Start()
    {
        Invoke("ActivateFreeze", 1.5f);
    }

    void ActivateFreeze()
    {
        FreezeZombies();
        if (freezeEffectPrefab != null)
        {
            freezeEffect = Instantiate(freezeEffectPrefab, transform.position, Quaternion.identity);
        }
        gameObject.GetComponent<Plant>().tile.hasPlant = false;
        Destroy(gameObject); // Destroy the Ice Shroom after activation
        Destroy(freezeEffect, 1f);
    }

    private void FreezeZombies()
    {
        Zombie[] zombies = FindObjectsOfType<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            zombie.Frozen(frozenTime);
        }
    }
}
