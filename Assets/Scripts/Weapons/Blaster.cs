using TMPro.EditorUtilities;
using UnityEngine;

public class Blaster : Weapon
{
    [SerializeField] private ProjectilePool _projectilePool;

    private float _nextShootTime;

    public override void Shoot()
    {
        if (Time.time < _nextShootTime)
            return;

        if (Target == null)
            return;

        _nextShootTime = Time.time + ShootDelay;

        Projectile projectile = _projectilePool.Get();
        projectile.transform.position = ShootPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(Target.position - ShootPoint.position);
    }
}
