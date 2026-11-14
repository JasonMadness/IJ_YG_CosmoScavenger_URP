using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private int _initialLevel = 1;
    [SerializeField] private int _baseRequirement = 30;
    [SerializeField] private int _requirementIncrease = 20;

    private int _level;
    private int _currentExperience;
    private int _requiredExperience;

    public int Level => _level;

    public int CurrentExperience => _currentExperience;

    public int RequiredExperience => _requiredExperience;

    public float Progress => _requiredExperience <= 0 ? 0f : (float)_currentExperience / _requiredExperience;

    public event Action Changed;

    public event Action LevelIncreased;

    private void Awake()
    {
        _level = _initialLevel;
        _requiredExperience = _baseRequirement;
    }

    public void Add(int amount)
    {
        if (amount <= 0)
            return;

        _currentExperience += amount;

        while (_currentExperience >= _requiredExperience)
        {
            _currentExperience -= _requiredExperience;
            _level++;
            _requiredExperience += _requirementIncrease;
            LevelIncreased?.Invoke();
        }

        Changed?.Invoke();
    }
}
