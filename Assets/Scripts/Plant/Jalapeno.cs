using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : MonoBehaviour
{
    public GameObject explosionEffect;
    public LayerMask targetMask;

    void Start()
    {
        Invoke("Explode", .7f);
    }

    void Explode()
    {
        GameObject explosion = Instantiate(explosionEffect, new Vector3(2, transform.position.y + .5f, 0), Quaternion.identity);
        Destroy(gameObject);

        Collider2D[] zombies = Physics2D.OverlapBoxAll(transform.position, new Vector2(20f, .4f), 0, targetMask);
        foreach (Collider2D zombie in zombies)
        {
            zombie.GetComponent<Zombie>().Boom();
        }
        GetComponent<Plant>().Destroy();
        Destroy(explosion, 1f);
    }
}
