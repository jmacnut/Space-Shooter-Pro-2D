using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private SceneManager _sceneManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            // Reload the current game scene
            // Need to add the scene to File-Build Settings-Add Open Scenes
            // set Build Settings: 0 = "Game" scene, 1 = "Main Menu"

            //SceneManager.LoadScene("Game"); 
            SceneManager.LoadScene(1); // index value is faster than string parameter
            //_isGameOver = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // must build for this to work
            Application.Quit();
        }
    }
    public void GameOver()
    {
        Debug.LogError("GameManager::GameOver() - Called");
        _isGameOver = true;
    }
}
