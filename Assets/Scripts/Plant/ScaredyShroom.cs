using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaredyShroom : MonoBehaviour
{
    public float range = 5.0f;
    public float rangeScare = 10.0f; // Range within which the shroom gets scared
    public float scaredDuration = 5.0f; // Duration for which the shroom stays scared
    public LayerMask zombieLayerMask; // Layer mask for detecting zombies
    private bool scary = false;
    private float scaredTimer = 0f;

    void Start()
    {
        GetComponent<Animator>().SetBool("scary", false);
    }

    void Update()
    {
        CheckForZombies();
        if (scary)
        {
            scaredTimer += Time.deltaTime;
            if (scaredTimer >= scaredDuration)
            {
                CalmDown();
            }
        }

    }

    void CheckForZombies()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, rangeScare, zombieLayerMask);
        if (hitColliders.Length > 0)
        {
            Debug.Log("Zombie detected within scare range.");
            GetScared();
        }
    }

    void GetScared()
    {
        if (!scary)
        {
            scary = true;
            scaredTimer = 0f;
            GetComponent<Animator>().SetBool("scary", true);
            Debug.Log("Shroom is scared.");
            // Additional logic for when the shroom is scared (e.g., stop attacking)
        }
    }

    void CalmDown()
    {
        scary = false;
        GetComponent<Animator>().SetBool("scary", false);
        Debug.Log("Shroom calmed down.");
        // Additional logic for when the shroom calms down (e.g., resume attacking)
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
