using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private float _attackRange = 1.5f;

    private Health _targetHealth;
    private float _nextAttackTime;

    public void SetTarget(Health targetHealth)
    {
        _targetHealth = targetHealth;
        _nextAttackTime = Time.time + _attackInterval;
    }

    private void Update()
    {
        if (_targetHealth == null || _targetHealth.CurrentHealth <= 0)
            return;

        if (Time.time < _nextAttackTime)
            return;

        if (GetFlatDistance() > _attackRange)
            return;

        _targetHealth.TakeDamage(_damage);
        _nextAttackTime = Time.time + _attackInterval;
    }

    private float GetFlatDistance()
    {
        Vector3 toTarget = _targetHealth.transform.position - transform.position;
        return new Vector2(toTarget.x, toTarget.z).magnitude;
    }
}
