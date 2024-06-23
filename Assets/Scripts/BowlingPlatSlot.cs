using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BowlingPlatSlot : MonoBehaviour
{

    public Image plantImage;
    public Sprite plantSprite;
    public GameObject plantObject;
    public TextMeshProUGUI priceText;
    public int price;
    private BowlingGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("BowlingGameManager").GetComponent<BowlingGameManager>();
        GetComponent<Button>().onClick.AddListener(BuyPlant);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuyPlant() {
        if (!gameManager.currentPlant)
        {
            gameManager.BuyPlant(plantObject, plantSprite);
        }
    }

    private void OnValidate() {
        if(plantSprite) {
            plantImage.sprite = plantSprite;
            plantImage.enabled = true;
            priceText.text = price.ToString();
        }
        else {
            plantImage.enabled = false;
        }
    }
}
