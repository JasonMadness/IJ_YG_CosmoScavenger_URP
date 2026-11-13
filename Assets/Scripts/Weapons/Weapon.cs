using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _minDamage;
    [SerializeField] private float _maxDamage;
    [SerializeField] private float _shootDelay;

    protected Transform Target;

    protected Transform ShootPoint => _shootPoint;
    protected float ShootDelay => _shootDelay;

    public void SetTarget(Enemy target)
    {
        if (target == null)
        {
            Target = null;
            return;
        }

        Target = target.transform;
    }

    public abstract void Shoot();
}
