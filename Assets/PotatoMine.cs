using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMine : MonoBehaviour
{
    public LayerMask shootMask;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetBool("boom"))
        {
            
            StartCoroutine(DestroyAfterDelay(.2f));
            
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, shootMask);
        if (hit.collider)
        {
            target = hit.collider.gameObject;
            GetComponent<Animator>().SetBool("boom", true);
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
