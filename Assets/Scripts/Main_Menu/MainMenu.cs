using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene(1); //main game scene
    }

    public void LoadMultiplayer()
    {
        SceneManager.LoadScene(2); //co-op game
    }

    public void Exit()
    {
        Application.Quit();
    }
}
