using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int health;

    public Tile tile;

    public void Hit(int damage)
    {
        if(!gameObject) return;
        health -= damage;
        if (health <= 0)
        {
            tile.hasPlant = false;
            Destroy(gameObject);
        }
    }

    public void Destroy() {
        //if (!gameObject) return;
        tile.hasPlant = false;
        Destroy(gameObject);
    }

    private void Start()
    {
        gameObject.layer = 9;
    }
}
