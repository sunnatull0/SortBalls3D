using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TubeSpawner tubeSpawner;  // Reference to the TubeSpawner script

    private List<LevelData> levels = new List<LevelData>
    {
        new LevelData(2, 2, 2f),   // Level 1: 3 tubes, 3 balls per tube, radius 5
    };

    private int currentLevelIndex = 0;  // Track the current level index

    void Start()
    {
        LoadLevel(currentLevelIndex);  // Load the first level on start
    }

    // Method to load a specific level by index
    public void LoadLevel(int levelIndex)
    {
        Debug.Log("Loaded level: " + levelIndex);
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }

        // Clear the current level before loading a new one
        // tubeSpawner.ClearCurrentLevel();

        // Load the new level with its data
        // tubeSpawner.InitializeLevel(levels[levelIndex]);
    }

    // Method to add new levels
    public void AddNewLevel(LevelData newLevel)
    {
        levels.Add(newLevel);
    }

    // Example of loading the next level
    public void LoadNextLevel()
    {
        currentLevelIndex = (currentLevelIndex + 1) % levels.Count;
        LoadLevel(currentLevelIndex);
    }
}