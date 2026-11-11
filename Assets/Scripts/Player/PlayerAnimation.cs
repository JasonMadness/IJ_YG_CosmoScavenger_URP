using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;
    [SerializeField] private PlayerMover _mover;

    private void Update()
    {
        if (_animator == null || _model == null || _mover == null) 
            throw new System.NullReferenceException("One or more required references are not assigned in the PlayerAnimation script.");

        Vector2 moveDir2D = _mover.MoveDirection;
        bool isMoving = moveDir2D.sqrMagnitude > 0.0001f;

        _animator.SetFloat("Speed", isMoving ? 1f : 0f);

        if (isMoving == false)
        {
            _animator.SetFloat("MoveX", 0f);
            _animator.SetFloat("MoveY", 0f);
            return;
        }

        Vector3 lookDir = _model.forward;
        lookDir.y = 0f;
        lookDir.Normalize();

        Vector3 moveDir3D = new Vector3(moveDir2D.x, 0f, moveDir2D.y);

        float angle = Vector3.SignedAngle(lookDir, moveDir3D, Vector3.up);
        float rad = angle * Mathf.Deg2Rad;

        float moveX = Mathf.Sin(rad);
        float moveY = Mathf.Cos(rad);

        _animator.SetFloat("MoveX", moveX);
        _animator.SetFloat("MoveY", moveY);
    }
}