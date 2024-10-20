using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _levelComplete;

    private void Start()
    {
        TubeSpawner.OnLevelComplete += OnLevelComplete;
    }

    private void OnDestroy()
    {
        TubeSpawner.OnLevelComplete -= OnLevelComplete;
    }

    private void OnLevelComplete()
    {
        foreach (var effect in _levelComplete)
        {
            effect.Play();
        }
    }
}