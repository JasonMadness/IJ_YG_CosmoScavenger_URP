using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _reachDistance;

    private float _positionY;
    private Player _target;

    private void Awake()
    {
        _positionY = transform.position.y;
    }

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

            transform.position = new Vector3(transform.position.x, _positionY, transform.position.z);
            transform.LookAt(_target.transform.position + Vector3.up * _positionY);
        }
    }
}
