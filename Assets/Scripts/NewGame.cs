using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Button newGameButton;
    private string filePath;

    private void Start()
    {
        // Path where the current level is saved
        filePath = Application.dataPath + "/currentLevel.txt";

        newGameButton.onClick.AddListener(NewGameAction);
    }

    public void NewGameAction()
    {
        File.WriteAllText(filePath, "1");
        GameManager.level = 1;
        SceneManager.LoadScene(1);
    }
}
