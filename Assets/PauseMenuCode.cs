using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuCode : MonoBehaviour
{
    public static bool GamePaused = false;

    public GameObject pauseMenuPanel;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause ()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;   
    }

    public void LoadSettings()
    {
        Debug.Log("Loading settings");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");
        Time.timeScale = 1f;
    }
}
