using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialPlantSlot : MonoBehaviour, IPointerDownHandler
{
    public Image background;
    public Image plantImage;
    public Sprite plantSprite;
    public GameObject plantObject;
    public TextMeshProUGUI priceText;
    public int price;
    private TutorialGameManager gameManager;
    private bool isDragging = false;

    void Start()
    {
        gameManager = GameObject.Find("TutorialGameManager").GetComponent<TutorialGameManager>();
    }

    void Update()
    {
        if (gameManager.suns >= price)
        {
            // background.color = Color.white;
            plantImage.color = Color.white;
        }
        else
        {
            // background.color = Color.gray;
            plantImage.color = Color.gray;
        }
    }

    private void OnValidate()
    {
        if (plantSprite)
        {
            plantImage.sprite = plantSprite;
            plantImage.enabled = true;
        }
        else
        {
            plantImage.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameManager.suns >= price && !gameManager.currentPlant)
        {
            gameManager.ChoosePlant(plantObject, plantSprite, price);
            gameManager.StartDragging();
            isDragging = true;
        }
    }
}
