using UnityEngine;

public class SpikeWeed : MonoBehaviour
{
    public float damage = 2f; // Damage inflicted per second
    public float attackCooldown = 1f;   // Cooldown between attacks
    public float range = .6f;          // Range within which it can attack
    public LayerMask targetMask;

    private bool isAttacking = false;   // Flag to indicate if currently attacking

    // Start is called before the first frame update
    void Start()
    {
        // Start attacking immediately and set up cooldown
        InvokeRepeating("Attack", 0f, attackCooldown);
    }

    private void OnDestroy()
    {
        CancelInvoke("Attack");
    }

    void Attack()
    {
        // Look for nearby enemies to attack
        Collider2D[] zombies = Physics2D.OverlapCircleAll(transform.position, range, targetMask);
        foreach (Collider2D zombie in zombies)
        {
            zombie.GetComponent<Zombie>().Hit(damage, false);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a visual representation of the attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
