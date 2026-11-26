using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private const float MaxResistance = 0.9f;

    [SerializeField] private float _maxHealth;

    private float _baseMaxHealth;
    private float _currentHealth;
    private float _damageResistance;
    private bool _isDead;

    public float CurrentHealth => _currentHealth;

    public float MaxHealth => _maxHealth;

    public event Action Died;

    public event Action HealthChanged;

    private void Awake()
    {
        _baseMaxHealth = _maxHealth;
        _currentHealth = _maxHealth;
    }

    public void ApplyMaxHealthMultiplier(float multiplier)
    {
        if (multiplier <= 0f)
            multiplier = 1f;

        _maxHealth = _baseMaxHealth * multiplier;
    }

    public void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        _currentHealth -= damage * (1f - _damageResistance);
        HealthChanged?.Invoke();

        if (_currentHealth <= 0)
            Die();
    }

    public void AddDamageResistance(float amount)
    {
        _damageResistance = Mathf.Clamp(_damageResistance + amount, 0f, MaxResistance);
    }

    public void Restore()
    {
        _currentHealth = _maxHealth;
        _isDead = false;
        HealthChanged?.Invoke();
    }

    private void Die()
    {
        _isDead = true;
        Died?.Invoke();
        gameObject.SetActive(false);
    }
}
