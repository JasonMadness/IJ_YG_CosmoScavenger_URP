using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private float rotationSpeed = 720f;

    private Vector3 _lookDirection;

    public void OnMoveDirectionChanged(Vector2 direction)
    {
        _lookDirection = new Vector3(direction.x, 0f, direction.y);        
    }

    private void Update()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_lookDirection, Vector3.up);

        _model.transform.rotation = Quaternion.RotateTowards(
            _model.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
