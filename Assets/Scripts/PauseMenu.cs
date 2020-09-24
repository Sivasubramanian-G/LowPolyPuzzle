using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject inGameUI;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void Pause()
    {
            pauseMenuUI.SetActive(true);
            inGameUI.SetActive(false);
            Time.timeScale = 0f;
            gamePaused = true;
    }

}
