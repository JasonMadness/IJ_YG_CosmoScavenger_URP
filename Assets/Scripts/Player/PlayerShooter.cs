using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _hand;

    private Enemy _target;
    private bool _hasTarget;

    public void OnEnemyDetected(Enemy enemy)
    {
        _target = enemy;
        _hasTarget = enemy != null;
        _weapon.SetTarget(_target);
    }

    public void IncreaseDamage(float percent)
    {
        _weapon.IncreaseDamage(percent);
    }

    public void IncreaseAttackSpeed(float percent)
    {
        _weapon.IncreaseAttackSpeed(percent);
    }

    private void Update()
    {
        if (_target == null)
            return;
        if (_hasTarget == false)
            return;
        if (_target.IsTargetable == false)
            return;

        _weapon.Shoot();
    }
}
