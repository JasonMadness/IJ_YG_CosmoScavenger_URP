using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private Health _health;

    public bool IsTargetable => _health.CurrentHealth > 0;

    private void Start()
    {
        _enemyMover.SetTarget(_player);
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }
}