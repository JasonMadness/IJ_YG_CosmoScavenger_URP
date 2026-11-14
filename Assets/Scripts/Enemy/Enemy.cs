using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private EnemyMeleeAttack _enemyMeleeAttack;
    [SerializeField] private Health _health;

    private Player _player;
    private EnemyPool _enemyPool;
    private Collider[] _colliders;
    private Collider[] _playerColliders;

    public bool IsTargetable => _health.CurrentHealth > 0;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>(true);
    }

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
        _enemyMeleeAttack.SetTarget(_player.GetComponent<Health>());
        IgnorePlayerCollisions();
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }

    private void IgnorePlayerCollisions()
    {
        if (_playerColliders == null)
            _playerColliders = _player.GetComponentsInChildren<Collider>(true);

        foreach (Collider ownCollider in _colliders)
        {
            foreach (Collider playerCollider in _playerColliders)
                Physics.IgnoreCollision(ownCollider, playerCollider);
        }
    }

    private void HandleDeath()
    {
        _enemyPool.Release(this);
    }
}
