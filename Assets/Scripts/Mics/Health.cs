using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _currentHealth;
    private bool _isDead;

    public float CurrentHealth => _currentHealth;

    public float MaxHealth => _maxHealth;

    public event Action Died;

    public event Action HealthChanged;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        _currentHealth -= damage;
        HealthChanged?.Invoke();

        if (_currentHealth <= 0)
            Die();
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
