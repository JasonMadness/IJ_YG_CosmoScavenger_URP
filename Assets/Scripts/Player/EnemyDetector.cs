using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _enemyLayer;

    private Enemy _closestEnemy;

    public Enemy ClosestEnemy => _closestEnemy;

    private void Update()
    {
        DetectClosestEnemy();
    }

    public void DetectClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _enemyLayer);
        float closestDistance = Mathf.Infinity;
        _closestEnemy = null;

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    _closestEnemy = enemy;
                }
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
