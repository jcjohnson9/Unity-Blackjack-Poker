using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;// place pause menu object here

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    //method to pause game
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);//UI is activated
        Time.timeScale = 0f; // Pause the game
    }

    //method to resume game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);//UI is deactivated
        Time.timeScale = 1f; // Resume the game
    }

    //method to go back to the main menu
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Make sure to resume time before loading the scene
        SceneManager.LoadScene("Main Menu"); // Replace "MainMenu" with your main menu scene name
    }

    //method to exit the application
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}