using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Teams teams;
    public GameObject menuUI;
    private bool gamePaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && teams.IsGameOver())
        {
            ReloadGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (teams.IsGameOver())
            {
                // If the game is over, Escape means exit the game
                ExitGame();
            }
            else
            {
                // If the games still runs, show the pause menu
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
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        menuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        menuUI.SetActive(false); 
    }
    public void ReloadGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
