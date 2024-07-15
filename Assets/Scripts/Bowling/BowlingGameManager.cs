using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BowlingGameManager : MonoBehaviour
{
    public GameObject currentPlant;
    public GameObject currentSelectedSlot;
    public Sprite currentPlantSprite;
    public Transform tiles;
    public LayerMask tileMask;
    public GameObject plant;

    public AudioClip plantSFX;
    private AudioSource plantSource;

    private void Start()
    {
        plantSource = GetComponent<AudioSource>();
    }


    public void BuyPlant(BowlingType bowlingType, GameObject gameObject) {
        currentPlant = plant;
        currentPlant.GetComponent<BowlingWallnut>().SetBowlingType(bowlingType);
        currentPlantSprite = bowlingType.sprite;
        currentSelectedSlot = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

        foreach(Transform tile in tiles) {
            tile.GetComponent<SpriteRenderer>().enabled=false;
        }

        if(hit.collider && currentPlant) {
            hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
            hit.collider.GetComponent<SpriteRenderer>().enabled = true;
            if(Input.GetMouseButtonDown(0) && !hit.collider.GetComponent<Tile>().hasPlant) {
                Plant(hit.collider.gameObject);
                Destroy(currentSelectedSlot);
            }
        }

    }
    
    void Plant(GameObject hitObject)
    {
        plantSource.volume = GameManager.soundVolume;
        plantSource.PlayOneShot(plantSFX);
        Instantiate(currentPlant, hitObject.transform.position, Quaternion.identity);
        currentPlant = null;
        currentPlantSprite = null;
    }

    public void Win()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
