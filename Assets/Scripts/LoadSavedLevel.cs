using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSavedLevel : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Button continueButton;
    private string filePath;

    private void Start()
    {
        // Path where the current level is saved
        filePath = Application.dataPath + "/currentLevel.txt";

        // Check if the file exists
        if (!File.Exists(filePath))
        {
            // Create the file and write "1" to it
            File.WriteAllText(filePath, "1");
        }

        // Add a listener to the continue button
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ContinueGame);
        }

        // Load the saved level and display it in the Text UI element
        LoadLevelText();
    }

    private void LoadLevelText()
    {
        if (File.Exists(filePath))
        {
            string savedLevel = File.ReadAllText(filePath);
            levelText.text = "Level " + savedLevel;
            Debug.Log("Loaded level: " + savedLevel);
        }
        else
        {
            Debug.LogError("Saved level file not found");
        }
    }

    public void ContinueGame()
    {
        Debug.Log("Check");
        if (File.Exists(filePath))
        {
            string savedLevel = File.ReadAllText(filePath);
            GameManager.level = Int32.Parse(savedLevel);
            if (GameManager.level < 3)
            {
                SceneManager.LoadScene(GameManager.level);
            }
            else if (GameManager.level % 5 == 0)
                SceneManager.LoadScene(4);
            else SceneManager.LoadScene(3);
        }
        else
        {
            Debug.LogError("Saved level file not found");
        }
    }
}