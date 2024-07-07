using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Button backToGameButton;
    public Button mainMenuButton;
    public Button restartLevelButton;
    public Button saveAndExitButton;
    private Animator[] animators;

    // Start is called before the first frame update
    void Start()
    {
        if (backToGameButton != null)
        {
            backToGameButton.onClick.AddListener(ResumeGame);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        if (restartLevelButton != null)
        {
            restartLevelButton.onClick.AddListener(RestartLevel);
        }

        if (saveAndExitButton != null)
        {
            saveAndExitButton.onClick.AddListener(SaveAndExit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            GameManager.StopPausing();
            ResumeAllAnimations();
        }
    }

    void GoToMainMenu()
    {
        // Replace "MainMenu" with the name of your main menu scene
        SceneManager.LoadScene(0);
    }

    void RestartLevel()
    {
        // Reload the current level
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void SaveAndExit()
    {
        // Save current level to a file
        string filePath = Application.dataPath + "/currentLevel.txt";
        string currentLevel = "1";

        File.WriteAllText(filePath, currentLevel);
        Debug.Log("Current level saved to " + filePath);     
        Debug.Log("Game Saved");

        // Replace "MainMenu" with the name of your main menu scene
        SceneManager.LoadScene(0);
    }

    public void ResumeAllAnimations()
    {
        // Find all Animator components in the scene
        Animator[] allAnimators = FindObjectsOfType<Animator>();

        // Loop through all found Animators and enable them
        foreach (Animator animator in allAnimators)
        {
            animator.enabled = true;
        }
    }
}
