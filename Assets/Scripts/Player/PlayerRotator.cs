using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    private const float DirectionEpsilonSqr = 0.0001f;

    [SerializeField] private Transform _model;
    [SerializeField] private float _rotationSpeed = 720f;

    private Vector3 _lookDirection;

    public void OnMoveDirectionChanged(Vector2 direction)
    {
        if (direction.sqrMagnitude < DirectionEpsilonSqr)
            return;

        _lookDirection = new Vector3(direction.x, 0f, direction.y);
    }

    private void Update()
    {
        if (_lookDirection.sqrMagnitude < DirectionEpsilonSqr)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(_lookDirection, Vector3.up);

        _model.rotation = Quaternion.RotateTowards(
            _model.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }
}