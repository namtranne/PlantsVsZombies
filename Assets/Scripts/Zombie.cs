using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 0.08f; // Movement speed
    public int health = 10;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the zombie to the left
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
