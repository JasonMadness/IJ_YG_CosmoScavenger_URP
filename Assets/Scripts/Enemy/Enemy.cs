using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private Health _health;

    private Player _player;
    private EnemyPool _enemyPool;

    public bool IsTargetable => _health.CurrentHealth > 0;

    private void OnEnable()
    {
        _health.Died += HandleDeath;
    }

    private void OnDisable()
    {
        _health.Died -= HandleDeath;
    }

    public void Initialize(Player player, EnemyPool enemyPool)
    {
        _player = player;
        _enemyPool = enemyPool;
        _health.Restore();
        _enemyMover.SetTarget(_player);
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }

    private void HandleDeath()
    {
        _enemyPool.Release(this);
    }
}
