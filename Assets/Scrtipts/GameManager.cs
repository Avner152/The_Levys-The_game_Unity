using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This class handles the Pause, Exit and Replay functions in the game
public class GameManager : MonoBehaviour
{
    public Teams teams;
    public GameObject menuUI;
    private bool gamePaused;

    // Update is called once per frame
    void Update()
    {
        // If we pressed the Enter key and the game is over, we need to reload it
        if (Input.GetKeyDown(KeyCode.Return) && teams.IsGameOver())
        {
            ReloadGame();
        }
        // If the Escape key is clicked, there are 2 options
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 1) The game is over and the player wants to quit
            if (teams.IsGameOver())
            {
                // If the game is over, Escape means exit the game
                ExitGame();
            }
            // 2) The game is still running, and the player wants to pause it
            else
            {
                // If the game is not paused, show the pause menu
                if (!gamePaused)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        // Stop everything (freeze time), that way nothing can run
        Time.timeScale = 0;
        // Free the cursor so the user can select item in the menu
        Cursor.lockState = CursorLockMode.None;
        // Show the menu
        menuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        gamePaused = false;
        // Resume everything(unfreeze time)
        Time.timeScale = 1;
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the menu
        menuUI.SetActive(false); 
    }
    public void ReloadGame()
    {
        // Resume the game (so time is unfreezed when reloading)
        ResumeGame();
        // And load the current scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
