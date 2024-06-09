using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [ContextMenu("Play Game")]
    public void PlayGame() {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
