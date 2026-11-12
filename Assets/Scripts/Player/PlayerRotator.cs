using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    private const float DirectionEpsilonSqr = 0.0001f;

    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Transform _model;
    [SerializeField] private float _rotationSpeed = 720f;

    private Vector3 _lookDirection;
    private Vector3 _moveDirection;
    private bool _enemyInSight;

    public void OnMoveDirectionChanged(Vector2 direction)
    {
        if (direction.sqrMagnitude < DirectionEpsilonSqr)
            return;

        _moveDirection = new Vector3(direction.x, 0f, direction.y);
    }

    private void Update()
    {
        _enemyInSight = _enemyDetector.ClosestEnemy != null;

        if (_enemyInSight)
        {
            _lookDirection = _enemyDetector.ClosestEnemy.transform.position - transform.position;
            _lookDirection.y = 0f;
        }
        else
        {
            if (_moveDirection.sqrMagnitude < DirectionEpsilonSqr)
                return;

            _lookDirection = _moveDirection;            
        }

        Quaternion targetRotation = Quaternion.LookRotation(_lookDirection, Vector3.up);

        _model.rotation = Quaternion.RotateTowards(
            _model.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }
}