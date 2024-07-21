using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BowlingGamePlantSlot : MonoBehaviour, IPointerDownHandler
{

    public Image plantImage;
    public Sprite plantSprite;
    public GameObject plantObject;
    //private BowlingGameManager gameManager;
    private GameManager gameManager;
    //public Button button;

    public LayerMask plantSlotMask;
    public BowlingType bowlingType;

    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.Find("BowlingGameManager").GetComponent<BowlingGameManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //button.onClick.AddListener(BuyPlant);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused)
        {
            return;
        }
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.left, 1, plantSlotMask);
        bool stopMoving = false;
        for(int i =0 ; i< hits.Length ; i++) {
            if(hits[i].collider.gameObject != gameObject) {
                stopMoving = true;
                break;
            }
        }
        if (!stopMoving && gameObject.transform.position.x > -8.1f)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    // void OnTriggerEnter

    private void BuyPlant()
    {
        gameManager.BowlingChoosePlant(bowlingType, gameObject);
    }

    private void OnValidate() {
        if(plantSprite) {
            plantImage.sprite = plantSprite;
            plantImage.enabled = true;
        }
        else {
            plantImage.enabled = false;
        }
    }

    public void UpdatePlant(BowlingType type) {
        bowlingType = type;
        plantImage.enabled = true;
        plantImage.sprite = bowlingType.sprite;
        plantSprite = bowlingType.sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.BowlingChoosePlant(bowlingType, gameObject);
        gameManager.StartDragging();
    }
}
