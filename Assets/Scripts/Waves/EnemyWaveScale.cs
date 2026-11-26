public sealed class EnemyWaveScale
{
    public static readonly EnemyWaveScale Neutral = new EnemyWaveScale(1f, 1f, 1f, 1f, 1f);

    private readonly float _health;
    private readonly float _damage;
    private readonly float _speed;
    private readonly float _attackSpeed;
    private readonly float _experience;

    public float Health => _health;

    public float Damage => _damage;

    public float Speed => _speed;

    public float AttackSpeed => _attackSpeed;

    public float Experience => _experience;

    public EnemyWaveScale(float health, float damage, float speed, float attackSpeed, float experience)
    {
        _health = health <= 0f ? 1f : health;
        _damage = damage <= 0f ? 1f : damage;
        _speed = speed <= 0f ? 1f : speed;
        _attackSpeed = attackSpeed <= 0f ? 1f : attackSpeed;
        _experience = experience <= 0f ? 1f : experience;
    }
}
