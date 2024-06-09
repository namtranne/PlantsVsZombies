using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject currentPlant;
    public Sprite currentPlantSprite;
    public Transform tiles;

    public LayerMask tileMask;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void BuyPlant(GameObject plant, Sprite sprite) {
        currentPlant = plant;
        currentPlantSprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

        foreach(Transform tile in tiles) {
            tile.GetComponent<SpriteRenderer>().enabled=false;
        }

        if(hit.collider && currentPlant ) {
            hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
            hit.collider.GetComponent<SpriteRenderer>().enabled = true;
            if(Input.GetMouseButtonDown(0) && !hit.collider.GetComponent<Tile>().hasPlant) {
                Instantiate(currentPlant, hit.collider.transform.position, Quaternion.identity);
                currentPlant = null;
                currentPlantSprite = null;
                hit.collider.GetComponent<Tile>().hasPlant = true;
            }
        }
    }
}
