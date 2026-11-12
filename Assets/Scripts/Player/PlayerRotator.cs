using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    private const float DirectionEpsilonSqr = 0.0001f;

    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Transform _model;
    [SerializeField] private float _rotationSpeed = 720f;

    private Vector3 _moveDirection;

    public void OnMoveDirectionChanged(Vector2 direction)
    {
        _moveDirection = new Vector3(direction.x, 0f, direction.y);
    }

    private void Update()
    {
        Vector3 lookDirection;

        if (_enemyDetector.ClosestEnemy != null)
        {
            lookDirection = _enemyDetector.ClosestEnemy.transform.position - transform.position;
            lookDirection.y = 0f;
        }
        else
        {
            if (_moveDirection.sqrMagnitude < DirectionEpsilonSqr)
                return;

            lookDirection = _moveDirection;
        }

        if (lookDirection.sqrMagnitude < DirectionEpsilonSqr)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        _model.rotation = Quaternion.RotateTowards(
            _model.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }
}