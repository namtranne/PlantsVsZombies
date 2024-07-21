using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingExplode : MonoBehaviour
{
    public LayerMask targetMask;
    public AudioSource audioSource;
    private HashSet<Zombie> zombieSet = new HashSet<Zombie>();
    // Start is called before the first frame update
    void Start()
    {
        audioSource = Instantiate(audioSource);
        Invoke("Destroy", 1f);
    }

    // Update is called once per frame

    void Destroy() {
        audioSource.Stop();
        Destroy(gameObject);
    }

    void Update() {
        float range = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        float height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector2 boxSize = new Vector2(range, height);
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, targetMask);

        // Process the hits
        foreach (Collider2D hit in hits)
        {
            Zombie zombie = hit.GetComponent<Zombie>();
            if(zombieSet.Contains(zombie)) {
                continue;
            }
            else {
                zombieSet.Add(zombie);
                zombie.Hit(100000, false);
            }
            
        }
    }

}
