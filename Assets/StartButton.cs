using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public  Button button;
    private GameManager gameManager;

    void Start()
    {
        button.onClick.AddListener(startGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startGame()
    {
        Debug.Log("Start Game");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.StartGame();
    }
   
}
