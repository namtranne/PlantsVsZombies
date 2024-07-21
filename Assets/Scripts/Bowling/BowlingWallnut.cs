using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class BowlingWallnut : MonoBehaviour
{
    // Speed of the rotation
    public float rotationSpeed = 100f;
    // Speed of the forward movement
    public float moveSpeed = 2f;
    
    // Direction for diagonal movement (true for up, false for down)
    private bool moveUp = false;
    private bool moveDown = false;
    public Sprite sprite;
    public BowlingType bowlingType;
    public GameObject explodeAnimation;

    void Start() {
        // gameObject.GetComponent<SpriteRenderer>().sprite = bowlingType.sprite;
    }

    void Update()
    {
        if (GameManager.isPaused) return;
        // Rotate the object around its center (local z-axis) at the defined speed
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Determine the direction of movement
        Vector3 movementDirection = Vector3.right;
        if (moveUp)
        {
            movementDirection += Vector3.up;
        }
        else if(moveDown)
        {
            movementDirection += Vector3.down;
        }

        // Normalize the movement direction to maintain consistent speed
        movementDirection = movementDirection.normalized;

        // Move the object forward and diagonally based on the current direction
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);

        // Check if the object is out of the screen bounds and destroy it if it is
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Zombie>(out Zombie zombie))
        {
            if(bowlingType.canExplode) {
                Instantiate(explodeAnimation, transform.position, Quaternion.identity);
                Invoke("Destroy", 0.4f);
            }
            else {
                float zombieHealth = zombie.GetHealth();
                zombie.Hit(zombieHealth, false);

                if(!moveDown && !moveUp) {
                    moveUp = true;
                }
                else {
                    moveDown = moveUp;
                    moveUp = !moveUp;
                }
            }
        }
    }
    void Destroy() {
        Destroy(gameObject);
    }

    public void SetBowlingType(BowlingType type)
    {
        bowlingType = type;
        gameObject.GetComponent<SpriteRenderer>().sprite = bowlingType.sprite;
    }
}
