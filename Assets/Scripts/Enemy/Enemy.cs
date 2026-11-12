using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private float _maxHealth = 3f;
    
    private float _currentHealth;

    public bool IsTargetable => _currentHealth > 0;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _enemyMover.SetTarget(_player);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}