using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : MonoBehaviour
{
    public float explosionRadius = 2f;
    public LayerMask targetMask;

    private void Start()
    {
        Invoke("ActivateExplosion", .9f);
    }

    void ActivateExplosion()
    {
        ExplodeZombies();
        gameObject.GetComponent<Plant>().tile.hasPlant = false;
        Destroy(gameObject, .7f); // Destroy the Cherry Bomb after activation
    }

    private void ExplodeZombies()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(4f, 5f), 0, targetMask);
        foreach (Collider2D hitCollider in hitColliders)
        {
            Zombie zombie = hitCollider.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.Boom();
            }
        }
    }
}
