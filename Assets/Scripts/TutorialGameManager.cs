using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class TutorialGameManager : MonoBehaviour
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

    public GameObject sunObject;
    public GameObject tutorialHand;
    public GameObject chatBlog;

    public GameObject zombie;
    public Transform spawnPoint;

    private bool isDragging = false;
    private GameObject draggedPlantInstance;
    private int currentPlantPrice;

    private int zombieCount = 0;
    private int tutorialStep = 1;

    private void Start()
    {
        plantSource = GetComponent<AudioSource>();
    }

    public void ChoosePlant(GameObject plant, Sprite sprite, int price)
    {
        currentPlant = plant;
        currentPlantSprite = sprite;
        currentPlantPrice = price;
    }

    void Update()
    {
        sunText.text = suns.ToString();
        HandleTutorialSteps();
        HandleDragging();
        HandleSunCollection();
    }

    private void HandleTutorialSteps()
    {
        switch (tutorialStep)
        {
            case 1:
                StartTutorialStep1();
                break;
            case 2:
                UpdateTutorialStep2();
                break;
            case 3:
                UpdateTutorialStep3();
                break;
            case 4:
                StartTutorialStep4();
                break;
            case 5:
                UpdateTutorialStep5();
                break;
            case 6:
                StartTutorialStep6();
                break;
            case 7:
                UpdateTutorialStep7();
                break;
            case 8:
                StartTutorialStep8();
                break;
            case 9:
                UpdateTutorialStep9();
                break;
            case 10:
                StartTutorialStep10();
                break;
            case 11:
                UpdateTutorialStep11();
                break;
            case 12:
                StartTutorialStep12();
                break;
        }
    }

    private void StartTutorialStep1()
    {
        SpawnSun();
        tutorialStep++;
    }

    private void UpdateTutorialStep2()
    {
        GameObject sunObject = GameObject.Find("TutorialSun(Clone)");
        if (sunObject != null)
        {
            TutorialSun sun = sunObject.GetComponent<TutorialSun>();
            if (sun.transform.position.y <= sun.dropToYPos)
            {
                ShowHandAndChat(sunObject.transform.position + new Vector3(0.55f, -0.55f, 0),
                    sunObject.transform.position + new Vector3(0.85f, -0.85f, 0), "Collect the suns to buy plants!!");
                tutorialStep++;
            }
        }
        else if (suns == 50)
        {
            tutorialStep++;
        }
    }

    private void UpdateTutorialStep3()
    {
        if (suns == 50)
        {
            DestroyObject("TutorialHand(Clone)");
            DestroyObject("ChatBlog(Clone)");
            tutorialStep++;
        }
    }

    private void StartTutorialStep4()
    {
        ShowHandAndChat(new Vector3(-6.57f, 3.48f, 0), new Vector3(-3.27f, -1.06f, 0),
            "You will need more sun in order to buy plants. Drag and drop the sunflower onto the grass. The sunflower will help you generate suns, while the repeater will help you fight the ninjas...");
        tutorialStep++;
    }

    private void UpdateTutorialStep5()
    {
        if (suns == 0)
        {
            DestroyObject("TutorialHand(Clone)");
            DestroyObject("ChatBlog(Clone)");
            tutorialStep++;
        }
    }

    private void StartTutorialStep6()
    {
        TutorialPlantSlot sunPlatSlot = GameObject.Find("Plant").GetComponent<TutorialPlantSlot>();
        sunPlatSlot.plantObject = null;
        SpawnSun();
        Invoke("SpawnSun", 4);
        Invoke("SpawnSun", 7);
        ShowChat("Collect more suns to buy Repeaterrr!!!!");
        tutorialStep++;
    }

    private void UpdateTutorialStep7()
    {
        if (suns == 100)
        {
            GameObject chat = GameObject.Find("ChatBlog(Clone)");
            chat.GetComponentInChildren<TextMeshProUGUI>().text = "Plant the repeater";
            tutorialStep++;
        }
    }

    private void StartTutorialStep8()
    {
        ShowHand(new Vector3(-5.57f, 3.48f, 0), new Vector3(-2.27f, -1.06f, 0));
        tutorialStep++;
    }

    private void UpdateTutorialStep9()
    {
        GameObject repeater = GameObject.Find("Repeater(Clone)");
        if (repeater != null)
        {
            DestroyObject("TutorialHand(Clone)");
            DestroyObject("ChatBlog(Clone)");
            tutorialStep++;
        }
    }

    private void StartTutorialStep10()
    {
        ShowChat("The Repeater will protect your house from the scary ninjas. You should protect all the paths to your house; otherwise, the ninjas will come and kill you!");
        tutorialStep++;
    }

    private void UpdateTutorialStep11()
    {
        if (FindObjectOfType<Zombie>() == null)
        {
            if (zombieCount == 2)
            {
                tutorialStep++;
                return;
            }
            zombieCount++;
            for (int i = 0; i < 3; i++)
            {
                Invoke("SpawnZombie", i);
            }
        }
    }

    private void StartTutorialStep12()
    {
        DestroyObject("ChatBlog(Clone)");
        ShowChat("Congratulations, you have completed the tutorial! Let's start your journey to protect your house from the ninjas.");
    }

    private void HandleDragging()
    {
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

                ResetDragging();
            }
        }
    }

    private void HandleSunCollection()
    {
        RaycastHit2D sunHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, sunMask);
        if (sunHit.collider)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sunSource.pitch = Random.Range(.9f, 1.1f);
                sunSource.PlayOneShot(sunSFX);
                suns += 25;
                Destroy(sunHit.collider.gameObject);
            }
        }
    }

    private void ShowHandAndChat(Vector3 handStart, Vector3 handEnd, string chatText)
    {
        ShowHand(handStart, handEnd);
        ShowChat(chatText);
    }

    private void ShowHand(Vector3 startPosition, Vector3 endPosition)
    {
        GameObject hand = Instantiate(tutorialHand);
        MoveDiagonal move = hand.GetComponent<MoveDiagonal>();
        move.SetPosition(startPosition, endPosition);
    }

    private void ShowChat(string message)
    {
        GameObject chat = Instantiate(chatBlog);
        TextMeshProUGUI tmpText = chat.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = message;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found in children.");
        }
    }

    private void DestroyObject(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    private void ResetDragging()
    {
        currentPlant = null;
        currentPlantSprite = null;
        Destroy(draggedPlantInstance);
        draggedPlantInstance = null;
        isDragging = false;
    }

    public void StartDragging()
    {
        isDragging = true;
    }

    public bool GetIsDragging()
    {
        return isDragging;
    }

    public void UpdateDraggedPlantPosition(Vector3 position)
    {
        if (draggedPlantInstance != null)
        {
            draggedPlantInstance.transform.position = position;
        }
    }

    private void Plant(GameObject hitObject)
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
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SpawnSun()
    {
        GameObject mySun = Instantiate(sunObject, new Vector3(Random.Range(-8f, 8f), 6, 0), Quaternion.identity);
        mySun.GetComponent<TutorialSun>().dropToYPos = Random.Range(0.0f, -1.0f);
    }

    public void SpawnZombie()
    {
        Instantiate(zombie, spawnPoint.position, Quaternion.identity);
    }
}
