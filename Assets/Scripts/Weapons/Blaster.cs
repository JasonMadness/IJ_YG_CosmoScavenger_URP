using UnityEngine;

public class Blaster : Weapon
{
    private const float MinAimSqrMagnitude = 0.0001f;

    [SerializeField] private ProjectilePool _projectilePool;

    private float _nextShootTime;

    public override void Shoot()
    {
        if (Time.time < _nextShootTime)
            return;

        if (Target == null)
            return;

        float damage = ApplyDamageMultiplier(Random.Range(MinDamage, MaxDamage));

        _nextShootTime = Time.time + CurrentShootDelay;

        Projectile projectile = _projectilePool.Get();
        projectile.transform.position = ShootPoint.position;
        projectile.transform.rotation = GetAimRotation();
        projectile.SetDamage(damage);
    }

    private Quaternion GetAimRotation()
    {
        Vector3 toTarget = Target.position - ShootPoint.position;

        if (toTarget.sqrMagnitude < MinAimSqrMagnitude)
            return ShootPoint.rotation;

        return Quaternion.LookRotation(toTarget);
    }
}
