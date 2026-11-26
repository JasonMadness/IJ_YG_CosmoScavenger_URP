using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _reachDistance;

    private float _baseSpeed;
    private float _groundY;
    private Player _target;

    private void Awake()
    {
        _baseSpeed = _speed;
        _groundY = transform.position.y;
    }

    public void SetTarget(Player target)
    {
        _target = target;
    }

    public void SetGroundY(float groundY)
    {
        _groundY = groundY;
    }

    public void ApplySpeedMultiplier(float multiplier)
    {
        if (multiplier <= 0f)
            multiplier = 1f;

        _speed = _baseSpeed * multiplier;
    }

    private void Update()
    {
        if (_target == null)
            return;

        Vector3 targetPosition = _target.transform.position;
        Vector3 flatTarget = new Vector3(targetPosition.x, _groundY, targetPosition.z);
        Vector3 toTarget = flatTarget - transform.position;

        if (toTarget.sqrMagnitude > _reachDistance * _reachDistance)
            transform.position += toTarget.normalized * (_speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, _groundY, transform.position.z);
        transform.LookAt(flatTarget);
    }
}