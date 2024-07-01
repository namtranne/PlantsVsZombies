using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Speed of the movement
    public float speed = 5f;

    void Update()
    {
        // Calculate the new position
        Vector3 newPosition = transform.position + speed * Time.deltaTime * Vector3.left;

        // Apply the new position to the transform
        transform.position = newPosition;
    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        speed = 0;
    }
}
