using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private EnemyMeleeAttack _enemyMeleeAttack;
    [SerializeField] private Health _health;
    [SerializeField] private int _experienceReward = 10;

    private int _currentExperienceReward;
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
        Initialize(player, enemyPool, null);
    }

    public void Initialize(Player player, EnemyPool enemyPool, EnemyWaveScale scale)
    {
        _player = player;
        _enemyPool = enemyPool;
        _isSpawned = false;

        EnemyWaveScale effectiveScale = scale ?? EnemyWaveScale.Neutral;

        _currentExperienceReward = Mathf.Max(1, Mathf.RoundToInt(_experienceReward * effectiveScale.Experience));

        _health.ApplyMaxHealthMultiplier(effectiveScale.Health);
        _health.Restore();
        _enemyMover.ApplySpeedMultiplier(effectiveScale.Speed);
        _enemyMeleeAttack.ApplyDamageMultiplier(effectiveScale.Damage);
        _enemyMeleeAttack.ApplyAttackSpeedMultiplier(effectiveScale.AttackSpeed);

        _enemyMover.SetTarget(_player);
        _enemyMeleeAttack.SetTarget(_player.GetComponent<Health>());
        IgnorePlayerCollisions();
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;

        if (_enemyMover != null)
            _enemyMover.SetGroundY(position.y);

        _isSpawned = true;

        Physics.SyncTransforms();
    }

    public float GetPivotToFeet()
    {
        float feetY = transform.position.y;

        foreach (Collider ownCollider in _colliders)
        {
            if (ownCollider == null)
                continue;

            feetY = Mathf.Min(feetY, ownCollider.bounds.min.y);
        }

        return transform.position.y - feetY;
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
            _player.GainExperience(_currentExperienceReward);

        if (_enemyPool != null)
            _enemyPool.Release(this);
        else
            Destroy(gameObject);
    }
}
