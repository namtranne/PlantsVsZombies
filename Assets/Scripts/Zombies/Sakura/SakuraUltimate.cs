using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class SakuraUltimate : MonoBehaviour
{
    public LayerMask plantMask;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector2.left, 50, plantMask);
        if(raycast.collider) {
            transform.position = raycast.collider.GetComponent<Plant>().transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate range based on half of the width of the SpriteRenderer bounds
        float range = GetComponent<SpriteRenderer>().bounds.size.x / 2;

        // Perform raycast to the left
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left, range, plantMask);

        // Perform raycast to the right
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, range, plantMask);

        // Process hits to the left
        foreach (RaycastHit2D hit in hitsLeft)
        {
            if(hit.collider) {
                hit.collider.GetComponent<Plant>().Destroy();
            }
        }

        // Process hits to the right
        foreach (RaycastHit2D hit in hitsRight)
        {
            hit.collider.GetComponent<Plant>().Destroy();
        }
        Invoke("Destroy",1);
        
    }

    void Destroy() {
        Destroy(gameObject);
    }

}
