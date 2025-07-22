using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;

    public GameObject pauseMenuUI;


    void Start()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


}
