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
        if (_target == null)
            return;

        Vector3 targetPosition = _target.transform.position;
        Vector3 flatTarget = new Vector3(targetPosition.x, _positionY, targetPosition.z);
        Vector3 toTarget = flatTarget - transform.position;

        if (toTarget.sqrMagnitude > _reachDistance * _reachDistance)
            transform.position += toTarget.normalized * (_speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, _positionY, transform.position.z);
        transform.LookAt(flatTarget);
    }
}