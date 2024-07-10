using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SasukeUltimate : MonoBehaviour
{
    public LayerMask plantMask;
    private float width;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        width = renderer.bounds.size.x;
        Debug.Log(width);
        transform.position += new Vector3(-width/2, 0.5f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(transform.position, Vector2.left, width/2, plantMask);
        foreach (RaycastHit2D raycastHit2D in raycastHit2Ds)
        {
            if(raycastHit2D.collider) {
                raycastHit2D.collider.GetComponent<Plant>().Destroy();
            }
        }
        raycastHit2Ds  = Physics2D.RaycastAll(transform.position, Vector2.right, width/2, plantMask);
        foreach (RaycastHit2D raycastHit2D in raycastHit2Ds)
        {
            if(raycastHit2D.collider) {
                raycastHit2D.collider.GetComponent<Plant>().Destroy();
            }
        }
        Destroy(gameObject, 1f);
    }
}
