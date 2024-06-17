using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject currentPlant;
    public Sprite currentPlantSprite;
    public Transform tiles;
    public LayerMask tileMask;
    public int suns;
    public TextMeshProUGUI sunText;
    public LayerMask sunMask;

    public AudioClip plantSFX;
    public AudioClip sunSFX;
    private AudioSource plantSource;
    public AudioSource sunSource;

    private void Start()
    {
        plantSource = GetComponent<AudioSource>();
    }


    public void BuyPlant(GameObject plant, Sprite sprite) {
        currentPlant = plant;
        currentPlantSprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        sunText.text = suns.ToString();

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

        foreach(Transform tile in tiles) {
            tile.GetComponent<SpriteRenderer>().enabled=false;
        }

        if(hit.collider && currentPlant) {
            hit.collider.GetComponent<SpriteRenderer>().sprite = currentPlantSprite;
            hit.collider.GetComponent<SpriteRenderer>().enabled = true;
            if(Input.GetMouseButtonDown(0) && !hit.collider.GetComponent<Tile>().hasPlant) {
                Plant(hit.collider.gameObject);
            }
        }

        RaycastHit2D sunHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, sunMask);
        if (sunHit.collider)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sunSource.pitch = UnityEngine.Random.Range(.9f, 1.1f);
                sunSource.PlayOneShot(sunSFX);
                suns += 25;
                Destroy(sunHit.collider.gameObject);
            }
        }
    }
    
    void Plant(GameObject hitObject)
    {
        plantSource.PlayOneShot(plantSFX);
        Instantiate(currentPlant, hitObject.transform.position, Quaternion.identity);
        currentPlant = null;
        currentPlantSprite = null;
        hitObject.GetComponent<Tile>().hasPlant = true;
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
