using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _hand;

    private Enemy _target;

    public void OnEnemyDetected(Enemy enemy)
    {
        _target = enemy;
        _weapon.SetTarget(_target);
    }

    public void IncreaseDamage(float percent)
    {
        _weapon.IncreaseDamage(percent);
    }

    private void Update()
    {
        if (_target == null)
            return;
        if (_target.IsTargetable == false)
            return;

        _weapon.Shoot();
    }
}
