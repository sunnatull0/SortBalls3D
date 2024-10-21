using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;

    [SerializeField] private List<ParticleSystem> _levelComplete;
    [SerializeField] private GameObject _ballRemoveEffect;
    [SerializeField] private GameObject _ballSpawnEffect;

    public Action<Transform, Transform> OnBallRemoved;
    public Action<Transform, GameObject> OnBallSpawnedAtTop;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        TubeSpawner.OnLevelComplete += OnLevelComplete;
        OnBallRemoved += PlayRemoveEffect;
        OnBallSpawnedAtTop += PlaySpawnEffectAtTop;
    }

    private void PlaySpawnEffectAtTop(Transform pos, GameObject obj)
    {
        Instantiate(_ballSpawnEffect, pos.position, Quaternion.identity, obj.transform);
    }

    private void PlayRemoveEffect(Transform pos, Transform tube)
    {
        Instantiate(_ballRemoveEffect, pos.position, Quaternion.identity, tube.transform);
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