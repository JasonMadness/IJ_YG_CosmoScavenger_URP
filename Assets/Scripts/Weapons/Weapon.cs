using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] protected float MinDamage;
    [SerializeField] protected float MaxDamage;
    [SerializeField] protected float ShootDelay;

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

    public abstract void Shoot();
}
