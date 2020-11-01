using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        // Build Settings
        // 0 = "Game" scene
        // 1 = "Main Menu"
        SceneManager.LoadScene(1);
    }
}
