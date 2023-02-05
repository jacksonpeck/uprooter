using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;
    public void ResumeGame()
    {
        pauseMenuUI.SetActive( false );
        Time.timeScale = 1f;
        paused = false;
    }
}
