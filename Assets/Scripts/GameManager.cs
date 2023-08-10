using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    public bool isCoopMode = false;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            _isGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Current Game Scene
        }

        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     // Application.Quit();
        //     SceneManager.LoadScene(0); //Main Menu
        // }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
 