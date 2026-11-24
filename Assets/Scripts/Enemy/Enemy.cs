using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private EnemyMeleeAttack _enemyMeleeAttack;
    [SerializeField] private Health _health;
    [SerializeField] private int _experienceReward = 10;

    private Player _player;
    private EnemyPool _enemyPool;
    private Collider[] _colliders;
    private Collider[] _playerColliders;
    private bool _isSpawned;

    public bool IsTargetable => _isSpawned && _health.CurrentHealth > 0;

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
        _isSpawned = false;
    }

    public void Initialize(Player player, EnemyPool enemyPool)
    {
        _player = player;
        _enemyPool = enemyPool;
        _isSpawned = false;
        _health.Restore();
        _enemyMover.SetTarget(_player);
        _enemyMeleeAttack.SetTarget(_player.GetComponent<Health>());
        IgnorePlayerCollisions();
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        _isSpawned = true;

        // The pooled collider was just re-enabled at its previous (death) position.
        // Without an immediate sync the physics world keeps the stale transform,
        // so EnemyDetector's OverlapSphere can see this enemy next to the player
        // while its transform already sits at the far spawn point.
        Physics.SyncTransforms();
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
        // Босс спавнится не из пула, поэтому ссылки могут не быть.
        // Падение здесь остановило бы весь список подписчиков Died,
        // и учёт волн не получил бы событие смерти.
        if (_player != null)
            _player.GainExperience(_experienceReward);

        if (_enemyPool != null)
            _enemyPool.Release(this);
        else
            Destroy(gameObject);
    }
}
