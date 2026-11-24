using System;
using UnityEngine;

[Serializable]
public sealed class WaveBalance
{
    [SerializeField] private int _firstWaveCount = 10;
    [SerializeField] private int _growthStep = 5;
    [SerializeField] private float _spawnInterval = 0.8f;
    [SerializeField] private float _pauseBetweenWaves = 5f;

    public int FirstWaveCount => _firstWaveCount;

    public int GrowthStep => _growthStep;

    public float SpawnInterval => _spawnInterval;

    public float PauseBetweenWaves => _pauseBetweenWaves;
}
