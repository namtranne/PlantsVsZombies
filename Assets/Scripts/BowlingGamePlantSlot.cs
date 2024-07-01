using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BowlingGamePlantSlot : MonoBehaviour
{

    public Image plantImage;
    public Sprite plantSprite;
    public GameObject plantObject;
    private BowlingGameManager gameManager;
    public Button button;

    public LayerMask plantSlotMask;

    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("BowlingGameManager").GetComponent<BowlingGameManager>();
        button.onClick.AddListener(BuyPlant);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.left, 0.5f, plantSlotMask);
        bool stopMoving = false;
        for(int i =0 ; i< hits.Length ; i++) {
            if(hits[i].collider.gameObject != gameObject) {
                stopMoving = true;
                break;
            }
        }
        if (!stopMoving && gameObject.transform.position.x > -8.2)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    // void OnTriggerEnter

    private void BuyPlant() {
        Debug.Log("Click");
        gameManager.BuyPlant(plantObject, plantSprite, gameObject);
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
}
