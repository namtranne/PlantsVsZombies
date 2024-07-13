using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float health;

    public Tile tile;

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            tile.hasPlant = false;
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameObject.layer = 9;
    }
}
