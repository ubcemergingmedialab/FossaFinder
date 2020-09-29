using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke.
/// Handles all button press navigations in the pause menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    void Start()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void LeaveLevel(int chapter)
    {
        LevelLoaderUtility.GoToLevelSelect();
    }

    public void PauseLevel()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void RestartLevel()
    {
        ObjectiveUtility.Score = 0;
        LevelManager.endGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
