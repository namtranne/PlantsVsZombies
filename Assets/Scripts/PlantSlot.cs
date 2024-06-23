using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlantSlot : MonoBehaviour, IPointerDownHandler
{
    public Image background;
    public Image plantImage;
    public Sprite plantSprite;
    public GameObject plantObject;
    public TextMeshProUGUI priceText;
    public int price;
    private GameManager gameManager;
    private bool isDragging = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.suns >= price)
        {
            background.color = Color.white;
            plantImage.color = Color.white;
        }
        else
        {
            background.color = Color.gray;
            plantImage.color = Color.gray;
        }
    }

    private void OnValidate()
    {
        if (plantSprite)
        {
            plantImage.sprite = plantSprite;
            plantImage.enabled = true;
            priceText.text = price.ToString();
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
