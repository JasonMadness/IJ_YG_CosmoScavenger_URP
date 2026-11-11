using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private float _dampTime = 0.1f;

    private void LateUpdate()
    {
        Vector2 moveDirection2D = _mover.MoveDirection;
        Vector3 moveDirection3D = new Vector3(moveDirection2D.x, 0f, moveDirection2D.y);
        Vector3 localDirection = _model.InverseTransformDirection(moveDirection3D);

        _animator.SetFloat(SpeedHash, _mover.MoveMagnitude, _dampTime, Time.deltaTime);
        _animator.SetFloat(MoveXHash, localDirection.x, _dampTime, Time.deltaTime);
        _animator.SetFloat(MoveYHash, localDirection.z, _dampTime, Time.deltaTime);
    }
}