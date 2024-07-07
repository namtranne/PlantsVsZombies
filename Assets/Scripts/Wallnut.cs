using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallnut : MonoBehaviour
{
    public RuntimeAnimatorController cracked_1;
    public int health_cracked_1;
    public RuntimeAnimatorController cracked_2;
    public int health_cracked_2;

    private Plant curPlant;
    private short curAnimation = 0;

    private void Start()
    {
        curPlant = GetComponent<Plant>();
    }

    private void Update()
    {
        if (!curPlant) return;
        if (curPlant.health <= health_cracked_1 && curAnimation < 1)
        {
            curAnimation = 1;
            GetComponent<Animator>().runtimeAnimatorController = cracked_1;
        }
        if (curPlant.health <= health_cracked_2 && curAnimation < 2)
        {
            curAnimation = 2;
            GetComponent<Animator>().runtimeAnimatorController = cracked_2;
        }
    }
}
