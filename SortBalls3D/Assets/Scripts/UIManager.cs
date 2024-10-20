using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel; // The UI panel for pausing the game
    public GameObject completePanel; // The panel that will show after game completion

    private bool isPaused = false; // Bool to track the paused state

    void Start()
    {
        pausePanel.SetActive(false);
        completePanel.SetActive(false);

        TubeSpawner.OnLevelComplete += OnLevelComplete;
    }

    private void OnDestroy()
    {
        TubeSpawner.OnLevelComplete -= OnLevelComplete;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Method to pause the game
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pauses the game
        pausePanel.SetActive(true); // Show the pause panel

        // Show the panel with a smooth animation in unscaled time
        pausePanel.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void ResumeGame()
    {
        // Resumes the game
        pausePanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).SetUpdate(true)
            .OnComplete(() =>
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
                isPaused = false;
            });
    }

    public void ShowCompletionPanel()
    {
        // Disable tube rotations and ball behaviors here
        completePanel.SetActive(true); // Show the completion panel

        // Smoothly scale the panel in with unscaled time animation
        completePanel.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack).SetUpdate(true);
    }

    // Optional: Call this method when the level is completed
    public void OnLevelComplete()
    {
        // First, pause the game or disable controls as needed
        ShowCompletionPanel();
        //isPaused = true;
        //Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}