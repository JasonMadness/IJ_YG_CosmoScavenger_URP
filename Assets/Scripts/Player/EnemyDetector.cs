using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private const int MaxColliders = 32;

    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _enemyLayer;

    private readonly Collider[] _buffer = new Collider[MaxColliders];
    private Enemy _closestEnemy;

    public Enemy ClosestEnemy => _closestEnemy;

    private void Update()
    {
        DetectClosestEnemy();
    }

    public void DetectClosestEnemy()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position, _radius, _buffer, _enemyLayer);

        float closestDistance = Mathf.Infinity;
        _closestEnemy = null;

        for (int i = 0; i < count; i++)
        {
            if (_buffer[i].TryGetComponent(out Enemy enemy) == false)
                continue;

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;

            if (sqrDistance < closestDistance)
            {
                closestDistance = sqrDistance;
                _closestEnemy = enemy;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}