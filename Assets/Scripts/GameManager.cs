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
    public GameObject panel;

    public AudioClip plantSFX;
    public AudioClip sunSFX;
    private AudioSource plantSource;
    public AudioSource sunSource;

    private bool isDragging = false;
    private GameObject draggedPlantInstance;
    private int currentPlantPrice;


    public static bool isSelecting = true;
    public static bool isPaused = true;
    public static int level = 1;

    public float gameTime { get; private set; }


    public static void StopPausing()
    {
        isPaused = false;
    }

    public static void Pausing()
    {
        isPaused = true;
    }

    private void Start()
    {
        plantSource = GetComponent<AudioSource>();
        isSelecting = true;
        isPaused = true;
        gameTime = 0f;
    }

    public void ChoosePlant(GameObject plant, Sprite sprite, int price)
    {
        currentPlant = plant;
        currentPlantSprite = sprite;
        currentPlantPrice = price;
    }

    void Update()
    {
        if (!isPaused)
        {
            gameTime += Time.deltaTime;
        }

        if (isPaused) return;

        sunText.text = suns.ToString();

        if (isDragging && currentPlant)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (draggedPlantInstance == null)
            {
                draggedPlantInstance = new GameObject("DraggedPlant");
                var spriteRenderer = draggedPlantInstance.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = currentPlantSprite;
            }

            draggedPlantInstance.transform.position = mousePosition;
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

                if (hit.collider && !hit.collider.GetComponent<Tile>().hasPlant)
                {
                    Plant(hit.collider.gameObject);
                    suns -= currentPlantPrice;
                }

                currentPlant = null;
                currentPlantSprite = null;
                Destroy(draggedPlantInstance);
                draggedPlantInstance = null;
                isDragging = false;
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

    public void StartDragging()
    {
        isDragging = true;
    }

    public bool getIsDragging()
    {
        return isDragging;
    }

    void Plant(GameObject hitObject)
    {
        plantSource.PlayOneShot(plantSFX);
        GameObject plant = Instantiate(currentPlant, hitObject.transform.position, Quaternion.identity);
        hitObject.GetComponent<Tile>().hasPlant = true;
        plant.GetComponent<Plant>().tile = hitObject.GetComponent<Tile>();
    }

    public void Win()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartGame()
    {
        panel.SetActive(false);
        isSelecting = false;
        isPaused = false;
    }

    public void AddSun(int value)
    {
        suns += value;
        sunText.text = suns.ToString();
        sunSource.pitch = UnityEngine.Random.Range(.9f, 1.1f);
        sunSource.PlayOneShot(sunSFX);
    }

    public float GetDifficulty()
    {
        return 1f + (level - 1) * 0.2f + gameTime / 300f; 
    }

    public bool CanAfford(int cost)
    {
        return suns >= cost;
    }

    public void SpendSun(int cost)
    {
        suns -= cost;
        sunText.text = suns.ToString();
    }
}