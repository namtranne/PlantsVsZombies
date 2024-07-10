using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDiagonal : MonoBehaviour
{
    private Vector3 start;
    private Vector3 end;
    private float speed = 2.0f; // Adjust speed as needed
    private bool movingToEnd = true; // Flag to track movement direction

    void Start()
    {
    }

    void Update()
    {
        float step = speed * Time.deltaTime; // Calculate distance to move

        if (movingToEnd)
        {
            transform.position = Vector3.Lerp(transform.position, end, step);
            if (Vector3.Distance(transform.position, end) < 0.001f)
            {
                movingToEnd = false; // Change direction
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, start, step);
            if (Vector3.Distance(transform.position, start) < 0.001f)
            {
                movingToEnd = true; // Change direction
            }
        }
    }

    public void SetPosition(Vector3 a, Vector3 b) {
        transform.position = a;
        this.start = a;
        this.end = b;
    }
}

