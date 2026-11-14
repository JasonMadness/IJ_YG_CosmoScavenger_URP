using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] protected float MinDamage;
    [SerializeField] protected float MaxDamage;
    [SerializeField] protected float ShootDelay;

    private float _damageMultiplier = 1f;

    protected Transform Target;

    protected Transform ShootPoint => _shootPoint;

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

    public abstract void Shoot();

    protected float ApplyDamageMultiplier(float damage)
    {
        return damage * _damageMultiplier;
    }
}
