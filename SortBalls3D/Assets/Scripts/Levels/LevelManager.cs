using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int LevelIndex;

    [SerializeField] private TubeSpawner _tubeSpawner;
    [SerializeField] private List<LevelData> _levels;

    private void Awake()
    {
        LoadLevel(ref LevelIndex);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void LoadLevel(ref int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= _levels.Count)
        {
            Debug.LogWarning("Level Index out of range. So defaulted to first level!");
            levelIndex = 0;
        }
        
        _tubeSpawner.numberOfTubes = _levels[levelIndex].NumberOfTubes;
        _tubeSpawner.ballsPerTube = _levels[levelIndex].BallsPerTube;
        _tubeSpawner.tubeRadius = _levels[levelIndex].TubeRadius;
    }

    public void LoadNextLevel()
    {
        LevelIndex++;
        SceneManager.LoadScene(0);
    }
}