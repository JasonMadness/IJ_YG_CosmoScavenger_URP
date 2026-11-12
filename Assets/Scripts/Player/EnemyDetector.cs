using System;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private const int MaxColliders = 32;

    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _enemyLayer;

    private readonly Collider[] _buffer = new Collider[MaxColliders];

    public event Action<Enemy> ClosestEnemyDetected;

    private void Update()
    {
        DetectClosestEnemy();
    }

    public void DetectClosestEnemy()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position, _radius, _buffer, _enemyLayer);

        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        for (int i = 0; i < count; i++)
        {
            if (_buffer[i].TryGetComponent(out Enemy enemy) == false)
                continue;

            if (enemy.IsTargetable == false)
                continue;

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;

            if (sqrDistance < closestDistance)
            {
                closestDistance = sqrDistance;
                closestEnemy = enemy;
            }
        }

        ClosestEnemyDetected?.Invoke(closestEnemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}