using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private const float MinShootDelayMultiplier = 0.1f;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] protected float MinDamage;
    [SerializeField] protected float MaxDamage;
    [SerializeField] protected float ShootDelay;

    private float _damageMultiplier = 1f;
    private float _shootDelayMultiplier = 1f;

    protected Transform Target;

    protected Transform ShootPoint => _shootPoint;

    protected float CurrentShootDelay => ShootDelay * _shootDelayMultiplier;

    public void SetTarget(Enemy target)
    {
        if (target == null)
        {
            Target = null;
            return;
        }

        Target = target.transform;
    }

    public void IncreaseDamage(float percent)
    {
        _damageMultiplier += percent;
    }

    public void IncreaseAttackSpeed(float percent)
    {
        _shootDelayMultiplier = Mathf.Max(
            _shootDelayMultiplier * (1f - percent),
            MinShootDelayMultiplier);
    }

    public abstract void Shoot();

    protected float ApplyDamageMultiplier(float damage)
    {
        return damage * _damageMultiplier;
    }
}
