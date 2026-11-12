using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _reachDistance;

    private Player _target;

    public void SetTarget(Player target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;

            if (Vector3.Distance(transform.position, _target.transform.position) > _reachDistance)
                transform.position += direction * _speed * Time.deltaTime;

            transform.LookAt(_target.transform.position);
        }
    }
}
